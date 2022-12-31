using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupChangeName : MonoBehaviour
{
    public TMP_InputField inputField;
    public Action<string> OnApplyChange;

    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void ApplyChange()
    {
        OnApplyChange?.Invoke(inputField.text);
    }

    public void Init(string value)
    {
        inputField.text = value;
    }
}
