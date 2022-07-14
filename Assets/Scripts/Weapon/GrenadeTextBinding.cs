using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GrenadeTextBinding : MonoBehaviour
{
    private GrenadeThrowing _grenadeThrowing;
    [HideInInspector]
    [SerializeField]
    private TMP_Text _grenadeText;


    void Start()
    {
        _grenadeThrowing = GameController.Instance.Player.GrenadeThrowing;
        _grenadeThrowing.OnGrenadeCountChanged += OnGrenadeCountChanged;
        OnGrenadeCountChanged();
    }

    private void OnGrenadeCountChanged() => _grenadeText.text = $"{_grenadeThrowing.GrenadeLeft}/{_grenadeThrowing.StartGrenadeCount}";
}
