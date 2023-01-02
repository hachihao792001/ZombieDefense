using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviourPun
{
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _damageRadius;
    [SerializeField]
    private float _secondsUntilExplode;
    [SerializeField]
    private GameObject _explosionEffect;
    [SerializeField]
    private LayerMask _zombieLayerMask;
    [HideInInspector]
    [SerializeField]
    private Rigidbody _rb;
    [HideInInspector]
    [SerializeField]
    private MeshRenderer _meshRenderer;

    private void OnValidate()
    {
        _rb = GetComponent<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
            explode();
    }

    void explode()
    {
        PlayExplosionEffect();
        PhotonNetwork.RaiseEvent(GameController.GrenadeExplodeEventCode, PhotonNetwork.LocalPlayer.ActorNumber, RaiseEventOptions.Default, SendOptions.SendUnreliable);

        _meshRenderer.enabled = false;
        _rb.isKinematic = true;
        Collider[] hitZombieColliders = Physics.OverlapSphere(transform.position, _damageRadius, _zombieLayerMask);
        for (int i = 0; i < hitZombieColliders.Length; i++)
        {
            if (hitZombieColliders[i].TryGetComponent(out Zombie zombie))
            {
                zombie.ZombieHealth.TakeDamage(_damage);
            }
        }

        StartCoroutine(destroyAfterSeconds(3f));
    }
    IEnumerator destroyAfterSeconds(float sec)
    {
        yield return new WaitForSeconds(sec);
        PhotonHelper.DestroyNetworkObject(gameObject);
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
        if (obj.Code == GameController.GrenadeExplodeEventCode)
        {
            int actorNumber = (int)obj.CustomData;
            if (photonView.OwnerActorNr == actorNumber && actorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                PlayExplosionEffect();
        }
    }

    private void PlayExplosionEffect()
    {
        _explosionEffect.SetActive(true);
    }

    public void AddForce(Vector3 force)
    {
        _rb.AddForce(force);
    }
}
