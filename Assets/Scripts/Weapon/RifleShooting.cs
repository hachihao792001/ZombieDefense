using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShooting : Shooting
{
    private bool _isFiring;
    private float lastShotTime;

    void Update()
    {
        if (!photonView.IsMine)
            return;
        if (_isFiring)
        {
            UpdateFiring();
        }
    }

    private void UpdateFiring()
    {
        float interval = 60f / Rpm;
        if (Time.time - lastShotTime >= interval)
        {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    public override void Shoot()
    {
        if (!photonView.IsMine)
            return;

        _animator.Play(FireStateHash, layer: 0, normalizedTime: 0);
        PhotonNetwork.RaiseEvent(GameController.FireStateEventCode, PhotonNetwork.LocalPlayer.ActorNumber, RaiseEventOptions.Default, SendOptions.SendUnreliable);

        base.Shoot();
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }
    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == GameController.FireStateEventCode)
        {
            int actorNumber = (int)obj.CustomData;
            if (photonView.OwnerActorNr == actorNumber && actorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                _animator.Play(FireStateHash, layer: 0, normalizedTime: 0);
        }
    }

}