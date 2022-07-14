using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmmoRefiller : MonoBehaviour
{
    [SerializeField]
    private GunAmmo[] _gunAmmos;
    public Action OnAmmoRefill;

    public void RefillAmmo()
    {
        for (int i = 0; i < _gunAmmos.Length; i++)
        {
            _gunAmmos[i].RefillAmmo();
        }
        OnAmmoRefill?.Invoke();
    }
}
