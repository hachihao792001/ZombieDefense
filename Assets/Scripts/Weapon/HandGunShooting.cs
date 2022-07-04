using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGunShooting : Shooting
{
    [SerializeField]
    private Transform _weaponCamera;
    [SerializeField]
    private GameObject _bulletImpactPrefab;

    protected override void Shoot()
    {
        _animator.Play(FireHash, 0, 0);

        PlayFireSound();

        if (Physics.Raycast(_weaponCamera.position, _weaponCamera.forward, out RaycastHit hitInfo))
        {
            Quaternion bulletImpactRotation = Quaternion.LookRotation(hitInfo.normal);
            GameObject newBulletImpact = Instantiate(_bulletImpactPrefab, hitInfo.point, bulletImpactRotation);
            newBulletImpact.transform.parent = hitInfo.transform;
        }

        base.Shoot();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
}
