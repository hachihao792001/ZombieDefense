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
    private float _lastYaw, _lastPitch;
    [SerializeField]
    private float _minPitch;
    [SerializeField]
    private float _maxPitch;

    private Vector2 _startDraggingPos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startDraggingPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 deltaDragPosition = (eventData.position - _startDraggingPos) / Screen.dpi;
        deltaDragPosition.x = -deltaDragPosition.x;

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