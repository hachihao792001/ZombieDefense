using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject[] guns;

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
    }

    public void SwitchToGun(int gunIndex)
    {
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].SetActive(i == gunIndex);
        }
    }

    public void HideAllGuns()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].SetActive(false);
        }
    }
}