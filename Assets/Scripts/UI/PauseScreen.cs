using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        Time.timeScale = 1;
        SceneManager.LoadScene("Home");
        gameObject.SetActive(false);
    }

    public void OnSensititySliderChanged(float value)
    {
        OnSensititySliderChangedAction?.Invoke(value);
    }
}
