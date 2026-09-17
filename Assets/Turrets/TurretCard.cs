using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TurretCard : MonoBehaviour, IPointerClickHandler
{
    private TurretData turretData;

    private Image background;
    private Image icon;
    private TextMeshProUGUI nameText;

    public TurretData Data
    {
        get { return turretData; }
    }

    public void Setup(TurretData data)
    {
        turretData = data;

        if (turretData == null)
        {
            Debug.LogError("TurretCard: TurretData отсутствует!");
            return;
        }

        CreateVisuals();

        if (icon != null)
        {
            icon.sprite = turretData.icon;
            icon.enabled = turretData.icon != null;
            icon.preserveAspect = true;
        }

        if (nameText != null)
        {
            nameText.text = turretData.turretName;
        }

        if (background != null)
        {
            background.color = GetRarityColor(turretData.rarity);
        }

        UpdateSelectedVisual();
    }

    private void CreateVisuals()
    {
        background = GetComponent<Image>();

        if (background == null)
        {
            background = gameObject.AddComponent<Image>();
        }

        Button button = GetComponent<Button>();

        if (button == null)
        {
            button = gameObject.AddComponent<Button>();
        }

        GameObject iconObject = new GameObject("Icon");

        iconObject.transform.SetParent(transform, false);

        icon = iconObject.AddComponent<Image>();

        RectTransform iconRect = icon.GetComponent<RectTransform>();

        iconRect.anchorMin = Vector2.zero;
        iconRect.anchorMax = Vector2.one;
        iconRect.offsetMin = new Vector2(8f, 8f);
        iconRect.offsetMax = new Vector2(-8f, -8f);

        icon.raycastTarget = false;
        icon.preserveAspect = true;

        GameObject nameObject = new GameObject("NameText");

        nameObject.transform.SetParent(transform, false);

        nameText = nameObject.AddComponent<TextMeshProUGUI>();

        RectTransform textRect = nameText.GetComponent<RectTransform>();

        textRect.anchorMin = new Vector2(0f, 0f);
        textRect.anchorMax = new Vector2(1f, 0f);
        textRect.pivot = new Vector2(0.5f, 0f);

        textRect.offsetMin = new Vector2(5f, 5f);
        textRect.offsetMax = new Vector2(-5f, 35f);

        nameText.alignment = TextAlignmentOptions.Center;
        nameText.fontSize = 22f;
        nameText.enableAutoSizing = true;
        nameText.fontSizeMin = 12f;
        nameText.fontSizeMax = 22f;
        nameText.color = Color.white;
        nameText.raycastTarget = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (turretData == null)
            return;

        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.ShowTurretInfo(turretData);
        }
    }

    public void UpdateSelectedVisual()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        bool selected = false;

        if (InventoryManager.Instance != null)
        {
            selected = InventoryManager.Instance.IsTurretSelected(turretData);
        }

        if (selected)
            canvasGroup.alpha = 0.45f;
        else
            canvasGroup.alpha = 1f;
    }

    private Color GetRarityColor(TurretRarity rarity)
    {
        switch (rarity)
        {
            case TurretRarity.Common:
                return new Color(0.55f, 0.55f, 0.55f);

            case TurretRarity.Rare:
                return new Color(0.25f, 0.45f, 0.9f);

            case TurretRarity.Epic:
                return new Color(0.65f, 0.3f, 0.9f);

            case TurretRarity.Legendary:
                return new Color(0.95f, 0.55f, 0.15f);

            case TurretRarity.Mythic:
                return new Color(0.9f, 0.2f, 0.3f);
        }

        return Color.white;
    }
}