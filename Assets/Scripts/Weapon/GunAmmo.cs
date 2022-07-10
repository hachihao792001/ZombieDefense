using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAmmo : MonoBehaviour
{
    private readonly int ReloadTriggerHash = Animator.StringToHash("Reload");

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
    private Shooting _shooting;

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
        _shooting = GetComponent<Shooting>();
    }

    private void OnEnable()
    {
        _isReloading = false;
        UnlockShooting();
    }

    private void Start()
    {
        LoadedAmmo = _magazineSize;
        RemainingAmmo = FullRemainingAmmo;
        _shooting.OnShoot.AddListener(OnShoot);
    }
    private void OnShoot()
    {
        LoadedAmmo--;
    }

    private void Update()
    {
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
        if (RemainingAmmo > 0)
        {
            _animator.SetTrigger(ReloadTriggerHash);
            _isReloading = true;
            _reloadSound.Play();
            LockShooting();
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
        UnlockShooting();
    }

    private void LockShooting() => _shooting.Lock();
    private void UnlockShooting()
    {
        if (LoadedAmmo > 0)
        {
            _shooting.Unlock();
        }
    }

    public void RefillAmmo()
    {
        RemainingAmmo = FullRemainingAmmo;
        LoadedAmmo = _magazineSize;
    }
}
