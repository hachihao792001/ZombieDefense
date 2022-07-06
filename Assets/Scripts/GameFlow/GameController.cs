using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GameController : MonoSingleton<GameController>
{
    [SerializeField]
    public List<Transform> Allies;
    public List<Zombie> Enemies;

    public int CurrentRound = 1;

    public Action<Zombie> OnZombieDiedAction;
    public Action OnNewRound;
    public int CurrentRoundZombieCount => _zombieSpawningData.ZombieSpawnCount[CurrentRound - 1];

    [SerializeField]
    private ZombieSpawningData _zombieSpawningData;
    [SerializeField]
    private Zombie _zombiePrefab;
    [SerializeField]
    private Transform _zombiesParent;
    [SerializeField]
    private List<Transform> _zombieSpawnPositions;



    private void Start()
    {
        SpawnZombies();
        OnNewRound?.Invoke();
    }

    private void SpawnZombies()
    {

        for (int i = 0; i < CurrentRoundZombieCount; i++)
        {
            Zombie newZombie = Instantiate(_zombiePrefab, _zombiesParent);
            newZombie.OnZombieDied = OnZombieDied;
            AssignZombiePosition(newZombie);
            Enemies.Add(newZombie);
        }
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
        Enemies.Remove(whichZombie);

        if (Enemies.Count == 0)
        {
            CurrentRound++;
            OnNewRound?.Invoke();
            SpawnZombies();
        }

        OnZombieDiedAction?.Invoke(whichZombie);
        whichZombie.OnZombieDied -= OnZombieDied;
    }

    public float[] GetDistanceToAllies(Vector3 pos)
    {
        float[] distances = new float[Allies.Count];
        for (int i = 0; i < Allies.Count; i++)
        {
            distances[i] = Vector3.Distance(pos, Allies[i].position);
        }
        return distances;
    }
}
