using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class MinimapUI : MonoBehaviour
{
    [SerializeField]
    private Camera _minimapCamera;
    [SerializeField]
    private Transform _enemyIndicatorsParent;
    [SerializeField]
    private EnemyIndicator _enemyIndicatorPrefab;
    [SerializeField]
    private Dictionary<Zombie, EnemyIndicator> _enemyIndicatorPairs;

    void Start()
    {
        _enemyIndicatorPairs = new Dictionary<Zombie, EnemyIndicator>();

        GameController.Instance.onZombieDied = OnZombieDied;
    }

    void Update()
    {
        List<Zombie> enemies = GameController.Instance.Enemies;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!_enemyIndicatorPairs.ContainsKey(enemies[i]))
            {
                if (IsVisbleOnMinimap(enemies[i]))
                {
                    EnemyIndicator spawnIndicator = LeanPool.Spawn(_enemyIndicatorPrefab, _enemyIndicatorsParent);
                    spawnIndicator.SetData(_minimapCamera, transform, enemies[i].transform);
                    _enemyIndicatorPairs.Add(enemies[i], spawnIndicator);
                }
            }
            else
            {
                if (!IsVisbleOnMinimap(enemies[i]))
                {
                    LeanPool.Despawn(_enemyIndicatorPairs[enemies[i]]);
                    _enemyIndicatorPairs.Remove(enemies[i]);
                }
            }
        }

    }

    public void OnZombieDied(Zombie whichZombie)
    {
        LeanPool.Despawn(_enemyIndicatorPairs[whichZombie].gameObject);
        _enemyIndicatorPairs.Remove(whichZombie);
    }

    private bool IsVisbleOnMinimap(Zombie enemy)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_minimapCamera);
        return GeometryUtility.TestPlanesAABB(planes, enemy.GetComponent<Collider>().bounds);
    }
}