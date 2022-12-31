using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomItemController : MonoBehaviour
{
    public TMP_Text textRoomName;
    public TMP_Text textPlayerCount;

    public void Init(RoomDetail room)
    {
        textRoomName.text = room.name;
        textPlayerCount.text = room.playerCount + "/" + room.maxPlayerCount;
    }
}
