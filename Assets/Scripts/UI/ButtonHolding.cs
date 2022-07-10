using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHolding : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool _isHolding = false;
    public bool IsHolding => _isHolding;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isHolding = false;
    }
}
