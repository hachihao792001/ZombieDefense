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
    private int _zombieLayer;

    private float lastShotTime;

    private void OnValidate()
    {
        _gunSwitcher = GetComponent<GunSwitcher>();
        _zombieLayer = LayerMask.NameToLayer("Enemy");
    }

    private void Update()
    {
        float interval = 60f / _gunSwitcher.CurrentGun.Rpm;
        if (Time.time - lastShotTime >= interval)
        {
            Ray aimingRay = new Ray(aimingCamera.position, aimingCamera.forward);
            if (Physics.Raycast(aimingRay, out RaycastHit hitInfo))
            {
                if (_gunSwitcher.CurrentGun.enabled && hitInfo.transform.gameObject.layer == _zombieLayer)
                    _gunSwitcher.CurrentGun.Shoot();
            }
            lastShotTime = Time.time;
        }
    }
}
