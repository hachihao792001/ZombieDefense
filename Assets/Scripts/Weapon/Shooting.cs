using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shooting : MonoBehaviourPun
{
    protected readonly int FireHash = Animator.StringToHash("Fire");
    protected readonly int FireStateHash = Animator.StringToHash("AlternateSingleFire");

    public UnityEvent OnShoot;

    public float Rpm;

    [HideInInspector]
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    private ParticleSystem _lightRay;
    [SerializeField]
    private AudioSource _fireSound;
    private void OnValidate() => _animator = GetComponent<Animator>();


    public virtual void Shoot()
    {
        if (!photonView.IsMine)
            return;
        PlayShootEffects();
        OnShoot?.Invoke();
    }

    protected void PlayShootEffects()
    {
        _lightRay.Emit(1);
        PlayFireSound();
    }

    public void PlayFireSound() => _fireSound.Play();

    public void Lock() => enabled = false;
    public void Unlock() => enabled = true;
}
