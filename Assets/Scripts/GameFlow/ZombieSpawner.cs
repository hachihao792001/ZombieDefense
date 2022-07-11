using System;
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

    [SerializeField]
    private ZombieSpawningData _zombieSpawningData;
    [SerializeField]
    private Zombie _zombiePrefab;
    [SerializeField]
    private Zombie _fastZombiePrefab;
    [SerializeField]
    private List<Transform> _zombieSpawnPositions;

    private void Awake()
    {
        int count = _zombieSpawnPositions.Count;
        List<Transform> randomizedPositions = new List<Transform>();
        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, _zombieSpawnPositions.Count);
            randomizedPositions.Add(_zombieSpawnPositions[randomIndex]);
            _zombieSpawnPositions.RemoveAt(randomIndex);
        }

        _zombieSpawnPositions.AddRange(randomizedPositions);
    }

    public bool HasSpawningData(int round)
    {
        return round - 1 < _zombieSpawningData.RoundSpawningDatas.Length;
    }

    public void SpawnZombies()
    {
        int currentRound = GameController.Instance.CurrentRound;
        int normalZombieCount = _zombieSpawningData.RoundSpawningDatas[currentRound - 1].NormalZombieCount;
        int fastZombieCount = _zombieSpawningData.RoundSpawningDatas[currentRound - 1].FastZombieCount;
        for (int i = 0; i < normalZombieCount; i++)
        {
            SpawnAZombie(_zombiePrefab);
        }
        for (int i = 0; i < fastZombieCount; i++)
        {
            SpawnAZombie(_fastZombiePrefab);
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
            zombie.ZombieMoving.WarpAgent(hit.position);
        }
    }

    private void OnZombieDied(Zombie whichZombie)
    {
        Zombies.Remove(whichZombie);

        if (Zombies.Count == 0)
        {
            GameController.Instance.EndRound();
        }

        OnZombieDiedAction?.Invoke(whichZombie);
        whichZombie.OnZombieDied -= OnZombieDied;
    }
}
