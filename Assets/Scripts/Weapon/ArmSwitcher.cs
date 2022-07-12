using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmSwitcher : MonoBehaviour
{
    [SerializeField]
    private ArmController[] arms;

    private int _currentArmIndex = 0;
    public ArmController CurrentArm => arms[_currentArmIndex];

    private void Update()
    {
        for (int i = 0; i < arms.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SwitchToArm(i);
                break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
            SwitchGunOnClick();
    }

    private void SwitchToArm(int gunIndex)
    {
        for (int i = 0; i < arms.Length; i++)
        {
            arms[i].gameObject.SetActive(i == gunIndex);
        }
        _currentArmIndex = gunIndex;
    }

    public void SwitchGunOnClick()
    {
        SwitchToArm(arms.Length - 1 - _currentArmIndex);
    }
}