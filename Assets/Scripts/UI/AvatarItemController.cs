using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarItemController : MonoBehaviour
{
    int index;
    Action<int> onClick;
    public void Init(int index, bool isCurrent, Action<int> onClick)
    {
        this.index = index;
        this.onClick = onClick;
    }

    public void OnClick()
    {
        onClick?.Invoke(index);
    }
}
