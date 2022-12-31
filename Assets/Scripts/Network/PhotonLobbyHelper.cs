using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobbyHelper : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        InitPhoton();
    }

    public static void InitPhoton()
    {
        Debug.Log("Connecting to Photon...");
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

    public static GameObject SpawnNewObject(string path, Vector3 position, Quaternion rotation)
    {
        return PhotonNetwork.Instantiate(path, position, rotation);
    }
    public override void OnJoinedRoom()
    {
        Photon.Realtime.Player localPlayer = PhotonNetwork.LocalPlayer;
        Debug.Log($"You, ( {localPlayer.ActorNumber} | {localPlayer.NickName} ) joined room " + PhotonNetwork.CurrentRoom.Name);
        GameController.Instance.SpawnNewPlayer();
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"Player ( {newPlayer.ActorNumber} | {newPlayer.NickName} ) joined room");
    }

}
