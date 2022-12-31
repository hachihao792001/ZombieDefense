using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{

    private void Update()
    {
        if (GameController.Instance.Player == null) return;
        Transform player = GameController.Instance.Player.CameraAndWeapon;
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, player.eulerAngles.y, transform.eulerAngles.z);
    }
}
