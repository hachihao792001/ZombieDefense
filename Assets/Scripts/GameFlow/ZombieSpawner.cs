using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    public List<Zombie> Zombies;

    public Action<Zombie> OnZombieDiedAction;
    public int CurrentRoundZombieCount => _zombieSpawningData.RoundSpawningDatas[GameController.Instance.CurrentRound - 1].Total;

    [SerializeField]
    private ZombieSpawningData _zombieSpawningData;
    [SerializeField]
    private Zombie _zombiePrefab;
    [SerializeField]
    private Zombie _fastZombiePrefab;
    [SerializeField]
    private List<Transform> _zombieSpawnPositions;

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
        Zombie newZombie = Instantiate(zombiePrefab, transform);
        newZombie.OnZombieDied = OnZombieDied;
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
