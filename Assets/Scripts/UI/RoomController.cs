using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class RoomController : MonoBehaviour
{
    public TMP_Text textRoomName;

    public void Init(string roomName)
    {
        textRoomName.text = "Room " + roomName;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
