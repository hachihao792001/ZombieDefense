using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlying : MonoBehaviourPun
{
    [SerializeField] float _speed;
    float _yaw, _pitch;
    [SerializeField] float _sensitivity;

    void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
        if (GameController.Instance.IsPaused || GameController.IsGameOver) return;

        UpdateMouse();

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (input != Vector2.zero)
        {
            Vector3 moveDir = new Vector3(input.x, 0, input.y).normalized;
            transform.Translate(moveDir * _speed);
        }
    }

    private void UpdateMouse()
    {
        _yaw += Input.GetAxis("Mouse X") * _sensitivity;
        _pitch -= Input.GetAxis("Mouse Y") * _sensitivity;

        transform.eulerAngles = new Vector3(0, _yaw, 0);
        transform.localEulerAngles = new Vector3(_pitch, transform.localEulerAngles.y, 0);

    }
}
