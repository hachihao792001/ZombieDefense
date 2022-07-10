using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookByMouse : PlayerLooking
{
    private new void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        UpdateMouse();
    }

    private void UpdateMouse()
    {
        _yaw += Input.GetAxis("Mouse X") * _sensitivity;
        _pitch -= Input.GetAxis("Mouse Y") * _sensitivity;

        transform.eulerAngles = new Vector3(0, _yaw, 0);
        _cameraAndWeapon.localEulerAngles = new Vector3(_pitch, 0, 0);

        ClampPitchAngle();
    }
}
