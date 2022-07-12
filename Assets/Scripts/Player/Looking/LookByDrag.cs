using System.Collections;
using UnityEngine;
using DG.Tweening;

public class LookByDrag : PlayerLooking
{
    [SerializeField]
    FPSDragArea _fPSDragArea;

    private float _lastYaw, _lastPitch;

    private Vector2 _startDraggingPos;

    private new void Start()
    {
        _fPSDragArea.onBeginDrag = OnBeginDrag;
        _fPSDragArea.onDrag = OnDrag;
        _fPSDragArea.onEndDrag = OnEndDrag;

        base.Start();
    }

    public void OnBeginDrag(Vector2 pos)
    {
        _startDraggingPos = pos;
    }

    public void OnDrag(Vector2 pos)
    {
        Vector2 deltaDragPosition = (pos - _startDraggingPos) / Screen.dpi;
        deltaDragPosition.y = -deltaDragPosition.y;

        transform.eulerAngles = new Vector3(0, _lastYaw + deltaDragPosition.x * _sensitivity, 0);
        _cameraAndWeapon.localEulerAngles = new Vector3(_lastPitch + deltaDragPosition.y * _sensitivity, 0, 0);

        ClampPitchAngle();
    }

    public void OnEndDrag()
    {
        _lastYaw = _cameraAndWeapon.eulerAngles.y;
        _lastPitch = _cameraAndWeapon.eulerAngles.x;
    }

    public void TurnAroundOnClick()
    {
        StopCoroutine(TurnAround());
        StartCoroutine(TurnAround());
    }

    private IEnumerator TurnAround()
    {
        _lastYaw = _cameraAndWeapon.eulerAngles.y + 180;
        yield return _cameraAndWeapon.DORotate(_cameraAndWeapon.eulerAngles + Vector3.up * 180, 0.2f);
        ClampPitchAngle();
    }
}
