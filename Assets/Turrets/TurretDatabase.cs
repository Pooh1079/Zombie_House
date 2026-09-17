using System.Collections.Generic;
using UnityEngine;

public class TurretDatabase : MonoBehaviour
{
    public static TurretDatabase Instance;

    [Header("Все турели игры")]
    public List<TurretData> allTurrets =
        new List<TurretData>();

    private void Awake()
    {
        if (
            Instance != null &&
            Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public List<TurretData> GetAllTurrets()
    {
        return allTurrets;
    }

    public List<TurretData> GetUnlockedTurrets()
    {
        List<TurretData> result =
            new List<TurretData>();

        if (
            InventoryManager.Instance != null &&
            InventoryManager.Instance.DataLoaded)
        {
            foreach (
                TurretData turret
                in InventoryManager.Instance
                    .unlockedTurrets)
            {
                if (turret != null)
                {
                    result.Add(turret);
                }
            }
        }
        else
        {
            foreach (
                TurretData turret
                in allTurrets)
            {
                if (turret == null)
                    continue;

                if (turret.unlocked)
                {
                    result.Add(turret);
                }
            }
        }

        SortByRarity(result);

        return result;
    }

    public TurretData FindByID(
        string id
    )
    {
        if (string.IsNullOrEmpty(id))
            return null;

        foreach (
            TurretData turret
            in allTurrets)
        {
            if (turret == null)
                continue;

            if (
                turret.turretID ==
                id)
            {
                return turret;
            }
        }

        return null;
    }

    private void SortByRarity(
        List<TurretData> turrets
    )
    {
        turrets.Sort(
            delegate (
                TurretData a,
                TurretData b
            )
            {
                return
                    ((int)a.rarity)
                    .CompareTo(
                        (int)b.rarity
                    );
            }
        );
    }
}