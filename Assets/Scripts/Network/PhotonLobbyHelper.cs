using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PhotonLobbyHelper : MonoBehaviourPunCallbacks
{
    #region Singleton
    static PhotonLobbyHelper _instance;
    public static PhotonLobbyHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                AssignSingleton(FindObjectOfType<PhotonLobbyHelper>());
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            AssignSingleton(this);
        }
        else if (this != _instance)
        {
            Destroy(gameObject);
        }
    }

    static void AssignSingleton(PhotonLobbyHelper instance)
    {
        _instance = instance;
        DontDestroyOnLoad(_instance.gameObject);
    }
    #endregion

    public static Action<Room> onJoinedRoom;
    public static Action onLeftCurrentRoom;
    public static Action onAvailableRoomListUpdated;
    public static Action<Photon.Realtime.Player> onOtherPlayerJoinedRoom;
    public static Action<Photon.Realtime.Player> onOtherPlayerLeftRoom;

    #region Init Photon
    private void Start()
    {
        InitPhoton();
    }
    public static void InitPhoton()
    {
        Debug.Log("Connecting to Photon...");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.NickName = "Player" + Random.Range(1, 100);
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon");
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);

        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from Photon " + cause);
    }
    #endregion

    #region Create and Join room
    public static void CreateRoom(string name, byte maxPlayer = 3)
    {
        if (!PhotonNetwork.IsConnected)
            return;
        RoomOptions options = new RoomOptions();
        options.PublishUserId = true;
        options.MaxPlayers = maxPlayer;
        PhotonNetwork.JoinOrCreateRoom(name, options, TypedLobby.Default);
    }

    public static void JoinRoom(string name)
    {
        PhotonNetwork.JoinRoom(name);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log($"You, {GetPlayerString(PhotonNetwork.LocalPlayer)} joined room " + PhotonNetwork.CurrentRoom.Name);
        onJoinedRoom?.Invoke(PhotonNetwork.CurrentRoom);
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"Player {GetPlayerString(newPlayer)} joined room");
        onOtherPlayerJoinedRoom?.Invoke(newPlayer);
    }
    #endregion

    #region Leave room
    public static void LeaveCurrentRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        Debug.Log($"You, {GetPlayerString(PhotonNetwork.LocalPlayer)} left room");
        onLeftCurrentRoom?.Invoke();
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log($"Player {GetPlayerString(otherPlayer)} left room");
        onOtherPlayerLeftRoom?.Invoke(otherPlayer);
    }
    #endregion

    #region Room list
    static List<RoomInfo> cachedRoomInfos = new List<RoomInfo>();
    public static List<RoomDetail> GetAvailableRoomDetails()
    {
        return cachedRoomInfos.ConvertAll(x =>
            new RoomDetail(x.Name, x.PlayerCount, x.MaxPlayers)
        );
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        cachedRoomInfos.Clear();
        foreach (RoomInfo roomInfo in roomList)
        {
            if (!roomInfo.RemovedFromList)
            {
                cachedRoomInfos.Add(roomInfo);
            }
        }
        onAvailableRoomListUpdated?.Invoke();
    }
    #endregion

    #region Inside room
    public static List<Photon.Realtime.Player> GetPlayerListInRoom()
    {
        if (PhotonNetwork.CurrentRoom == null)
            return new List<Photon.Realtime.Player>();

        return new List<Photon.Realtime.Player>(PhotonNetwork.CurrentRoom.Players.Values);
    }

    public static void LoadScene(string name)
    {
        PhotonNetwork.LoadLevel(name);
    }
    #endregion

    #region Ingame Helpers
    public static GameObject SpawnNewObject(string path, Vector3 position, Quaternion rotation)
    {
        return PhotonNetwork.Instantiate(path, position, rotation);
    }
    #endregion

    string GetPlayerString(Photon.Realtime.Player player)
    {
        return $"( {player.ActorNumber} | {player.NickName} )";
    }
}
