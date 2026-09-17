using UnityEngine;

public enum TurretRarity
{
    Common,
    Rare,
    Epic,
    Legendary,
    Mythic
}

[CreateAssetMenu(
    fileName = "NewTurret",
    menuName = "Game/Turret Data"
)]
public class TurretData : ScriptableObject
{
    [Header("ID")]
    public string turretID;

    [Header("Basic")]
    public string turretName;

    public Sprite icon;

    public GameObject turretPrefab;

    [Header("Build")]
    public int buildPrice = 100;

    [Header("Rarity")]
    public TurretRarity rarity = TurretRarity.Common;

    [Header("First Launch")]
    public bool unlocked = false;
}