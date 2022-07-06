using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct RoundSpawningData
{
    public int NormalZombieCount;
    public int FastZombieCount;

    public int Total => NormalZombieCount + FastZombieCount;
}

[CreateAssetMenu(fileName = "ZombieSpawningData", menuName = "ScriptableObjects/ZombieSpawningData", order = 1)]
public class ZombieSpawningData : ScriptableObject
{
    public RoundSpawningData[] RoundSpawningDatas;
}
