using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public struct RoomDetail
{
    public string name;
    public int playerCount;
    public int maxPlayerCount;

    public RoomDetail(string name, int playerCount, int maxPlayerCount)
    {
        this.name = name;
        this.playerCount = playerCount;
        this.maxPlayerCount = maxPlayerCount;
    }
}

public class LobbyScreenController : MonoBehaviour
{
    public RoomItemController roomItemPrefab;
    public Transform roomListContent;
    public RoomController roomController;
    public TMP_InputField codeRoom;

    private void Start()
    {
        PhotonLobbyHelper.onAvailableRoomListUpdated += RefreshAvailableRoomListUI;
    }
    private void OnDestroy()
    {
        PhotonLobbyHelper.onAvailableRoomListUpdated -= RefreshAvailableRoomListUI;
    }

    public void Init()
    {
        RefreshAvailableRoomListUI();
    }

    private void RefreshAvailableRoomListUI()
    {
        foreach (Transform child in roomListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var room in PhotonLobbyHelper.GetAvailableRoomDetails())
        {
            RoomItemController roomItem = Instantiate(roomItemPrefab, roomListContent);
            roomItem.Init(room);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(codeRoom.text))
        {
            WarningController.Instance.ShowWarning("Room code cannot be empty");
            return;
        }
        gameObject.SetActive(false);
        roomController.gameObject.SetActive(true);
        roomController.Init("Room 1");
        codeRoom.text = "";
    }
}
