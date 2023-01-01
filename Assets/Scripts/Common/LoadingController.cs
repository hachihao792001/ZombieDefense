using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingController : OneSceneMonoSingleton<LoadingController>
{
    public GameObject popup;
    public TMP_Text loadingText;

    public void ShowLoading(string text)
    {
        loadingText.text = text;
        popup.gameObject.SetActive(true);
    }

    public void HideLoading()
    {
        popup.gameObject.SetActive(false);
    }
}
