using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLooking : MonoBehaviourPun
{
    [SerializeField]
    protected Transform _cameraAndWeapon;
    [SerializeField]
    protected float _sensitivity = 2;
    [SerializeField]
    private float _minSensitivity = 3;
    [SerializeField]
    private float _maxSensitivity = 15;

    [SerializeField]
    protected float _minPitch = -60;
    [SerializeField]
    protected float _maxPitch = 60;

    protected virtual void Start()
    {
        GameController.Instance.OnSensitivitySliderChanged =
           (float value) => _sensitivity = value * (_maxSensitivity - _minSensitivity) + _minSensitivity;
    }

    protected void ClampPitchAngle()
    {
        float pitch = _cameraAndWeapon.localEulerAngles.x;
        while (pitch > 180)
        {
            pitch -= 360;
        }
        while (pitch < -180)
        {
            pitch += 360;
        }

        pitch = Mathf.Clamp(pitch, _minPitch, _maxPitch);
        _cameraAndWeapon.localEulerAngles = new Vector3(pitch, 0, 0);
    }
}
