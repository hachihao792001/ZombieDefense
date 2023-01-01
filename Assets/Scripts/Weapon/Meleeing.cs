using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meleeing : MonoBehaviourPun
{
    private readonly int MeleeHash = Animator.StringToHash("Melee");
    private readonly byte MeleeEventCode = 3;

    public float Distance;
    public int Rpm;

    [SerializeField]
    private Transform aimingCamera;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private GameObject _zombieBloodPrefab;
    [SerializeField]
    private int _damage;
    [HideInInspector]
    [SerializeField]
    private int _zombieLayer;

    private void OnValidate()
    {
        _zombieLayer = LayerMask.NameToLayer("Enemy");
        _animator = GetComponent<Animator>();
    }

    public void DoMelee()
    {
        _animator.Play(MeleeHash, 0, 0);
        PhotonNetwork.RaiseEvent(MeleeEventCode, PhotonNetwork.LocalPlayer.ActorNumber, RaiseEventOptions.Default, SendOptions.SendUnreliable);
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
        if (obj.Code == MeleeEventCode)
        {
            int actorNumber = (int)obj.CustomData;
            if (photonView.OwnerActorNr == actorNumber && actorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                _animator.Play(MeleeHash, 0, 0);
        }
    }

    public void MeleeDealDamage()   //animation event
    {
        if (!photonView.IsMine)
            return;
        PerformRaycasting();
    }

    private void PerformRaycasting()
    {
        Ray aimingRay = new Ray(aimingCamera.position, aimingCamera.forward);
        if (Physics.Raycast(aimingRay, out RaycastHit hitInfo, Distance))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            Health health = hitObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(_damage);
            }
            else if (hitObject.tag == "Head")
            {
                hitObject.GetComponentInParent<Health>().TakeDamage(_damage * 2);
            }

            if (hitObject.layer == _zombieLayer)
            {
                CreateHitEffect(_zombieBloodPrefab, hitInfo);
            }
        }
    }

    private void CreateHitEffect(GameObject impactEffect, RaycastHit hitInfo)
    {
        Quaternion holeRotation = Quaternion.LookRotation(hitInfo.normal);
        Instantiate(impactEffect, hitInfo.point, holeRotation).transform.parent = hitInfo.collider.transform;
    }

    public void Lock() => enabled = false;
    public void Unlock() => enabled = true;
}
