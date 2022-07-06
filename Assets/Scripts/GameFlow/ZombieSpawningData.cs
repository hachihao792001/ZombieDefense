using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieSpawningData", menuName = "ScriptableObjects/ZombieSpawningData", order = 1)]
public class ZombieSpawningData : ScriptableObject
{
    public int[] ZombieSpawnCount;
}
