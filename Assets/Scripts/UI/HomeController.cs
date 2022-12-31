using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeController : MonoBehaviour
{
    public PopupChangeName popupChangeName;
    public PopupChangeName popupRoomName;
    public RoomController roomController;
    public LobbyScreenController lobbyScreenController;
    public TMP_Text username;

    public void ChangeName()
    {
        popupChangeName.gameObject.SetActive(true);
        popupChangeName.Init(PlayerPrefs.GetString("username", ""));
    }

    public void ApplyChangeName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            WarningController.Instance.ShowWarning("Username cannot be empty");
            return;
        }
        username.text = name;
        PlayerPrefs.SetString("username", name);
        popupChangeName.Close();
    }

    public void CreateRoom()
    {
        popupRoomName.gameObject.SetActive(true);
        popupRoomName.Init("");
    }

    public void ApplyChangeRoomName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            WarningController.Instance.ShowWarning("Room name cannot be empty");
            return;
        }
        PlayerPrefs.SetString("roomname", name);
        roomController.gameObject.SetActive(true);
        roomController.Init(name);
        popupRoomName.Close();
    }

    public void JoinRoom()
    {
        lobbyScreenController.gameObject.SetActive(true);
        lobbyScreenController.Init();
    }

    private void Start()
    {
        popupChangeName.OnApplyChange += ApplyChangeName;
        popupRoomName.OnApplyChange += ApplyChangeRoomName;
    }

    private void OnEnable()
    {
        username.text = PlayerPrefs.GetString("username", "Username");
    }
}
