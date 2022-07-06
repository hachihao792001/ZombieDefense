using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticZombieShooting : MonoBehaviour
{
    [SerializeField]
    private Transform aimingCamera;
    [HideInInspector]
    [SerializeField]
    private GunSwitcher _gunSwitcher;
    [SerializeField]
    private LayerMask _enemyMask;

    private float lastShotTime;

    private void OnValidate()
    {
        _gunSwitcher = GetComponent<GunSwitcher>();
    }

    private void Update()
    {
        float interval = 60f / _gunSwitcher.CurrentGun.Rpm;
        if (Time.time - lastShotTime >= interval)
        {
            Ray aimingRay = new Ray(aimingCamera.position, aimingCamera.forward);
            if (Physics.Raycast(aimingRay, out RaycastHit hitInfo, Mathf.Infinity, _enemyMask))
            {
                if (_gunSwitcher.CurrentGun.enabled)
                    _gunSwitcher.CurrentGun.Shoot();
            }
            lastShotTime = Time.time;
        }
    }
}
