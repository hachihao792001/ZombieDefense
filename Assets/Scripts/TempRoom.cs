using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempRoom : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("C pressed");
            PhotonLobbyHelper.CreateRoom("a");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            PhotonLobbyHelper.JoinRoom("a");
        }
    }
}
