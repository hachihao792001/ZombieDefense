using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GameController : MonoSingleton<GameController>
{
    [Header("FPS Controller")]
    [SerializeField] Player PlayerPrefab;
    public Player Player;

    public string PlayerPrefabName;
    public Vector3 PlayerStartSpawnPos;

    [Header("Game")]
    [SerializeField]
    public ZombieSpawner ZombieSpawner;
    [SerializeField]
    private RV _rv;
    [SerializeField]
    private MoneyManager _moneyManager;
    [SerializeField]
    private MoneyEarningData _moneyEarningData;
    [SerializeField]
    private PauseScreen _pauseScreen;
    [SerializeField]
    private GameObject _winScreen;
    [SerializeField]
    private LoseScreenController _loseScreen;

    public List<Transform> Allies;
    public int CurrentRound = 1;
    public Action OnNewRound;
    public bool IsPaused = false;
    public Action<float> OnSensitivitySliderChanged;
    public Action<Zombie> OnZombieDiedAction;

    public static bool IsGameOver = false;

    private void Start()
    {
        ZombieSpawner.SpawnZombies();
        ZombieSpawner.OnZombieDiedAction += OnZombieDied;

        OnNewRound?.Invoke();

        _pauseScreen.ResumeOnClick();
        _pauseScreen.OnSensititySliderChangedAction = (float v) => OnSensitivitySliderChanged?.Invoke(v);

        IsGameOver = false;
    }

    private void Update()
    {
        if (IsGameOver) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsPaused)
            {
                PauseOnClick();
                UnlockCursor();
            }
            else
            {
                _pauseScreen.ResumeOnClick();
            }
        }
    }

    public void SpawnNewPlayer()
    {
        GameObject spawned = PhotonLobbyHelper.SpawnNewObject(PlayerPrefabName, PlayerStartSpawnPos, Quaternion.identity);
        Player = spawned.GetComponent<Player>();
    }

    //public void SetMobileControl(bool mobileControl)
    //{
    //    _pcPlayer.gameObject.SetActive(!mobileControl);
    //    _mobilePlayer.gameObject.SetActive(mobileControl);
    //    Player = mobileControl ? _mobilePlayer : _pcPlayer;

    //    Allies = new List<Transform>();
    //    for (int i = 0; i < _rv.AttackPositions.Length; i++)
    //        Allies.Add(_rv.AttackPositions[i]);
    //    Allies.Add(Player.transform);
    //}


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
        _moneyManager.EarnMoney(_moneyEarningData.Finish1Round);

        if (ZombieSpawner.HasSpawningData(CurrentRound))
        {
            ZombieSpawner.SpawnZombies();
            OnNewRound?.Invoke();
        }
        else
        {
            GameOver(true);
            OnNewRound?.Invoke();
        }
    }

    public void GameOver(bool win, string loseReason = "")
    {
        UnlockCursor();
        IsGameOver = true;
        if (win)
        {
            _winScreen.gameObject.SetActive(true);
        }
        else
        {
            _loseScreen.Show(loseReason);
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

    public void PauseOnClick()
    {
        IsPaused = true;
        _pauseScreen.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        IsPaused = false;
        LockCursor();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
