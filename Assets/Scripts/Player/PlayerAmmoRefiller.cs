using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmmoRefiller : MonoBehaviour
{
    [SerializeField]
    private GunAmmo[] _gunAmmos;

    public void RefillAmmo()
    {
        for (int i = 0; i < _gunAmmos.Length; i++)
        {
            _gunAmmos[i].RefillAmmo();
        }
    }
}
