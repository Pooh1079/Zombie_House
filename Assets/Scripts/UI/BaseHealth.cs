using UnityEngine;
using TMPro;

public class BaseHealth : MonoBehaviour
{
    [Header("Base Health")]
    public float maxHealth = 100f;

    private float currentHealth;



    [Header("UI")]
    public TextMeshProUGUI healthText;



    private bool destroyed = false;



    private void Start()
    {
        currentHealth = maxHealth;

        UpdateText();
    }





    public void TakeDamage(float damage)
    {
        if (destroyed)
            return;


        currentHealth -= damage;


        Debug.Log(
            "Дом получил урон: "
            + damage +
            " HP осталось: "
            + currentHealth);



        UpdateText();



        if (currentHealth <= 0)
        {
            DestroyBase();
        }
    }





    void UpdateText()
    {
        if (healthText != null)
        {
            healthText.text =
            "Дом: "
            + currentHealth
            + "/"
            + maxHealth;
        }
    }





    void DestroyBase()
    {
        if (destroyed)
            return;


        destroyed = true;



        Debug.Log("Дом уничтожен!");



        // вызываем общий проигрыш
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoseGame();
        }



        gameObject.SetActive(false);
    }
}