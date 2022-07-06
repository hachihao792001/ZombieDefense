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
    public int CurrentRoundZombieCount => _zombieSpawningData.RoundSpawningDatas[CurrentRound - 1].Total;

    [SerializeField]
    private ZombieSpawningData _zombieSpawningData;
    [SerializeField]
    private Zombie _zombiePrefab;
    [SerializeField]
    private Zombie _fastZombiePrefab;
    [SerializeField]
    private Transform _zombiesParent;
    [SerializeField]
    private List<Transform> _zombieSpawnPositions;

    public bool IsPaused = false;
    [SerializeField]
    private PauseScreen _pauseScreen;
    public Action<float> OnSensitivitySliderChanged;

    private void Start()
    {
        SpawnZombies();
        OnNewRound?.Invoke();

        _pauseScreen.OnSensititySliderChangedAction = (float v) => OnSensitivitySliderChanged?.Invoke(v);
    }

    private void SpawnZombies()
    {
        int normalZombieCount = _zombieSpawningData.RoundSpawningDatas[CurrentRound - 1].NormalZombieCount;
        int fastZombieCount = _zombieSpawningData.RoundSpawningDatas[CurrentRound - 1].FastZombieCount;
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
        Zombie newZombie = Instantiate(zombiePrefab, _zombiesParent);
        newZombie.OnZombieDied = OnZombieDied;
        AssignZombiePosition(newZombie);
        Enemies.Add(newZombie);
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

    public void PauseOnClick()
    {
        Time.timeScale = 0;
        IsPaused = true;
        _pauseScreen.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        IsPaused = false;
    }
}
