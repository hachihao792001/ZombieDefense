using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LookByDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    Transform _player;
    [SerializeField]
    Transform _cameraAndWeapon;

    [SerializeField]
    private float _sensitivity = 2;
    [SerializeField]
    private float _minSensitivity = 3;
    [SerializeField]
    private float _maxSensitivity = 15;

    private float _lastYaw, _lastPitch;
    [SerializeField]
    private float _minPitch;
    [SerializeField]
    private float _maxPitch;

    private Vector2 _startDraggingPos;

    private void Start()
    {
        GameController.Instance.OnSensitivitySliderChanged =
            (float value) => _sensitivity = value * (_maxSensitivity - _minSensitivity) + _minSensitivity;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startDraggingPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 deltaDragPosition = (eventData.position - _startDraggingPos) / Screen.dpi;
        deltaDragPosition.y = -deltaDragPosition.y;

        _player.eulerAngles = new Vector3(0, _lastYaw + deltaDragPosition.x * _sensitivity, 0);
        _cameraAndWeapon.localEulerAngles = new Vector3(_lastPitch + deltaDragPosition.y * _sensitivity, 0, 0);

        ClampPitchAngle();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _lastYaw = _cameraAndWeapon.eulerAngles.y;
        _lastPitch = _cameraAndWeapon.eulerAngles.x;
    }

    private void ClampPitchAngle()
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
