using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoseScreenController : MonoBehaviour
{
    [SerializeField] TMP_Text _txtReason;

    public void Show(string reason)
    {
        gameObject.SetActive(true);
        _txtReason.text = reason;
    }
}
