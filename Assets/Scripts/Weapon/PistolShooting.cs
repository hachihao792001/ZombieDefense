using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShooting : Shooting
{
    [SerializeField]
    private Transform _weaponCamera;

    public override void Shoot()
    {
        if (!photonView.IsMine)
            return;

        _animator.Play(FireHash, layer: 0, normalizedTime: 0);
        PhotonNetwork.RaiseEvent(FireEventCode, PhotonNetwork.LocalPlayer.ActorNumber, RaiseEventOptions.Default, SendOptions.SendUnreliable);

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
        if (obj.Code == FireEventCode)
        {
            int actorNumber = (int)obj.CustomData;
            if (photonView.OwnerActorNr == actorNumber && actorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                _animator.Play(FireHash, layer: 0, normalizedTime: 0);
        }
    }
}
