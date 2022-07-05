using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShooting : Shooting
{
    private readonly int FireStateHash = Animator.StringToHash("AlternateSingleFire");

    [SerializeField]
    private float _rpm;

    private bool _isFiring;
    private float lastShotTime;

    void Update()
    {
        _isFiring = Input.GetButton("Fire1");

        if (_isFiring)
        {
            UpdateFiring();
        }
    }

    private void UpdateFiring()
    {
        float interval = 60f / _rpm;
        if (Time.time - lastShotTime >= interval)
        {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    protected override void Shoot()
    {
        _animator.Play(FireStateHash, layer: 0, normalizedTime: 0);
        PlayFireSound();
        //PerformRaycasting();
        base.Shoot();
    }


}