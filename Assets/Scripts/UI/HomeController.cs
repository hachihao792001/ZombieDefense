using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class HomeController : MonoBehaviour
{
    public PopupChangeName popupChangeName;
    public PopupChangeName popupRoomName;
    public RoomController roomController;
    public LobbyScreenController lobbyScreenController;
    public TMP_Text username;

    private void Start()
    {
        popupChangeName.OnApplyChange += ApplyChangeUserName;
        popupRoomName.OnApplyChange += ApplyRoomName;

        PhotonHelper.onJoinedRoom += OnJoinedRoom;

        if (PhotonNetwork.CurrentRoom != null)  //back from gameplay
        {
            OnJoinedRoom(PhotonNetwork.CurrentRoom);
        }
    }

    private void OnDestroy()
    {
        popupChangeName.OnApplyChange -= ApplyChangeUserName;
        popupRoomName.OnApplyChange -= ApplyRoomName;

        PhotonHelper.onJoinedRoom -= OnJoinedRoom;
    }

    public void ChangeUserNameOnClick()
    {
        popupChangeName.gameObject.SetActive(true);
        popupChangeName.Init(PlayerPrefs.GetString("username", ""));
    }

    public void ApplyChangeUserName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            WarningController.Instance.ShowWarning("Username cannot be empty");
            return;
        }
        username.text = name;
        PlayerPrefs.SetString("username", name);
        PhotonNetwork.NickName = name;
        popupChangeName.Close();
    }

    public void CreateRoomOnClick()
    {
        popupRoomName.gameObject.SetActive(true);
        popupRoomName.Init("");
    }

    public void ApplyRoomName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            WarningController.Instance.ShowWarning("Room name cannot be empty");
            return;
        }
        PlayerPrefs.SetString("roomname", name);
        popupRoomName.Close();

        PhotonHelper.CreateRoom(name);
    }

    public void OnJoinedRoom(Room room)
    {
        roomController.gameObject.SetActive(true);
        roomController.Init(room.Name);
    }

    public void LobbyOnClick()
    {
        lobbyScreenController.gameObject.SetActive(true);
        lobbyScreenController.Init();
    }

    private void OnEnable()
    {
        username.text = PhotonNetwork.NickName;
    }
}
