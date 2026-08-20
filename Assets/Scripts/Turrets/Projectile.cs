using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Speed")]
    public float speed = 10f;


    private Transform target;

    private float damage;



    public void SetTarget(Transform enemy, float damageAmount)
    {
        target = enemy;
        damage = damageAmount;
    }



    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }



        Vector3 direction =
            target.position -
            transform.position;



        transform.position =
            Vector3.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );



        // поворот стрелы
        float angle =
            Mathf.Atan2(
                direction.y,
                direction.x
            ) * Mathf.Rad2Deg;


        transform.rotation =
            Quaternion.Euler(
                0,
                0,
                angle
            );



        if (Vector2.Distance(
            transform.position,
            target.position) < 0.1f)
        {
            Hit();
        }
    }




    void Hit()
    {
        Zombie zombie =
            target.GetComponent<Zombie>();


        if (zombie != null)
        {
            zombie.TakeDamage(damage);
        }


        Destroy(gameObject);
    }
}