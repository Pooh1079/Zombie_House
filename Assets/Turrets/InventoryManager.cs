using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Полученные турели")]
    public List<TurretData> unlockedTurrets =
        new List<TurretData>();

    [Header("Выбранные турели")]
    public List<TurretData> selectedTurrets =
        new List<TurretData>();

    [Header("Settings")]
    public int maxSelectedTurrets = 6;

    public bool DataLoaded { get; private set; }

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

    public void InitializeNewGame()
    {
        unlockedTurrets.Clear();
        selectedTurrets.Clear();

        if (TurretDatabase.Instance != null)
        {
            List<TurretData> all =
                TurretDatabase.Instance
                    .GetAllTurrets();

            foreach (
                TurretData turret
                in all)
            {
                if (turret == null)
                    continue;

                if (turret.unlocked)
                {
                    unlockedTurrets.Add(
                        turret
                    );
                }
            }
        }

        DataLoaded = true;

        Debug.Log(
            "НОВАЯ ИГРА. Турелей: " +
            unlockedTurrets.Count
        );
    }

    public void LoadFromIDs(
        List<string> unlockedIDs,
        List<string> selectedIDs
    )
    {
        unlockedTurrets.Clear();
        selectedTurrets.Clear();

        if (TurretDatabase.Instance == null)
        {
            Debug.LogError(
                "InventoryManager: TurretDatabase не найден!"
            );

            return;
        }

        if (unlockedIDs != null)
        {
            foreach (
                string id
                in unlockedIDs)
            {
                TurretData turret =
                    TurretDatabase.Instance
                        .FindByID(id);

                if (turret == null)
                    continue;

                if (
                    !unlockedTurrets
                        .Contains(turret))
                {
                    unlockedTurrets.Add(
                        turret
                    );
                }
            }
        }

        if (selectedIDs != null)
        {
            foreach (
                string id
                in selectedIDs)
            {
                if (string.IsNullOrEmpty(id))
                    continue;

                TurretData turret =
                    TurretDatabase.Instance
                        .FindByID(id);

                if (turret == null)
                    continue;

                if (
                    !unlockedTurrets
                        .Contains(turret))
                {
                    continue;
                }

                if (
                    selectedTurrets.Count >=
                    maxSelectedTurrets)
                {
                    break;
                }

                if (
                    !selectedTurrets
                        .Contains(turret))
                {
                    selectedTurrets.Add(
                        turret
                    );
                }
            }
        }

        DataLoaded = true;

        Debug.Log(
            "ИНВЕНТАРЬ ЗАГРУЖЕН. Получено: " +
            unlockedTurrets.Count +
            " | Выбрано: " +
            selectedTurrets.Count
        );
    }

    public bool UnlockTurret(
        TurretData turret
    )
    {
        if (turret == null)
            return false;

        if (
            unlockedTurrets
                .Contains(turret))
        {
            return false;
        }

        unlockedTurrets.Add(
            turret
        );

        Save();

        return true;
    }

    public bool SelectTurret(
        TurretData turret
    )
    {
        if (turret == null)
            return false;

        if (
            !unlockedTurrets
                .Contains(turret))
        {
            return false;
        }

        if (
            selectedTurrets
                .Contains(turret))
        {
            return false;
        }

        if (
            selectedTurrets.Count >=
            maxSelectedTurrets)
        {
            Debug.Log(
                "Все 6 слотов заняты!"
            );

            return false;
        }

        selectedTurrets.Add(
            turret
        );

        Save();

        return true;
    }

    public void RemoveTurret(
        TurretData turret
    )
    {
        if (turret == null)
            return;

        if (
            selectedTurrets
                .Contains(turret))
        {
            selectedTurrets.Remove(
                turret
            );

            Save();
        }
    }

    public TurretData GetSelectedTurret(
        int index
    )
    {
        if (index < 0)
            return null;

        if (
            index >=
            selectedTurrets.Count)
        {
            return null;
        }

        return selectedTurrets[index];
    }

    public bool IsTurretSelected(
        TurretData turret
    )
    {
        if (turret == null)
            return false;

        return selectedTurrets
            .Contains(turret);
    }

    public bool IsTurretUnlocked(
        TurretData turret
    )
    {
        if (turret == null)
            return false;

        return unlockedTurrets
            .Contains(turret);
    }

    public int GetSelectedCount()
    {
        return selectedTurrets.Count;
    }

    public int GetUnlockedCount()
    {
        return unlockedTurrets.Count;
    }

    public void Save()
    {
        if (GameSaveSystem.Instance != null)
        {
            GameSaveSystem.Instance
                .SaveAll();
        }
    }
}