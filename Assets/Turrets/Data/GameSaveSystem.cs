using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveSystem : MonoBehaviour
{
    public static GameSaveSystem Instance;

    private const string SAVE_KEY =
        "PLAYER_GAME_SAVE_V2";

    [Serializable]
    public class TurretSave
    {
        public string turretID;

        public int level;

        public float hp;
    }

    [Serializable]
    public class SaveData
    {
        public List<string> unlockedTurrets =
            new List<string>();

        public List<string> selectedTurrets =
            new List<string>();

        public List<TurretSave> turretStates =
            new List<TurretSave>();
    }

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

    private void Start()
    {
        StartCoroutine(
            LoadNextFrame()
        );
    }

    private IEnumerator LoadNextFrame()
    {
        yield return null;

        LoadAll();
    }

    // =====================================================
    // HAS SAVE
    // =====================================================

    public bool HasSave()
    {
        return PlayerPrefs.HasKey(
            SAVE_KEY
        );
    }

    // =====================================================
    // SAVE EVERYTHING
    // =====================================================

    public void SaveAll()
    {
        SaveData data =
            new SaveData();

        if (InventoryManager.Instance != null)
        {
            foreach (
                TurretData turret
                in InventoryManager.Instance.unlockedTurrets)
            {
                if (turret == null)
                    continue;

                if (
                    string.IsNullOrEmpty(
                        turret.turretID))
                {
                    continue;
                }

                if (
                    !data.unlockedTurrets
                        .Contains(
                            turret.turretID))
                {
                    data.unlockedTurrets.Add(
                        turret.turretID
                    );
                }
            }

            foreach (
                TurretData turret
                in InventoryManager.Instance.selectedTurrets)
            {
                if (turret == null)
                    continue;

                if (
                    string.IsNullOrEmpty(
                        turret.turretID))
                {
                    continue;
                }

                data.selectedTurrets.Add(
                    turret.turretID
                );
            }
        }

        Turret[] turrets =
            FindObjectsOfType<Turret>();

        foreach (
            Turret turret
            in turrets)
        {
            if (turret == null)
                continue;

            if (turret.turretData == null)
                continue;

            if (
                string.IsNullOrEmpty(
                    turret.turretData.turretID))
            {
                continue;
            }

            TurretSave saved =
                new TurretSave();

            saved.turretID =
                turret.turretData.turretID;

            saved.level =
                turret.currentLevel;

            saved.hp =
                turret.currentHP;

            data.turretStates.Add(
                saved
            );
        }

        string json =
            JsonUtility.ToJson(
                data
            );

        PlayerPrefs.SetString(
            SAVE_KEY,
            json
        );

        PlayerPrefs.Save();

        Debug.Log(
            "ИГРА СОХРАНЕНА!"
        );
    }

    // =====================================================
    // LOAD EVERYTHING
    // =====================================================

    public void LoadAll()
    {
        if (!HasSave())
        {
            Debug.Log(
                "ПЕРВЫЙ ЗАПУСК — создаём новую игру."
            );

            if (
                InventoryManager.Instance !=
                null)
            {
                InventoryManager.Instance
                    .InitializeNewGame();
            }

            SaveAll();

            return;
        }

        string json =
            PlayerPrefs.GetString(
                SAVE_KEY
            );

        if (string.IsNullOrEmpty(json))
            return;

        SaveData data =
            JsonUtility.FromJson<SaveData>(
                json
            );

        if (data == null)
            return;

        if (
            InventoryManager.Instance !=
            null)
        {
            InventoryManager.Instance
                .LoadFromIDs(
                    data.unlockedTurrets,
                    data.selectedTurrets
                );
        }

        Debug.Log(
            "ИГРА ЗАГРУЖЕНА!"
        );
    }

    // =====================================================
    // GET SAVED LEVEL
    // =====================================================

    public int GetSavedTurretLevel(
        string turretID
    )
    {
        TurretSave saved =
            FindTurretSave(
                turretID
            );

        if (saved == null)
            return 0;

        return Mathf.Max(
            0,
            saved.level
        );
    }

    // =====================================================
    // GET SAVED HP
    // =====================================================

    public float GetSavedTurretHP(
        string turretID
    )
    {
        if (!HasSave())
            return -1f;

        TurretSave saved =
            FindTurretSave(
                turretID
            );

        if (saved == null)
            return -1f;

        return saved.hp;
    }

    // =====================================================
    // SAVE TURRET STATE
    // =====================================================

    public void SaveTurretState(
        string turretID,
        int level,
        float hp
    )
    {
        if (
            string.IsNullOrEmpty(
                turretID))
        {
            return;
        }

        SaveData data =
            LoadSaveData();

        TurretSave found =
            FindTurretSave(
                data,
                turretID
            );

        if (found == null)
        {
            found =
                new TurretSave();

            found.turretID =
                turretID;

            data.turretStates.Add(
                found
            );
        }

        found.level =
            level;

        found.hp =
            hp;

        SaveDataToPrefs(
            data
        );

        Debug.Log(
            "СОХРАНЕНО: " +
            turretID +
            " | LV " +
            (level + 1) +
            " | HP " +
            hp
        );
    }

    // =====================================================
    // FIND
    // =====================================================

    private TurretSave FindTurretSave(
        string turretID
    )
    {
        SaveData data =
            LoadSaveData();

        return FindTurretSave(
            data,
            turretID
        );
    }

    private TurretSave FindTurretSave(
        SaveData data,
        string turretID
    )
    {
        if (data == null)
            return null;

        if (data.turretStates == null)
            return null;

        foreach (
            TurretSave saved
            in data.turretStates)
        {
            if (saved == null)
                continue;

            if (
                saved.turretID ==
                turretID)
            {
                return saved;
            }
        }

        return null;
    }

    // =====================================================
    // LOAD SAVE DATA
    // =====================================================

    private SaveData LoadSaveData()
    {
        if (!HasSave())
        {
            return new SaveData();
        }

        string json =
            PlayerPrefs.GetString(
                SAVE_KEY
            );

        if (string.IsNullOrEmpty(json))
        {
            return new SaveData();
        }

        SaveData data =
            JsonUtility.FromJson<SaveData>(
                json
            );

        if (data == null)
        {
            return new SaveData();
        }

        if (data.unlockedTurrets == null)
        {
            data.unlockedTurrets =
                new List<string>();
        }

        if (data.selectedTurrets == null)
        {
            data.selectedTurrets =
                new List<string>();
        }

        if (data.turretStates == null)
        {
            data.turretStates =
                new List<TurretSave>();
        }

        return data;
    }

    // =====================================================
    // SAVE JSON
    // =====================================================

    private void SaveDataToPrefs(
        SaveData data
    )
    {
        string json =
            JsonUtility.ToJson(
                data
            );

        PlayerPrefs.SetString(
            SAVE_KEY,
            json
        );

        PlayerPrefs.Save();
    }

    // =====================================================
    // DELETE
    // =====================================================

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(
            SAVE_KEY
        );

        PlayerPrefs.Save();

        Debug.Log(
            "СОХРАНЕНИЕ УДАЛЕНО!"
        );
    }
}