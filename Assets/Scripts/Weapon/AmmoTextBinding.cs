using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoTextBinding : MonoBehaviour
{
    [SerializeField]
    private GunAmmo gunAmmo;
    [HideInInspector]
    [SerializeField]
    private TMP_Text ammoText;

    private void OnValidate() => ammoText = GetComponent<TMP_Text>();

    void Start()
    {
        gunAmmo.OnAmmoChanged += UpdateAmmo;
        UpdateAmmo();
    }

    private void UpdateAmmo() => ammoText.text = $"{gunAmmo.LoadedAmmo}/{gunAmmo.RemainingAmmo}";
}
