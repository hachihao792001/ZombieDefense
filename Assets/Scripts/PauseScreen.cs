using System;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public Action<float> OnSensititySliderChangedAction;

    public void ResumeOnClick()
    {
        GameController.Instance.ResumeGame();
        gameObject.SetActive(false);
    }
    public void MenuOnClick()
    {

    }

    public void OnSensititySliderChanged(float value)
    {
        OnSensititySliderChangedAction?.Invoke(value);
    }
}
