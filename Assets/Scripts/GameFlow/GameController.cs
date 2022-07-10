using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GameController : MonoSingleton<GameController>
{
    [Header("FPS Controller")]
    [SerializeField]
    private Player _pcPlayer;
    [SerializeField]
    private Player _mobilePlayer;
    public Player Player;

    [Header("Game")]
    [SerializeField]
    public ZombieSpawner ZombieSpawner;

    [SerializeField]
    private RV _rv;
    [SerializeField]
    public List<Transform> Allies;


    [SerializeField]
    private MoneyManager _moneyManager;
    [SerializeField]
    private MoneyEarningData _moneyEarningData;

    public int CurrentRound = 1;

    public Action OnNewRound;

    public bool IsPaused = false;
    [SerializeField]
    private PauseScreen _pauseScreen;
    public Action<float> OnSensitivitySliderChanged;

    public Action<Zombie> OnZombieDiedAction;

    private void Start()
    {
        ZombieSpawner.SpawnZombies();
        ZombieSpawner.OnZombieDiedAction += OnZombieDied;

        OnNewRound?.Invoke();

        _pauseScreen.ResumeOnClick();
        _pauseScreen.OnSensititySliderChangedAction = (float v) => OnSensitivitySliderChanged?.Invoke(v);
    }

    public void SetMobileControl(bool mobileControl)
    {
        _pcPlayer.gameObject.SetActive(!mobileControl);
        _mobilePlayer.gameObject.SetActive(mobileControl);
        Player = mobileControl ? _mobilePlayer : _pcPlayer;

        Allies = new List<Transform>();
        for (int i = 0; i < _rv.AttackPositions.Length; i++)
            Allies.Add(_rv.AttackPositions[i]);
        Allies.Add(Player.transform);
    }


    private void OnZombieDied(Zombie whichZombie)
    {
        if (whichZombie.ZombieType == ZombieType.Normal)
        {
            _moneyManager.EarnMoney(_moneyEarningData.KillingZombie);
        }
        else if (whichZombie.ZombieType == ZombieType.Fast)
        {
            _moneyManager.EarnMoney(_moneyEarningData.KillingFastZombie);
        }

        OnZombieDiedAction?.Invoke(whichZombie);
    }

    public void EndRound()
    {
        CurrentRound++;
        OnNewRound?.Invoke();
        _moneyManager.EarnMoney(_moneyEarningData.Finish1Round);
        ZombieSpawner.SpawnZombies();
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
