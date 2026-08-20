using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject rangeCirclePrefab;

    private GameObject currentRangeCircle;

    [Header("Levels")]
    public List<TurretLevel> levels = new List<TurretLevel>();

    [Header("References")]
    public SpriteRenderer spriteRenderer;
    public Transform firePoint;
    public GameObject rangeCircle;

    [Header("Layers")]
    public LayerMask zombieLayer;

    [Header("Settings")]
    public int currentLevel = 0;

    private float shootTimer;
    private Transform currentTarget;

   
    private void Start()
    {
        ApplyLevel();

        currentRangeCircle =
            Instantiate(
                rangeCirclePrefab,
                transform.position,
                Quaternion.identity
            );

        currentRangeCircle.transform.SetParent(transform);

        currentRangeCircle.transform.localPosition =
            Vector3.zero;

        currentRangeCircle.SetActive(false);

        UpdateRangeCircle();
    }

    private void Update()
    {
        if (levels.Count == 0)
            return;

        FindTarget();
        RotateToTarget();
        Shoot();
    }

    void FindTarget()
    {
        currentTarget = null;

        float range = levels[currentLevel].range;

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                transform.position,
                range,
                zombieLayer);

        float closest = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            Zombie zombie = hit.GetComponent<Zombie>();

            if (zombie == null)
                continue;

            float distance =
                Vector2.Distance(
                    transform.position,
                    hit.transform.position);

            if (distance < closest)
            {
                closest = distance;
                currentTarget = hit.transform;
            }
        }
    }

    void RotateToTarget()
    {
        if (currentTarget == null)
            return;

        Vector3 dir =
            currentTarget.position -
            transform.position;

        float angle =
            Mathf.Atan2(
                dir.y,
                dir.x) *
            Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(
                0,
                0,
                angle);
    }

    void Shoot()
    {
        if (currentTarget == null)
            return;

        shootTimer += Time.deltaTime;

        float delay =
            1f /
            levels[currentLevel].fireRate;

        if (shootTimer < delay)
            return;

        shootTimer = 0;

        if (firePoint == null)
        {
            Debug.LogError("Нет FirePoint!");
            return;
        }

        if (levels[currentLevel].projectilePrefab == null)
        {
            Debug.LogError("Нет ProjectilePrefab!");
            return;
        }

        GameObject arrow =
            Instantiate(
                levels[currentLevel].projectilePrefab,
                firePoint.position,
                firePoint.rotation);

        Projectile projectile =
            arrow.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.SetTarget(
                currentTarget,
                levels[currentLevel].damage);
        }

        if (levels[currentLevel].shootSound != null)
        {
            AudioSource.PlayClipAtPoint(
                levels[currentLevel].shootSound,
                transform.position);
        }
    }
    void ApplyLevel()
    {
        if (levels.Count == 0)
            return;

        TurretLevel level = levels[currentLevel];

        if (spriteRenderer != null &&
            level.sprite != null)
        {
            spriteRenderer.sprite = level.sprite;
        }

        UpdateRangeCircle();
    }

    void UpdateRangeCircle()
    {
        if (rangeCircle == null)
            return;

        SpriteRenderer sr = rangeCircle.GetComponent<SpriteRenderer>();

        if (sr == null)
            return;

        if (sr.sprite == null)
            return;

        float spriteDiameter = sr.sprite.bounds.size.x;

        float wantedDiameter = levels[currentLevel].range * 2f;

        float scale = wantedDiameter / spriteDiameter;

        rangeCircle.transform.localScale =
            new Vector3(scale, scale, 1f);

        Debug.Log("Диаметр = " + (levels[currentLevel].range * 2f));

    }

    public void Upgrade()
    {
        if (currentLevel >= levels.Count - 1)
        {
            Debug.Log("Максимальный уровень");
            return;
        }

        currentLevel++;

        ApplyLevel();

        Debug.Log("Турель улучшена до LV " + (currentLevel + 1));
    }

    public void ShowRange()
    {
        if (rangeCircle != null)
            rangeCircle.SetActive(true);
    }

    public void HideRange()
    {
        if (rangeCircle != null)
            rangeCircle.SetActive(false);
    }

    private void OnMouseDown()
    {
        Debug.Log("Нажали на турель");

        ShowRange();

        if (TurretUI.Instance != null)
        {
            TurretUI.Instance.Open(this);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (levels == null || levels.Count == 0)
            return;

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(
            transform.position,
            levels[currentLevel].range);
    }
}