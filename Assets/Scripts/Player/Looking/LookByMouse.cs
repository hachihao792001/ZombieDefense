using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookByMouse : PlayerLooking
{
    private float _yaw, _pitch;

    protected override void Start()
    {
        base.Start();
        GameController.Instance.LockCursor();
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;
        if (GameController.Instance.IsPaused || GameController.IsGameOver) return;
        UpdateMouse();
    }

    private void UpdateMouse()
    {
        _yaw += Input.GetAxis("Mouse X") * _sensitivity;
        _pitch -= Input.GetAxis("Mouse Y") * _sensitivity;

        transform.eulerAngles = new Vector3(0, _yaw, 0);
        _cameraAndWeapon.localEulerAngles = new Vector3(_pitch, 0, 0);

        ClampPitchAngle();
        _pitch = _cameraAndWeapon.localEulerAngles.x;
    }
}
