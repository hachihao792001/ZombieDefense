using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAmmo : MonoBehaviourPun
{
    private readonly int ReloadTriggerHash = Animator.StringToHash("Reload");
    private readonly byte ReloadEventCode = 2;

    [SerializeField]
    public int FullRemainingAmmo;
    [field: SerializeField]
    public int RemainingAmmo { get; private set; }

    public Action OnAmmoChanged;

    [HideInInspector]
    [SerializeField]
    private Animator _animator;
    [HideInInspector]
    [SerializeField]
    private ArmController _arm;

    [SerializeField]
    private AudioSource _reloadSound;

    [SerializeField]
    private int _magazineSize;
    private bool _isReloading;
    private int _loadedAmmo;
    public int LoadedAmmo
    {
        get => _loadedAmmo;
        set
        {
            _loadedAmmo = value;
            if (LoadedAmmo <= 0)
            {
                LockShooting();
            }
            OnAmmoChanged?.Invoke();
        }
    }

    private void OnValidate()
    {
        _animator = GetComponent<Animator>();
        _arm = GetComponent<ArmController>();
    }

    private void OnEnable()
    {
        _isReloading = false;
        UnlockAttacking();

        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void Start()
    {
        LoadedAmmo = _magazineSize;
        RemainingAmmo = FullRemainingAmmo;
        _arm.Shooting.OnShoot.AddListener(OnShoot);
    }
    private void OnShoot()
    {
        LoadedAmmo--;
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;
        if (_isReloading) return;
        if (LoadedAmmo < _magazineSize && (Input.GetKeyDown(KeyCode.R) || LoadedAmmo <= 0))
        {
            Reload();
        }
    }

    public void ReloadOnClick()
    {
        if (gameObject.activeSelf && !_isReloading)
            Reload();
    }

    private void Reload()
    {
        if (RemainingAmmo > 0 && LoadedAmmo < _magazineSize)
        {
            _animator.SetTrigger(ReloadTriggerHash);
            PhotonNetwork.RaiseEvent(ReloadEventCode, PhotonNetwork.LocalPlayer.ActorNumber, RaiseEventOptions.Default, SendOptions.SendUnreliable);

            _isReloading = true;
            _reloadSound.Play();
            LockAttacking();
        }
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }
    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == ReloadEventCode)
        {
            int actorNumber = (int)obj.CustomData;
            if (photonView.OwnerActorNr == actorNumber && actorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                _animator.SetTrigger(ReloadTriggerHash);
        }
    }

    public void AddAmmo()
    {
        int requiredAmmo = _magazineSize - LoadedAmmo;
        int addedAmmo = Mathf.Min(requiredAmmo, RemainingAmmo);
        RemainingAmmo -= addedAmmo;
        LoadedAmmo += addedAmmo;
    }

    public void DoneReloading()
    {
        _isReloading = false;
        UnlockAttacking();
    }

    private void LockShooting()
    {
        _arm.Shooting.Lock();
    }

    private void LockAttacking()
    {
        _arm.Shooting.Lock();
        _arm.Meleeing.Lock();
    }
    private void UnlockAttacking()
    {
        if (LoadedAmmo > 0)
        {
            _arm.Shooting.Unlock();
        }
        _arm.Meleeing.Unlock();
    }

    public void RefillAmmo()
    {
        RemainingAmmo = FullRemainingAmmo;
        LoadedAmmo = _magazineSize;
    }
}
