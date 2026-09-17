using UnityEngine;
using TMPro;

public class TurretUI : MonoBehaviour
{
    public static TurretUI Instance;

    [Header("Panel")]
    public GameObject panel;

    [Header("Texts")]
    public TMP_Text nameText;
    public TMP_Text statsText;
    public TMP_Text upgradeText;
    public TMP_Text sellText;

    private Turret turret;

    private void Awake()
    {
        Instance = this;

        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    public void Open(Turret t)
    {
        if (t == null)
            return;

        if (
            t.levels == null ||
            t.levels.Count == 0)
        {
            return;
        }

        turret = t;

        if (panel != null)
        {
            panel.SetActive(true);
        }

        TurretLevel level =
            turret.levels[
                turret.currentLevel
            ];

        string turretName =
            turret.turretData != null
                ? turret.turretData.turretName
                : turret.name;

        if (nameText != null)
        {
            nameText.text =
                turretName +
                " LV " +
                (turret.currentLevel + 1);
        }

        float dps =
            level.damage *
            level.fireRate;

        if (statsText != null)
        {
            statsText.text =
                "Урон: " +
                level.damage +

                "\nДальность: " +
                level.range +

                "\nПерезарядка: " +
                (
                    level.fireRate > 0f
                    ? (1f / level.fireRate)
                        .ToString("0.0")
                    : "-"
                ) +

                "\nDPS: " +
                dps.ToString("0.0") +

                "\nHP: " +
                turret.currentHP.ToString("0") +
                "/" +
                level.maxHP.ToString("0") +

                "\nОглушение: " +
                level.stunDuration.ToString("0.0") +
                " сек.";
        }

        if (
            turret.currentLevel <
            turret.levels.Count - 1)
        {
            int price =
                turret.levels[
                    turret.currentLevel + 1
                ].upgradePrice;

            if (upgradeText != null)
            {
                upgradeText.text =
                    "Прокачать: " +
                    price;
            }
        }
        else
        {
            if (upgradeText != null)
            {
                upgradeText.text =
                    "MAX";
            }
        }

        if (sellText != null)
        {
            sellText.text =
                "Продать: " +
                level.sellPrice;
        }
    }

    public void Upgrade()
    {
        if (turret == null)
            return;

        if (
            turret.currentLevel >=
            turret.levels.Count - 1)
        {
            return;
        }

        int price =
            turret.levels[
                turret.currentLevel + 1
            ].upgradePrice;

        // Это именно боевые деньги.
        MoneyManager money =
            FindObjectOfType<MoneyManager>();

        if (money == null)
        {
            Debug.LogError(
                "TurretUI: MoneyManager не найден!"
            );

            return;
        }

        if (
            !money.SpendMoney(price))
        {
            return;
        }

        turret.Upgrade();

        Open(turret);
    }

    public void Sell()
    {
        if (turret == null)
            return;

        MoneyManager money =
            FindObjectOfType<MoneyManager>();

        if (money != null)
        {
            money.AddMoney(
                turret.levels[
                    turret.currentLevel
                ].sellPrice
            );
        }

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                turret.transform.position,
                0.3f
            );

        foreach (
            Collider2D hit
            in hits)
        {
            BuildBlock block =
                hit.GetComponent<BuildBlock>();

            if (block != null)
            {
                Destroy(
                    block.gameObject
                );
            }
        }

        Destroy(
            turret.gameObject
        );

        Close();
    }

    public void Close()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }

        turret = null;
    }
}