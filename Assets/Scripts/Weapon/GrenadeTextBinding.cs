using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GrenadeTextBinding : MonoBehaviour
{
    [SerializeField]
    private GrenadeThrowing _grenadeThrowing;
    [SerializeField]
    private TMP_Text _grenadeText;

    void Start()
    {
        _grenadeThrowing.OnGrenadeCountChanged += OnGrenadeCountChanged;
        OnGrenadeCountChanged();
    }

    private void OnDestroy()
    {
        _grenadeThrowing.OnGrenadeCountChanged -= OnGrenadeCountChanged;
    }

    private void OnGrenadeCountChanged() => _grenadeText.text = $"{_grenadeThrowing.GrenadeLeft}/{_grenadeThrowing.StartGrenadeCount}";
}
