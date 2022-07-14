using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Lean.Pool;

public class ZombieSpawner : MonoBehaviour
{
    public List<Zombie> Zombies;

    public Action<Zombie> OnZombieDiedAction;
    public int CurrentRoundZombieCount
    {
        get
        {
            if (HasSpawningData(GameController.Instance.CurrentRound))
                return _zombieSpawningData.RoundSpawningDatas[GameController.Instance.CurrentRound - 1].Total;
            else
                return 0;
        }
    }

    public int TotalRound => _zombieSpawningData.RoundSpawningDatas.Length;

    [SerializeField]
    private ZombieSpawningData _zombieSpawningData;
    [SerializeField]
    private Zombie _zombiePrefab;
    [SerializeField]
    private Zombie _fastZombiePrefab;
    [SerializeField]
    private Zombie _bigZombiePrefab;
    [SerializeField]
    private List<Transform> _zombieSpawnPositions;

    private void Awake()
    {
        _zombieSpawnPositions.Randomize();
    }

    public bool HasSpawningData(int round)
    {
        return round - 1 < _zombieSpawningData.RoundSpawningDatas.Length;
    }

    public void SpawnZombies()
    {
        int currentRound = GameController.Instance.CurrentRound;
        RoundSpawningData currentRoundSpawningData = _zombieSpawningData.RoundSpawningDatas[currentRound - 1];

        List<Zombie> thisRoundZombies = new List<Zombie>();
        for (int i = 0; i < currentRoundSpawningData.NormalZombieCount; i++)
            thisRoundZombies.Add(_zombiePrefab);
        for (int i = 0; i < currentRoundSpawningData.FastZombieCount; i++)
            thisRoundZombies.Add(_fastZombiePrefab);
        for (int i = 0; i < currentRoundSpawningData.BigZombieCount; i++)
            thisRoundZombies.Add(_bigZombiePrefab);

        thisRoundZombies.Randomize();
        StartCoroutine(spawnZombieCoroutine(thisRoundZombies));
    }

    IEnumerator spawnZombieCoroutine(List<Zombie> thisRoundZombies)
    {
        while (thisRoundZombies.Count > 0)
        {
            SpawnAZombie(thisRoundZombies[0]);
            thisRoundZombies.RemoveAt(0);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void SpawnAZombie(Zombie zombiePrefab)
    {
        Zombie newZombie = LeanPool.Spawn(zombiePrefab, transform);
        newZombie.OnZombieDied = OnZombieDied;
        newZombie.Init();
        AssignZombiePosition(newZombie);
        Zombies.Add(newZombie);
    }


    int _currentSpawnPositionIndex = 0;
    private void AssignZombiePosition(Zombie zombie)
    {
        Vector3 randomPosition = _zombieSpawnPositions[_currentSpawnPositionIndex].position;
        _currentSpawnPositionIndex++;
        if (_currentSpawnPositionIndex >= _zombieSpawnPositions.Count)
            _currentSpawnPositionIndex = 0;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, Mathf.Infinity, NavMesh.AllAreas))
        {
            zombie.transform.position = hit.position;
            zombie.ZombieMoving.SetAgentEnable(true);
        }
    }

    private void OnZombieDied(Zombie whichZombie)
    {
        Zombies.Remove(whichZombie);

        OnZombieDiedAction?.Invoke(whichZombie);
        whichZombie.OnZombieDied -= OnZombieDied;

        if (Zombies.Count == 0)
        {
            GameController.Instance.EndRound();
        }
    }
}
