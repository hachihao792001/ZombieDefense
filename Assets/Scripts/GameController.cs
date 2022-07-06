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

    [SerializeField]
    private int _zombieSpawnCount;
    [SerializeField]
    private Zombie _zombiePrefab;
    [SerializeField]
    private Transform _zombiesParent;
    [SerializeField]
    private Vector3 _minSpawnPosition;
    [SerializeField]
    private Vector3 _maxSpawnPosition;

    public Action<Zombie> onZombieDied;

    private void Start()
    {
        SpawnZombies();
    }

    private void SpawnZombies()
    {
        for (int i = 0; i < _zombieSpawnCount; i++)
        {
            Zombie newZombie = Instantiate(_zombiePrefab, _zombiesParent);
            newZombie.OnZombieDied = OnZombieDied;
            PutZombieAtRandomPosition(newZombie);
            Enemies.Add(newZombie);
        }
    }

    private void PutZombieAtRandomPosition(Zombie zombie)
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(_minSpawnPosition.x, _maxSpawnPosition.x),
            Random.Range(_minSpawnPosition.y, _maxSpawnPosition.y),
            Random.Range(_minSpawnPosition.z, _maxSpawnPosition.z));

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, Mathf.Infinity, NavMesh.AllAreas))
        {
            zombie.ZombieMoving.WarpAgent(hit.position);
        }
    }

    private void OnZombieDied(Zombie whichZombie)
    {
        onZombieDied?.Invoke(whichZombie);

        whichZombie.OnZombieDied -= OnZombieDied;
        Enemies.Remove(whichZombie);
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
