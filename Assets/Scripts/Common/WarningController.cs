using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WarningController : OneSceneMonoSingleton<WarningController>
{
    public GameObject popup;
    public TMP_Text warningText;

    public void ShowWarning(string text)
    {
        warningText.text = text;
        popup.gameObject.SetActive(true);
        StartCoroutine(HideWarningAfterDelay(3));
    }

    public void HideWarning()
    {
        popup.gameObject.SetActive(false);
    }

    IEnumerator HideWarningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideWarning();
    }
}
