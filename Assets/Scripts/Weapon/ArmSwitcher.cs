using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmSwitcher : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    private ArmController[] arms;

    private int _currentArmIndex = 0;
    public ArmController CurrentArm => arms[_currentArmIndex];

    private void Update()
    {
        if (!photonView.IsMine)
            return;

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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            for (int i = 0; i < arms.Length; i++)
                stream.SendNext(arms[i].gameObject.activeSelf);
        }
        else if (stream.IsReading)
        {
            for (int i = 0; i < arms.Length; i++)
            {
                bool isArmActive = (bool)stream.ReceiveNext();
                arms[i].gameObject.SetActive(isArmActive);
            }
        }
    }
}