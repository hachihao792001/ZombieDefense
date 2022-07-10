using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSwitcher : MonoBehaviour
{
    [SerializeField]
    private Shooting[] guns;

    private int _currentGunIndex = 0;
    public Shooting CurrentGun => guns[_currentGunIndex];

    private void Update()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SwitchToGun(i);
                break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
            SwitchGunOnClick();
    }

    public void SwitchToGun(int gunIndex)
    {
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].gameObject.SetActive(i == gunIndex);
        }
        _currentGunIndex = gunIndex;
    }

    public void SwitchGunOnClick()
    {
        SwitchToGun(guns.Length - 1 - _currentGunIndex);
    }
}