using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GameController : OneSceneMonoSingleton<GameController>
{
    private readonly byte GameOverEventCode = 7;

    public string PlayerPrefabName;
    public Vector3 PlayerSpawnAreaCenter;
    public float PlayerSpawnAreaRadius;

    public Player Player;
    public List<Player> AlivePlayers;

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
        if (PhotonNetwork.IsMasterClient)
        {
            ZombieSpawner.SpawnZombies();
            ZombieSpawner.OnZombieDiedAction += OnZombieDied;
        }

        OnNewRound?.Invoke();

        _pauseScreen.ResumeOnClick();
        _pauseScreen.OnSensititySliderChangedAction = (float v) => OnSensitivitySliderChanged?.Invoke(v);

        IsGameOver = false;

        SpawnLocalPlayer();
    }

    private void Update()
    {
        if (IsGameOver) return;
        if (Input.GetKeyDown(KeyCode.P))
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

    public void SpawnLocalPlayer()
    {
        Vector2 randomAreaLocalPos = Random.insideUnitCircle * PlayerSpawnAreaRadius;
        Vector3 newPlayerPos = new Vector3(PlayerSpawnAreaCenter.x + randomAreaLocalPos.x, PlayerSpawnAreaCenter.y, PlayerSpawnAreaCenter.z + randomAreaLocalPos.y);
        GameObject spawned = PhotonHelper.SpawnNewNetworkObject(PlayerPrefabName, newPlayerPos, Quaternion.identity);
        Player = spawned.GetComponent<Player>();
    }

    public void OnNewPlayerSpawned(int viewId)
    {
        Player newPlayer = PhotonView.Find(viewId).GetComponent<Player>();
        Allies.Add(newPlayer.transform);
        AlivePlayers.Add(newPlayer);
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
        else if (whichZombie.ZombieType == ZombieType.Big)
        {
            _moneyManager.EarnMoney(_moneyEarningData.KillingBigZombie);
        }

        OnZombieDiedAction?.Invoke(whichZombie);
    }

    public void EndRound()
    {
        CurrentRound++;
        _moneyManager.EarnMoney(_moneyEarningData.Finish1Round);

        if (PhotonNetwork.IsMasterClient && ZombieSpawner.HasSpawningData(CurrentRound))
        {
            ZombieSpawner.SpawnZombies();
            OnNewRound?.Invoke();
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
                GameOver(true);
            OnNewRound?.Invoke();
        }
    }

    public void OnPlayerDied(int viewId)
    {
        Player diedPlayer = PhotonView.Find(viewId).GetComponent<Player>();
        AlivePlayers.Remove(diedPlayer);
        Allies.Remove(diedPlayer.transform);

        if (AlivePlayers.Count == 0)
        {
            GameOver(false, "All players are dead!");
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }
    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == GameOverEventCode)
        {
            Debug.Log("Receive GameOver event");
            object[] datas = (object[])obj.CustomData;
            bool win = (bool)datas[0];
            string loseReason = (string)datas[1];

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
    }
    public void GameOver(bool win, string loseReason = "")
    {
        Debug.Log("GameOver() -> RaiseEvent " + PhotonNetwork.IsMasterClient);
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(GameOverEventCode, new object[] { win, loseReason }, raiseEventOptions, SendOptions.SendReliable);
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
