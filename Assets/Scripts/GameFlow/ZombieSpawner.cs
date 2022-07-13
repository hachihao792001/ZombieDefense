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
        RandomizeSpawnPositions();
    }

    private void RandomizeSpawnPositions()
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

        List<Zombie> thisRoundZombies = new List<Zombie>();
        for (int i = 0; i < _zombieSpawningData.RoundSpawningDatas[currentRound - 1].Total; i++)
        {
            int random = UnityEngine.Random.Range(0, 2);
            if (random == 0)
            {
                if (normalZombieCount > 0)
                {
                    thisRoundZombies.Add(_zombiePrefab);
                    normalZombieCount--;
                }
                else
                {
                    thisRoundZombies.Add(_fastZombiePrefab);
                    fastZombieCount--;
                }
            }
            else
            {
                if (fastZombieCount > 0)
                {
                    thisRoundZombies.Add(_fastZombiePrefab);
                    fastZombieCount--;
                }
                else
                {
                    thisRoundZombies.Add(_zombiePrefab);
                    normalZombieCount--;
                }
            }
        }

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

        if (Zombies.Count == 0)
        {
            GameController.Instance.EndRound();
        }

        OnZombieDiedAction?.Invoke(whichZombie);
        whichZombie.OnZombieDied -= OnZombieDied;
    }
}
