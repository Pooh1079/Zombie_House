using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    [Header("Inventory")]
    public Transform turretGrid;

    [Header("Info Panel")]
    public GameObject infoPanel;

    public Image infoIcon;

    public TMP_Text nameText;

    public TMP_Text rarityText;

    public TMP_Text statsText;

    [Header("Loadout")]
    public Image[] loadoutSlots;

    [Header("Paging")]
    public RectTransform turretGridRect;

    public GridLayoutGroup gridLayout;

    public int visibleRows = 2;

    public int columns = 3;

    private int currentRow = 0;

    private TurretData currentTurret;

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }

        // Ждём один кадр, чтобы GameSaveSystem
        // успел загрузить сохранение.
        yield return null;

        RefreshInventory();

        RefreshLoadout();

        ResetPaging();
    }

    // =====================================================
    // INVENTORY
    // =====================================================

    public void RefreshInventory()
    {
        if (TurretDatabase.Instance == null)
        {
            Debug.LogError(
                "InventoryUI: TurretDatabase не найден!"
            );

            return;
        }

        if (turretGrid == null)
        {
            Debug.LogError(
                "InventoryUI: TurretGrid не назначен!"
            );

            return;
        }

        // Удаляем старые карточки.
        // Это НЕ удаляет UpButton / DownButton,
        // потому что они должны находиться ВНЕ TurretGrid.
        for (
            int i = turretGrid.childCount - 1;
            i >= 0;
            i--
        )
        {
            Destroy(
                turretGrid.GetChild(i).gameObject
            );
        }

        var turrets =
            TurretDatabase.Instance
                .GetUnlockedTurrets();

        foreach (
            TurretData turret
            in turrets)
        {
            if (turret == null)
                continue;

            CreateTurretCard(turret);
        }

        // После создания карточек
        // возвращаемся на первую страницу.
        ResetPaging();
    }

    private void CreateTurretCard(
        TurretData turret
    )
    {
        GameObject cardObject =
            new GameObject(
                "TurretCard_" +
                turret.turretName
            );

        cardObject.transform.SetParent(
            turretGrid,
            false
        );

        Image image =
            cardObject.AddComponent<Image>();

        image.raycastTarget = true;

        Button button =
            cardObject.AddComponent<Button>();

        TurretCard card =
            cardObject.AddComponent<TurretCard>();

        card.Setup(turret);
    }

    // =====================================================
    // SHOW TURRET INFO
    // =====================================================

    public void ShowTurretInfo(
        TurretData turret
    )
    {
        if (turret == null)
            return;

        currentTurret = turret;

        if (infoPanel != null)
        {
            infoPanel.SetActive(true);
        }

        // -----------------------------
        // ICON
        // -----------------------------

        if (infoIcon != null)
        {
            infoIcon.sprite =
                turret.icon;

            infoIcon.enabled =
                turret.icon != null;

            infoIcon.preserveAspect =
                true;
        }

        // -----------------------------
        // NAME
        // -----------------------------

        if (nameText != null)
        {
            nameText.text =
                turret.turretName;
        }

        // -----------------------------
        // RARITY
        // -----------------------------

        if (rarityText != null)
        {
            rarityText.text =
                GetRarityName(
                    turret.rarity
                );
        }

        // -----------------------------
        // STATS
        // -----------------------------

        if (statsText != null)
        {
            statsText.text =
                GetTurretStats(
                    turret
                );
        }
    }

    // =====================================================
    // GET REAL TURRET STATS
    // =====================================================

    private string GetTurretStats(
        TurretData data
    )
    {
        if (data == null)
        {
            return "Нет данных";
        }

        // Проверяем prefab.
        if (data.turretPrefab == null)
        {
            return
                "Prefab турели не назначен!";
        }

        // Берём Turret прямо из prefab.
        Turret turret =
            data.turretPrefab
                .GetComponent<Turret>();

        if (turret == null)
        {
            return
                "На prefab нет компонента Turret!";
        }

        if (
            turret.levels == null ||
            turret.levels.Count == 0)
        {
            return
                "У турели нет уровней!";
        }

        // В инвентаре показываем
        // характеристики первого уровня.
        TurretLevel level =
            turret.levels[0];

        float dps =
            level.damage *
            level.fireRate;

        string fireRateText;

        if (level.fireRate > 0f)
        {
            float reload =
                1f /
                level.fireRate;

            fireRateText =
                reload.ToString("0.0") +
                " сек.";
        }
        else
        {
            fireRateText = "-";
        }

        return
            "ХАРАКТЕРИСТИКИ\n\n" +

            "Урон: " +
            level.damage.ToString("0.0") +

            "\nДальность: " +
            level.range.ToString("0.0") +

            "\nПерезарядка: " +
            fireRateText +

            "\nСкорость стрельбы: " +
            level.fireRate.ToString("0.0") +
            "/сек" +

            "\nDPS: " +
            dps.ToString("0.0") +

            "\nHP: " +
            level.maxHP.ToString("0") +

            "\nОглушение: " +
            level.stunDuration.ToString("0.0") +
            " сек." +

            "\n\nЦена строительства: " +
            data.buildPrice;
    }

    // =====================================================
    // SELECT
    // =====================================================

    public void SelectCurrentTurret()
    {
        if (currentTurret == null)
        {
            Debug.Log(
                "Сначала выбери турель!"
            );

            return;
        }

        if (InventoryManager.Instance == null)
        {
            Debug.LogError(
                "InventoryManager не найден!"
            );

            return;
        }

        bool selected =
            InventoryManager.Instance
                .SelectTurret(
                    currentTurret
                );

        if (!selected)
            return;

        RefreshLoadout();

        RefreshCardVisuals();
    }

    // =====================================================
    // LOADOUT
    // =====================================================

    public void RefreshLoadout()
    {
        if (loadoutSlots == null)
            return;

        for (
            int i = 0;
            i < loadoutSlots.Length;
            i++)
        {
            if (loadoutSlots[i] == null)
                continue;

            TurretData turret = null;

            if (InventoryManager.Instance != null)
            {
                turret =
                    InventoryManager.Instance
                        .GetSelectedTurret(i);
            }

            Image slot =
                loadoutSlots[i];

            if (turret != null)
            {
                slot.sprite =
                    turret.icon;

                slot.enabled =
                    true;

                slot.color =
                    Color.white;

                slot.preserveAspect =
                    true;
            }
            else
            {
                slot.sprite = null;

                slot.enabled =
                    true;

                slot.color =
                    new Color(
                        1f,
                        1f,
                        1f,
                        0.25f
                    );
            }
        }
    }

    // =====================================================
    // CARD VISUALS
    // =====================================================

    public void RefreshCardVisuals()
    {
        if (turretGrid == null)
            return;

        TurretCard[] cards =
            turretGrid
                .GetComponentsInChildren<
                    TurretCard
                >();

        foreach (
            TurretCard card
            in cards)
        {
            if (card != null)
            {
                card.UpdateSelectedVisual();
            }
        }
    }

    // =====================================================
    // RARITY
    // =====================================================

    private string GetRarityName(
        TurretRarity rarity
    )
    {
        switch (rarity)
        {
            case TurretRarity.Common:
                return "COMMON";

            case TurretRarity.Rare:
                return "RARE";

            case TurretRarity.Epic:
                return "EPIC";

            case TurretRarity.Legendary:
                return "LEGENDARY";

            case TurretRarity.Mythic:
                return "MYTHIC";
        }

        return "UNKNOWN";
    }

    // =====================================================
    // CLOSE INFO
    // =====================================================

    public void CloseInfo()
    {
        currentTurret = null;

        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }
    }

    // =====================================================
    // NEXT ROW
    // =====================================================

    public void NextRow()
    {
        if (turretGrid == null)
            return;

        if (gridLayout == null)
        {
            gridLayout =
                turretGrid
                    .GetComponent<
                        GridLayoutGroup
                    >();
        }

        if (turretGridRect == null)
        {
            turretGridRect =
                turretGrid
                    .GetComponent<
                        RectTransform
                    >();
        }

        if (gridLayout == null)
            return;

        int itemCount =
            turretGrid.childCount;

        int totalRows =
            Mathf.CeilToInt(
                (float)itemCount /
                columns
            );

        int maxRow =
            Mathf.Max(
                0,
                totalRows -
                visibleRows
            );

        if (currentRow >= maxRow)
            return;

        currentRow++;

        UpdateGridPosition();
    }

    // =====================================================
    // PREVIOUS ROW
    // =====================================================

    public void PreviousRow()
    {
        if (currentRow <= 0)
            return;

        currentRow--;

        UpdateGridPosition();
    }

    // =====================================================
    // GRID POSITION
    // =====================================================

    private void UpdateGridPosition()
    {
        if (
            turretGridRect == null ||
            gridLayout == null)
        {
            return;
        }

        float rowHeight =
            gridLayout.cellSize.y +
            gridLayout.spacing.y;

        Vector2 position =
            turretGridRect.anchoredPosition;

        position.y =
            -currentRow *
            rowHeight;

        turretGridRect.anchoredPosition =
            position;
    }

    // =====================================================
    // RESET PAGE
    // =====================================================

    public void ResetPaging()
    {
        currentRow = 0;

        if (
            turretGridRect == null)
        {
            if (turretGrid != null)
            {
                turretGridRect =
                    turretGrid
                        .GetComponent<
                            RectTransform
                        >();
            }
        }

        if (gridLayout == null)
        {
            if (turretGrid != null)
            {
                gridLayout =
                    turretGrid
                        .GetComponent<
                            GridLayoutGroup
                        >();
            }
        }

        if (
            turretGridRect == null ||
            gridLayout == null)
        {
            return;
        }

        Vector2 position =
            turretGridRect.anchoredPosition;

        position.y = 0f;

        turretGridRect.anchoredPosition =
            position;
    }
}