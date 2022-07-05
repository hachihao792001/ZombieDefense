using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShooting : Shooting
{
    [SerializeField]
    private Transform _weaponCamera;

    protected override void Shoot()
    {
        _animator.Play(FireHash, 0, 0);

        PlayFireSound();

        base.Shoot();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
}
