using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShooting : Shooting
{
    private readonly int FireStateHash = Animator.StringToHash("AlternateSingleFire");

    private bool _isFiring;
    private float lastShotTime;

    void Update()
    {
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
        _animator.Play(FireStateHash, layer: 0, normalizedTime: 0);
        base.Shoot();
    }
}