using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FPSDragArea : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Action<Vector2> onBeginDrag;
    public Action<Vector2> onDrag;
    public Action onEndDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        onBeginDrag?.Invoke(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        onDrag?.Invoke(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onEndDrag?.Invoke();
    }
}
