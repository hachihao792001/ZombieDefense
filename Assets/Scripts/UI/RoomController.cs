using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class RoomController : MonoBehaviour
{
    public TMP_Text textRoomName;
    [SerializeField] List<PlayerItemController> _playerItems;

    private void Start()
    {
        PhotonHelper.onLeftCurrentRoom += OnLeftRoom;
        PhotonHelper.onOtherPlayerJoinedRoom += OnOtherPlayerJoinedRoom;
        PhotonHelper.onOtherPlayerLeftRoom += OnOtherPlayerLeftRoom;
    }

    private void OnDestroy()
    {
        PhotonHelper.onLeftCurrentRoom -= OnLeftRoom;
        PhotonHelper.onOtherPlayerJoinedRoom -= OnOtherPlayerJoinedRoom;
        PhotonHelper.onOtherPlayerLeftRoom -= OnOtherPlayerLeftRoom;
    }
    public void Init(string roomName)
    {
        textRoomName.text = "Room " + roomName;
        RefreshPlayerListUI();
    }

    public void RefreshPlayerListUI()
    {
        List<Photon.Realtime.Player> playersInRoom = PhotonHelper.GetPlayerListInRoom();
        for (int i = 0; i < playersInRoom.Count; i++)
        {
            _playerItems[i].Init(playersInRoom[i]);
        }
        for (int i = playersInRoom.Count; i < _playerItems.Count; i++)
        {
            _playerItems[i].Init(null);
        }
    }

    public void Close()
    {
        PhotonHelper.LeaveCurrentRoom();
    }

    private void OnLeftRoom()
    {
        gameObject.SetActive(false);
    }

    private void OnOtherPlayerLeftRoom(Photon.Realtime.Player obj)
    {
        RefreshPlayerListUI();
    }

    private void OnOtherPlayerJoinedRoom(Photon.Realtime.Player obj)
    {
        RefreshPlayerListUI();
    }

    public void StartGame()
    {
        PhotonHelper.LoadScene("Game");
    }
}
