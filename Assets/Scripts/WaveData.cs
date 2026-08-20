using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZombieSpawn
{
    public GameObject zombiePrefab;
    public int count = 5;
}

[System.Serializable]
public class WaveData
{
    public List<ZombieSpawn> zombies = new List<ZombieSpawn>();

    public float delayBetweenSpawns = 1f;
}