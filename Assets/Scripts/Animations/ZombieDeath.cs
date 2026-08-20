using UnityEngine;
using System.Collections;

public class ZombieDeath : MonoBehaviour
{
    [Header("Death")]
    public float fallAngle = 90f;
    public float fallSpeed = 300f;
    public float destroyDelay = 2f;


    [Header("Money")]
    public int moneyReward = 25;


    private bool dead;



    public void Die()
    {
        if (dead)
            return;

        dead = true;

        StartCoroutine(Death());
    }



    IEnumerator Death()
    {
        float angle = 0;


        while (angle < fallAngle)
        {
            angle += fallSpeed * Time.deltaTime;


            transform.rotation =
                Quaternion.Euler(
                    0,
                    0,
                    angle
                );


            yield return null;
        }



        yield return new WaitForSeconds(0.5f);



        // ÄÅÍÜÃÈ ÇÀ ÇÎÌÁÈ
        MoneyManager money =
            FindObjectOfType<MoneyManager>();


        if (money != null)
        {
            money.AddMoney(moneyReward);
        }



        yield return new WaitForSeconds(destroyDelay);


        Destroy(gameObject);
    }
}