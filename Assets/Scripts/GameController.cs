using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoSingleton<GameController>
{
    [SerializeField]
    private NeedDefendingBuilding[] PossibleNeedDefendingBuildings;
    public List<Transform> Allies;
    private NeedDefendingBuilding _currentDefendingBuilding;

    [SerializeField]
    private int _zombieSpawnCount;
    [SerializeField]
    private GameObject _zombiePrefab;
    [SerializeField]
    private Transform _zombiesParent;

    private void Start()
    {
        if (PossibleNeedDefendingBuildings.Length > 0)
            AssignNewBuildingToDefend();
        SpawnZombies();
    }

    private void AssignNewBuildingToDefend()
    {
        if (_currentDefendingBuilding != null)
        {
            _currentDefendingBuilding.Deactivate();
            Allies.Remove(_currentDefendingBuilding.transform);
        }

        NeedDefendingBuilding newBuildingToDefend = PossibleNeedDefendingBuildings[Random.Range(0, PossibleNeedDefendingBuildings.Length)];
        while (newBuildingToDefend == _currentDefendingBuilding)
        {
            newBuildingToDefend = PossibleNeedDefendingBuildings[Random.Range(0, PossibleNeedDefendingBuildings.Length)];
        }
        _currentDefendingBuilding = newBuildingToDefend;

        _currentDefendingBuilding.Activate();
        Allies.Add(_currentDefendingBuilding.transform);
    }

    private void SpawnZombies()
    {
        for (int i = 0; i < _zombieSpawnCount; i++)
        {
            GameObject newZombie = Instantiate(_zombiePrefab, _zombiesParent);
            PutZombieAtRandomPosition(newZombie);
        }
    }

    private void PutZombieAtRandomPosition(GameObject zombie)
    {
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

        int vertexIndex = Random.Range(0, triangulation.vertices.Length);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(triangulation.vertices[vertexIndex], out hit, 2f, 0))
        {
            zombie.GetComponent<ZombieMoving>().WarpAgent(hit.position);
        }
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
