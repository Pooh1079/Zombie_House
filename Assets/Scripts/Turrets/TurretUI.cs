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



    void Awake()
    {
        Instance = this;

        panel.SetActive(false);
    }



    public void Open(Turret t)
    {
        turret = t;


        panel.SetActive(true);



        TurretLevel level =
        turret.levels[turret.currentLevel];



        nameText.text =
        turret.name +
        " LV " +
        (turret.currentLevel + 1);



        float dps =
        level.damage *
        level.fireRate;



        statsText.text =
        "Урон: " + level.damage +
        "\nРадиус: " + level.range +
        "\nКулдаун: " + (1f / level.fireRate).ToString("0.0") +
        "\nDPS: " + dps.ToString("0.0");



        if (turret.currentLevel < turret.levels.Count - 1)
        {
            upgradeText.text =
            "Улучшить: " +
            turret.levels[turret.currentLevel + 1].upgradePrice;
        }
        else
        {
            upgradeText.text =
            "MAX";
        }


        sellText.text =
        "Продать: " +
        level.sellPrice;
    }




    public void Upgrade()
    {
        if (turret == null)
            return;


        if (turret.currentLevel >= turret.levels.Count - 1)
            return;



        int price =
        turret.levels[turret.currentLevel + 1].upgradePrice;



        MoneyManager money =
        FindObjectOfType<MoneyManager>();



        if (money.SpendMoney(price))
        {
            turret.Upgrade();

            Open(turret);
        }
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
                turret.levels[turret.currentLevel].sellPrice);
        }

        // Удаляем BuildBlock на месте турели
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                turret.transform.position,
                0.3f);

        foreach (Collider2D hit in hits)
        {
            BuildBlock block =
                hit.GetComponent<BuildBlock>();

            if (block != null)
            {
                Destroy(block.gameObject);
            }
        }

        Destroy(turret.gameObject);

        Close();
    }



    public void Close()
    {
        panel.SetActive(false);

        turret = null;
    }
}