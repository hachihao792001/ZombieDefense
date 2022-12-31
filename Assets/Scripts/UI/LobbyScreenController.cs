using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public struct RoomDetail
{
    public string name;
    public int playerCount;
    public int maxPlayerCount;
}

public class LobbyScreenController : MonoBehaviour
{
    public List<RoomDetail> exampleRoomList = new List<RoomDetail>(){
        {
            new RoomDetail() {
                name = "Room 1",
                playerCount = 1,
                maxPlayerCount = 4
            }
        },
        {
            new RoomDetail() {
                name = "Room 2",
                playerCount = 2,
                maxPlayerCount = 4
            }
        },
        {
            new RoomDetail() {
                name = "Room 3",
                playerCount = 3,
                maxPlayerCount = 4
            }
        },
        {
            new RoomDetail() {
                name = "Room 4",
                playerCount = 4,
                maxPlayerCount = 4
            }
        },
        {
            new RoomDetail() {
                name = "Room 5",
                playerCount = 1,
                maxPlayerCount = 4
            }
        }
    };
    public GameObject roomItemPrefab;
    public Transform roomListContent;
    public RoomController roomController;
    public TMP_InputField codeRoom;

    public void Init()
    {
        foreach (Transform child in roomListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var room in exampleRoomList)
        {
            var roomItem = Instantiate(roomItemPrefab, roomListContent);
            roomItem.GetComponent<RoomItemController>().Init(room);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void JoinRoom()
    {
        gameObject.SetActive(false);
        roomController.gameObject.SetActive(true);
        roomController.Init("Room 1");
        codeRoom.text = "";
    }
}
