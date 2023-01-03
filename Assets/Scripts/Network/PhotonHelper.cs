using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PhotonHelper : MonoBehaviourPunCallbacks
{
    #region Singleton
    static PhotonHelper _instance;
    public static PhotonHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                AssignSingleton(FindObjectOfType<PhotonHelper>());
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            AssignSingleton(this);
            InitPhoton();
        }
        else if (this != _instance)
        {
            Destroy(gameObject);
        }
    }

    static void AssignSingleton(PhotonHelper instance)
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
    public static void InitPhoton()
    {
        Debug.Log("Connecting to Photon...");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.NickName = PlayerPrefs.GetString("username", "Player" + Random.Range(1, 100));
        PhotonNetwork.SendRate = 40; //default 20
        PhotonNetwork.SerializationRate = 20; //default 10
        PhotonNetwork.ConnectUsingSettings();

        LoadingController.Instance.ShowLoading("Connecting to server...");
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
        LoadingController.Instance.HideLoading();
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

        LoadingController.Instance.ShowLoading("Creating room...");
    }

    public static void JoinRoom(string name)
    {
        PhotonNetwork.JoinRoom(name);
        LoadingController.Instance.ShowLoading("Joining room...");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log($"You, {GetPlayerString(PhotonNetwork.LocalPlayer)} joined room " + PhotonNetwork.CurrentRoom.Name);
        onJoinedRoom?.Invoke(PhotonNetwork.CurrentRoom);

        LoadingController.Instance.HideLoading();
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Join room failed: " + message);

        LoadingController.Instance.HideLoading();
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
        LoadingController.Instance.ShowLoading("Leaving room...");
    }
    public override void OnLeftRoom()
    {
        Debug.Log($"You, {GetPlayerString(PhotonNetwork.LocalPlayer)} left room");
        LoadingController.Instance.HideLoading();
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

        List<Photon.Realtime.Player> players = new List<Photon.Realtime.Player>(PhotonNetwork.CurrentRoom.Players.Values);
        players.Sort((a, b) => a.ActorNumber - b.ActorNumber);
        return players;
    }

    public static void LoadScene(string name)
    {
        PhotonNetwork.LoadLevel(name);
    }
    #endregion

    #region Ingame Helpers
    public static GameObject SpawnNewNetworkObject(string path, Vector3 position, Quaternion rotation)
    {
        return PhotonNetwork.Instantiate(path, position, rotation);
    }

    public static void DestroyNetworkObject(GameObject gameObject)
    {
        PhotonNetwork.Destroy(gameObject);
    }
    #endregion

    #region Custom properties
    static ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();

    public static void SetMyAvatarIndex(int index)
    {
        _myCustomProperties["avatar"] = index;
        PhotonNetwork.SetPlayerCustomProperties(_myCustomProperties);
    }

    public static int GetAvatarIndexOf(int actorNumber)
    {
        if (PhotonNetwork.CurrentRoom.Players[actorNumber].CustomProperties.ContainsKey("avatar"))
        {
            int result = (int)PhotonNetwork.CurrentRoom.Players[actorNumber].CustomProperties["avatar"];
            return result;
        }
        return 0;
    }
    #endregion

    string GetPlayerString(Photon.Realtime.Player player)
    {
        return $"( {player.ActorNumber} | {player.NickName} )";
    }
}
