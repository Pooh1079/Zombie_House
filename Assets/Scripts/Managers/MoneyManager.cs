using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public int money = 200;


    [Header("UI")]
    public TextMeshProUGUI moneyText;



    [Header("Popup")]
    public GameObject moneyPopupPrefab;
    public Transform popupPoint;



    void Start()
    {
        UpdateMoney();
    }




    public bool SpendMoney(int amount)
    {
        Debug.Log("СПИСАНИЕ ДЕНЕГ: " + amount);
        if (money < amount)
        {
            return false;
        }


        money -= amount;


        UpdateMoney();


        CreatePopup(
            "-" + amount + "₽",
            Color.red
        );


        return true;
    }





    public void AddMoney(int amount)
    {
        Debug.Log("ДОБАВЛЕНИЕ ДЕНЕГ: " + amount);
        money += amount;


        UpdateMoney();


        CreatePopup(
            "+" + amount + "₽",
            Color.yellow
        );
    }





    void UpdateMoney()
    {
        if (moneyText != null)
        {
            moneyText.text =
                "$ " + money;
        }
    }





    void CreatePopup(string textValue, Color color)
    {
        if (moneyPopupPrefab == null)
            return;


        if (popupPoint == null)
            return;



        GameObject obj =
            Instantiate(
                moneyPopupPrefab,
                popupPoint.position,
                Quaternion.identity
            );



        MoneyPopup popup =
            obj.GetComponent<MoneyPopup>();


        if (popup != null)
        {
            popup.Show(
                textValue,
                color
            );
        }
    }
}