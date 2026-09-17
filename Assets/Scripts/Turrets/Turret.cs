using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Turret Data")]
    public TurretData turretData;

    [Header("Range Circle")]
    public GameObject rangeCirclePrefab;

    private GameObject currentRangeCircle;

    [Header("Levels")]
    public List<TurretLevel> levels =
        new List<TurretLevel>();

    [Header("References")]
    public SpriteRenderer spriteRenderer;

    public Transform firePoint;

    public GameObject rangeCircle;

    [Header("Layers")]
    public LayerMask zombieLayer;

    [Header("Settings")]
    public int currentLevel = 0;

    [Header("Health")]
    public float currentHP = 100f;

    [Header("Stun")]
    public bool isStunned = false;

    private float stunTimer = 0f;

    private float shootTimer;

    private Transform currentTarget;

    private void Start()
    {
        if (levels.Count == 0)
        {
            Debug.LogError(
                "Turret: У турели нет LEVELS!"
            );

            return;
        }

        LoadSavedData();

        ApplyLevel();

        CreateRangeCircle();

        UpdateRangeCircle();
    }

    private void Update()
    {
        if (levels.Count == 0)
            return;

        if (
            currentLevel < 0 ||
            currentLevel >= levels.Count)
        {
            currentLevel = 0;
        }

        UpdateStun();

        if (isStunned)
            return;

        FindTarget();

        RotateToTarget();

        Shoot();
    }

    // =====================================================
    // RANGE CIRCLE
    // =====================================================

    private void CreateRangeCircle()
    {
        if (rangeCirclePrefab == null)
            return;

        currentRangeCircle =
            Instantiate(
                rangeCirclePrefab,
                transform.position,
                Quaternion.identity
            );

        currentRangeCircle.transform.SetParent(
            transform
        );

        currentRangeCircle.transform.localPosition =
            Vector3.zero;

        currentRangeCircle.SetActive(false);

        if (rangeCircle == null)
        {
            rangeCircle =
                currentRangeCircle;
        }
    }

    // =====================================================
    // LOAD SAVED DATA
    // =====================================================

    private void LoadSavedData()
    {
        currentLevel = 0;

        if (turretData == null)
        {
            Debug.LogWarning(
                "Turret: TurretData не назначен!"
            );

            SetHPFromLevel();

            return;
        }

        if (GameSaveSystem.Instance == null)
        {
            SetHPFromLevel();

            return;
        }

        currentLevel =
            GameSaveSystem.Instance
                .GetSavedTurretLevel(
                    turretData.turretID
                );

        currentLevel =
            Mathf.Clamp(
                currentLevel,
                0,
                levels.Count - 1
            );

        float savedHP =
            GameSaveSystem.Instance
                .GetSavedTurretHP(
                    turretData.turretID
                );

        if (savedHP < 0f)
        {
            SetHPFromLevel();
        }
        else
        {
            currentHP =
                Mathf.Clamp(
                    savedHP,
                    0f,
                    levels[currentLevel].maxHP
                );
        }

        Debug.Log(
            "ЗАГРУЖЕНА ТУРЕЛЬ: " +
            turretData.turretName +
            " | LV " +
            (currentLevel + 1) +
            " | HP " +
            currentHP
        );
    }

    private void SetHPFromLevel()
    {
        currentHP =
            levels[currentLevel].maxHP;
    }

    // =====================================================
    // APPLY LEVEL
    // =====================================================

    private void ApplyLevel()
    {
        if (levels.Count == 0)
            return;

        TurretLevel level =
            levels[currentLevel];

        if (spriteRenderer != null &&
            level.sprite != null)
        {
            spriteRenderer.sprite =
                level.sprite;
        }

        transform.localScale =
            level.scale;

        if (currentHP <= 0f)
        {
            currentHP =
                level.maxHP;
        }

        currentHP =
            Mathf.Clamp(
                currentHP,
                0f,
                level.maxHP
            );

        UpdateRangeCircle();
    }

    // =====================================================
    // HEALTH
    // =====================================================

    public void TakeDamage(float damage)
    {
        if (damage <= 0f)
            return;

        if (isStunned)
            return;

        currentHP -= damage;

        currentHP =
            Mathf.Clamp(
                currentHP,
                0f,
                levels[currentLevel].maxHP
            );

        Debug.Log(
            "ТУРЕЛЬ получила урон: " +
            damage +
            " | HP: " +
            currentHP +
            "/" +
            levels[currentLevel].maxHP
        );

        SaveHealth();

        if (currentHP <= 0f)
        {
            StartStun();
        }
    }

    public float GetCurrentHP()
    {
        return currentHP;
    }

    public float GetMaxHP()
    {
        if (levels.Count == 0)
            return 0f;

        return levels[currentLevel].maxHP;
    }

    // =====================================================
    // STUN
    // =====================================================

    private void StartStun()
    {
        isStunned = true;

        stunTimer =
            levels[currentLevel].stunDuration;

        Debug.Log(
            "ТУРЕЛЬ ОГЛУШЕНА на " +
            stunTimer +
            " сек."
        );
    }

    private void UpdateStun()
    {
        if (!isStunned)
            return;

        stunTimer -=
            Time.deltaTime;

        if (stunTimer <= 0f)
        {
            stunTimer = 0f;

            isStunned = false;

            currentHP =
                levels[currentLevel].maxHP;

            SaveHealth();

            Debug.Log(
                "ТУРЕЛЬ восстановилась после оглушения."
            );
        }
    }

    public float GetStunTime()
    {
        return stunTimer;
    }

    // =====================================================
    // SAVE HEALTH
    // =====================================================

    private void SaveHealth()
    {
        if (turretData == null)
            return;

        if (GameSaveSystem.Instance == null)
            return;

        GameSaveSystem.Instance
            .SaveTurretState(
                turretData.turretID,
                currentLevel,
                currentHP
            );
    }

    // =====================================================
    // TARGET
    // =====================================================

    private void FindTarget()
    {
        currentTarget = null;

        float range =
            levels[currentLevel].range;

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                transform.position,
                range,
                zombieLayer
            );

        float closest =
            Mathf.Infinity;

        foreach (
            Collider2D hit
            in hits)
        {
            Zombie zombie =
                hit.GetComponent<Zombie>();

            if (zombie == null)
                continue;

            float distance =
                Vector2.Distance(
                    transform.position,
                    hit.transform.position
                );

            if (distance < closest)
            {
                closest = distance;

                currentTarget =
                    hit.transform;
            }
        }
    }

    // =====================================================
    // ROTATE
    // =====================================================

    private void RotateToTarget()
    {
        if (currentTarget == null)
            return;

        Vector3 dir =
            currentTarget.position -
            transform.position;

        float angle =
            Mathf.Atan2(
                dir.y,
                dir.x
            ) * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                angle
            );
    }

    // =====================================================
    // SHOOT
    // =====================================================

    private void Shoot()
    {
        if (currentTarget == null)
            return;

        shootTimer +=
            Time.deltaTime;

        float fireRate =
            levels[currentLevel].fireRate;

        if (fireRate <= 0f)
            return;

        float delay =
            1f / fireRate;

        if (shootTimer < delay)
            return;

        shootTimer = 0f;

        if (firePoint == null)
        {
            Debug.LogError(
                "Turret: FirePoint не назначен!"
            );

            return;
        }

        if (
            levels[currentLevel]
                .projectilePrefab == null)
        {
            Debug.LogError(
                "Turret: ProjectilePrefab не назначен!"
            );

            return;
        }

        GameObject arrow =
            Instantiate(
                levels[currentLevel]
                    .projectilePrefab,
                firePoint.position,
                firePoint.rotation
            );

        Projectile projectile =
            arrow.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.SetTarget(
                currentTarget,
                levels[currentLevel].damage
            );
        }

        if (
            levels[currentLevel]
                .shootSound != null)
        {
            AudioSource.PlayClipAtPoint(
                levels[currentLevel]
                    .shootSound,
                transform.position
            );
        }
    }

    // =====================================================
    // RANGE
    // =====================================================

    private void UpdateRangeCircle()
    {
        if (rangeCircle == null)
            return;

        if (levels.Count == 0)
            return;

        SpriteRenderer sr =
            rangeCircle.GetComponent<SpriteRenderer>();

        if (sr == null)
            return;

        if (sr.sprite == null)
            return;

        float spriteDiameter =
            sr.sprite.bounds.size.x;

        if (spriteDiameter <= 0f)
            return;

        float wantedDiameter =
            levels[currentLevel].range * 2f;

        float scale =
            wantedDiameter /
            spriteDiameter;

        rangeCircle.transform.localScale =
            new Vector3(
                scale,
                scale,
                1f
            );
    }

    // =====================================================
    // UPGRADE
    // =====================================================

    public void Upgrade()
    {
        if (levels.Count == 0)
            return;

        if (
            currentLevel >=
            levels.Count - 1)
        {
            Debug.Log(
                "ТУРЕЛЬ УЖЕ МАКСИМАЛЬНОГО УРОВНЯ"
            );

            return;
        }

        currentLevel++;

        ApplyLevel();

        currentHP =
            levels[currentLevel].maxHP;

        SaveHealth();

        Debug.Log(
            "ТУРЕЛЬ ПРОКАЧАНА → LV " +
            (currentLevel + 1)
        );
    }

    // =====================================================
    // RANGE UI
    // =====================================================

    public void ShowRange()
    {
        if (rangeCircle != null)
        {
            rangeCircle.SetActive(true);
        }
    }

    public void HideRange()
    {
        if (rangeCircle != null)
        {
            rangeCircle.SetActive(false);
        }
    }

    // =====================================================
    // CLICK
    // =====================================================

    private void OnMouseDown()
    {
        Debug.Log(
            "КЛИК ПО ТУРЕЛИ"
        );

        ShowRange();

        if (TurretUI.Instance != null)
        {
            TurretUI.Instance.Open(this);
        }
    }

    // =====================================================
    // GIZMOS
    // =====================================================

    private void OnDrawGizmosSelected()
    {
        if (
            levels == null ||
            levels.Count == 0)
        {
            return;
        }

        if (
            currentLevel < 0 ||
            currentLevel >= levels.Count)
        {
            return;
        }

        Gizmos.color =
            Color.green;

        Gizmos.DrawWireSphere(
            transform.position,
            levels[currentLevel].range
        );
    }
}