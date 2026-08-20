using UnityEngine;
using TMPro;

public class TurretUpgradePanel : MonoBehaviour
{
    public static TurretUpgradePanel Instance;


    [Header("UI")]
    public GameObject panel;
    public TMP_Text turretNameText;
    public TMP_Text upgradeText;
    public TMP_Text sellText;


    private Turret currentTurret;



    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }



    public void Open(Turret turret)
    {
        currentTurret = turret;


        panel.SetActive(true);


        TurretLevel level =
            turret.levels[turret.currentLevel];


        turretNameText.text =
            turret.name +
            " LV " +
            (turret.currentLevel + 1);



        if (turret.currentLevel < turret.levels.Count - 1)
        {
            upgradeText.text =
                "Прокачать: " +
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
        if (currentTurret == null)
            return;


        if (currentTurret.currentLevel >= currentTurret.levels.Count - 1)
            return;



        int price =
        currentTurret.levels[currentTurret.currentLevel + 1].upgradePrice;



        MoneyManager money =
        FindObjectOfType<MoneyManager>();


        if (money != null)
        {
            if (money.SpendMoney(price))
            {
                currentTurret.Upgrade();
                Open(currentTurret);
            }
        }
    }




    public void Sell()
    {
        if (currentTurret == null)
            return;


        MoneyManager money =
        FindObjectOfType<MoneyManager>();


        if (money != null)
        {
            money.AddMoney(
                currentTurret.levels[currentTurret.currentLevel].sellPrice
            );
        }


        Destroy(currentTurret.gameObject);

        Close();
    }




    public void Close()
    {
        panel.SetActive(false);

        currentTurret = null;
    }
}