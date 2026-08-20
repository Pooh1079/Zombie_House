using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public static List<ZombieSpawner> AllSpawners = new List<ZombieSpawner>();

    public static int finishedSpawners = 0;

    [Header("Spawn")]
    public Transform spawnPoint;

    [Header("Waves")]
    public List<WaveData> waves = new List<WaveData>();

    private void Awake()
    {
        AllSpawners.Add(this);
    }

    private void OnDestroy()
    {
        AllSpawners.Remove(this);
    }

    public void StartWave(int waveIndex)
    {
        StopAllCoroutines();

        if (waveIndex >= waves.Count)
            return;

        if (finishedSpawners > 0)
            finishedSpawners = 0;

        StartCoroutine(SpawnRoutine(waves[waveIndex]));
    }

    IEnumerator SpawnRoutine(WaveData wave)
    {
        foreach (ZombieSpawn zombie in wave.zombies)
        {
            for (int i = 0; i < zombie.count; i++)
            {
                Instantiate(
                    zombie.zombiePrefab,
                    spawnPoint.position,
                    Quaternion.identity);

                GameManager.Instance.ZombieSpawned();

                yield return new WaitForSeconds(wave.delayBetweenSpawns);
            }
        }

        finishedSpawners++;

        if (finishedSpawners >= AllSpawners.Count)
        {
            GameManager.Instance.spawningFinished = true;

            if (GameManager.Instance.aliveZombies <= 0)
            {
                finishedSpawners = 0;

                GameManager.Instance.spawningFinished = false;

                WaveManager.Instance.WaveCompleted();
            }
        }
    }
}