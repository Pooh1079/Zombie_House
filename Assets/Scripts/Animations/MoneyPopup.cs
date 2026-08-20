using UnityEngine;
using TMPro;

public class MoneyPopup : MonoBehaviour
{
    public TMP_Text text;

    public float moveSpeed = 2f;
    public float lifeTime = 1.2f;


    public void Show(string message, Color color)
    {
        if (text != null)
        {
            text.text = message;
            text.color = color;
        }

        Destroy(gameObject, lifeTime);
    }



    // Для старого ZombieDeath
    public void ShowMoney(int amount, Color color)
    {
        Show(
            "+" + amount + "₽",
            color
        );
    }



    // Для старого кода с Show(amount)
    public void Show(int amount)
    {
        Show(
            "+" + amount + "₽",
            Color.yellow
        );
    }



    void Update()
    {
        transform.position +=
            Vector3.up *
            moveSpeed *
            Time.deltaTime;
    }
}