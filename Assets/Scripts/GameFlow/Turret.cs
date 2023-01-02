using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Turret : MonoBehaviourPun
{
    private Zombie _target;
    [SerializeField]
    private Transform _head;
    [SerializeField]
    private float _range;
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _headRotateLerpRate;
    [SerializeField]
    private float _rpm;
    [SerializeField]
    private AudioSource _fireSound;
    [SerializeField]
    private LayerMask _notTurretLayer;
    [SerializeField]
    private LayerMask _zombieLayerMask;
    [SerializeField]
    private GameObject _bulletImpactPrefab;
    [SerializeField]
    private GameObject _zombieBloodPrefab;
    [SerializeField]
    private ParticleSystem _lightRay;

    public UnityEvent OnShoot;

    private void Start()
    {
        if (!photonView.IsMine)
            return;
        LookForTarget();
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;
        if (_target != null)
        {
            _head.forward = Vector3.Lerp(
                _head.forward,
                (_target.TurretTarget.position - _head.position).normalized,
                _headRotateLerpRate * Time.deltaTime);

            UpdateFiring();
            if (_target != null && Vector3.Distance(_head.position, _target.transform.position) > _range)
            {
                _target = null;
            }
        }
        else
        {
            LookForTarget();
        }
    }

    private float lastShotTime;
    private void UpdateFiring()
    {
        float interval = 60f / _rpm;
        if (Time.time - lastShotTime >= interval)
        {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    private void Shoot()
    {
        if (Physics.Raycast(_head.position, _head.forward, out RaycastHit hitInfo, Mathf.Infinity, _notTurretLayer))
        {
            if (hitInfo.transform == _target.transform)
            {
                PlayShootEffects();
                PhotonNetwork.RaiseEvent(GameController.TurretShootEventCode, PhotonNetwork.LocalPlayer.ActorNumber, RaiseEventOptions.Default, SendOptions.SendUnreliable);

                _target.ZombieHealth.TakeDamage(_damage);
                OnShoot?.Invoke();

                if (CheckLayerInsideLayerMask(hitInfo.collider.gameObject.layer, _zombieLayerMask))
                {
                    CreateHitEffect(_zombieBloodPrefab, hitInfo);
                }
                else
                {
                    CreateHitEffect(_bulletImpactPrefab, hitInfo);
                }
            }
        }
    }

    private void PlayShootEffects()
    {
        _fireSound.Play();
        _lightRay.Emit(1);
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
        if (obj.Code == GameController.TurretShootEventCode)
        {
            int actorNumber = (int)obj.CustomData;
            if (photonView.OwnerActorNr == actorNumber && actorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                PlayShootEffects();
        }
    }

    public void OnTargetDied(Zombie zombie)
    {
        if (_target != null && _target == zombie)
        {
            _target = null;
            LookForTarget();
        }
    }

    private void LookForTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(_head.position, _range, _zombieLayerMask);
        if (colliders.Length > 0)
        {
            Zombie foundZombie = null;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (!colliders[i].CompareTag("Head"))
                {
                    foundZombie = colliders[i].gameObject.GetComponent<Zombie>();
                    break;
                }
            }
            if (foundZombie != null)
            {
                _target = foundZombie;
                if (_target != null)
                    _target.OnZombieDied += OnTargetDied;
            }
        }
    }

    private void CreateHitEffect(GameObject impactEffect, RaycastHit hitInfo)
    {
        Quaternion holeRotation = Quaternion.LookRotation(hitInfo.normal);
        Instantiate(impactEffect, hitInfo.point, holeRotation).transform.parent = hitInfo.collider.transform;
    }

    bool CheckLayerInsideLayerMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }
}
