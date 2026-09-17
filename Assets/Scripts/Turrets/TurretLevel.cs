using UnityEngine;

[System.Serializable]
public class TurretLevel
{
    [Header("Stats")]
    public float damage = 5f;

    public float range = 5f;

    public float fireRate = 1f;

    [Header("Health")]
    public float maxHP = 100f;

    public float stunDuration = 2f;

    [Header("Upgrade")]
    public int upgradePrice = 50;

    public int sellPrice = 25;

    [Header("Visual")]
    public Sprite sprite;

    public Vector3 scale = Vector3.one;

    [Header("Projectile")]
    public GameObject projectilePrefab;

    [Header("Sound")]
    public AudioClip shootSound;
}