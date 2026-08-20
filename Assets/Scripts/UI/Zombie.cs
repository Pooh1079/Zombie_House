using UnityEngine;

public class Zombie : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 20f;
    private float currentHealth;


    [Header("Movement")]
    public float speed = 2f;


    [Header("Attack")]
    public float damage = 5f;
    public float attackDistance = 0.8f;
    public float attackCooldown = 1f;


    [Header("Body Parts")]
    public Transform leftArm;
    public Transform rightArm;
    public Transform leftLeg;
    public Transform rightLeg;


    [Header("Walk")]
    public float legAngle = 25f;
    public float walkSpeed = 8f;


    [Header("Attack Animation")]
    public float raisedAngle = 90f;
    public float raisedSpeed = 400f;
    public float hitSpeed = 600f;


    private BaseHealth house;

    private Vector3 startScale;

    private bool attacking;
    private bool handsUp;
    private bool dead = false;

    private float cooldown;



    void Start()
    {
        currentHealth = maxHealth;

        house = FindObjectOfType<BaseHealth>();

        startScale = transform.localScale;
    }



    void Update()
    {
        if (house == null)
            return;


        float distance =
        Vector2.Distance(
        transform.position,
        house.transform.position);



        if (distance > attackDistance)
        {
            Move();
        }
        else
        {
            Attack();
        }
    }



    void Move()
    {
        Vector3 direction =
        house.transform.position -
        transform.position;



        transform.position =
        Vector3.MoveTowards(
        transform.position,
        house.transform.position,
        speed * Time.deltaTime);



        if (direction.x > 0)
        {
            transform.localScale =
            new Vector3(
            Mathf.Abs(startScale.x),
            startScale.y,
            startScale.z);
        }
        else
        {
            transform.localScale =
            new Vector3(
            -Mathf.Abs(startScale.x),
            startScale.y,
            startScale.z);
        }



        float step =
        Mathf.Sin(
        Time.time * walkSpeed)
        * legAngle;



        if (leftLeg)
        {
            leftLeg.localRotation =
            Quaternion.Euler(
            0, 0, step);
        }


        if (rightLeg)
        {
            rightLeg.localRotation =
            Quaternion.Euler(
            0, 0, -step);
        }
    }



    void Attack()
    {
        if (leftLeg)
            leftLeg.localRotation =
            Quaternion.identity;


        if (rightLeg)
            rightLeg.localRotation =
            Quaternion.identity;



        cooldown += Time.deltaTime;



        if (!handsUp)
        {
            float angle =
            Mathf.MoveTowardsAngle(
            leftArm.localEulerAngles.z,
            raisedAngle,
            raisedSpeed *
            Time.deltaTime);



            if (leftArm)
                leftArm.localRotation =
                Quaternion.Euler(
                0, 0, angle);


            if (rightArm)
                rightArm.localRotation =
                Quaternion.Euler(
                0, 0, angle);



            if (Mathf.Abs(
            Mathf.DeltaAngle(
            angle,
            raisedAngle)) < 1)
            {
                handsUp = true;
            }


            return;
        }



        float down =
        Mathf.MoveTowardsAngle(
        leftArm.localEulerAngles.z,
        0,
        hitSpeed *
        Time.deltaTime);



        if (leftArm)
            leftArm.localRotation =
            Quaternion.Euler(
            0, 0, down);


        if (rightArm)
            rightArm.localRotation =
            Quaternion.Euler(
            0, 0, down);



        if (Mathf.Abs(
        Mathf.DeltaAngle(
        down, 0)) < 1)
        {

            if (!attacking)
            {
                house.TakeDamage(damage);
                attacking = true;
            }



            if (cooldown >= attackCooldown)
            {
                attacking = false;
                handsUp = false;
                cooldown = 0;
            }
        }
    }



    public void TakeDamage(float damageAmount)
    {
        if (dead)
            return;

        currentHealth -= damageAmount;

        Debug.Log("Zombie HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            dead = true;

            Collider2D col = GetComponent<Collider2D>();

            if (col != null)
                col.enabled = false;

            enabled = false;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.ZombieDead();
                GameManager.Instance.ZombieKilled();
            }

            ZombieDeath death = GetComponent<ZombieDeath>();

            if (death != null)
            {
                death.Die();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}