using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomItemController : MonoBehaviour
{
    public TMP_Text textRoomName;
    public TMP_Text textPlayerCount;
    public RoomDetail roomDetail;

    public void Init(RoomDetail room)
    {
        this.roomDetail = room;
        textRoomName.text = room.name;
        textPlayerCount.text = room.playerCount + "/" + room.maxPlayerCount;
    }

    public void OnClick()
    {
        PhotonLobbyHelper.JoinRoom(roomDetail.name);
    }
}
