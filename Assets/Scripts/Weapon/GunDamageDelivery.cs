using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class GunDamageDelivery : MonoBehaviour
{
    [SerializeField]
    private Transform aimingCamera;
    [SerializeField]
    private GameObject _bulletImpactPrefab;
    [SerializeField]
    private GameObject _zombieBloodPrefab;
    [SerializeField]
    private float bulletForce;
    [SerializeField]
    private int _damage;
    [HideInInspector]
    [SerializeField]
    private int _zombieLayer;

    private void OnValidate() => _zombieLayer = LayerMask.NameToLayer("Enemy");

    public void OnShoot()
    {
        PerformRaycasting();
    }

    private void PerformRaycasting()
    {
        Ray aimingRay = new Ray(aimingCamera.position, aimingCamera.forward);
        if (Physics.Raycast(aimingRay, out RaycastHit hitInfo))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            Health health = hitObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(_damage);
            }
            else if (hitObject.tag == "Head")
            {
                hitObject.GetComponentInParent<Health>().TakeDamage(_damage * 2);
            }

            if (hitObject.layer == _zombieLayer)
            {
                CreateHitEffect(_zombieBloodPrefab, hitInfo);
            }
            else
            {
                CreateHitEffect(_bulletImpactPrefab, hitInfo);
            }

            if (hitInfo.rigidbody)
                AddForceToHit(hitInfo);
        }
    }

    private void CreateHitEffect(GameObject impactEffect, RaycastHit hitInfo)
    {
        Quaternion holeRotation = Quaternion.LookRotation(hitInfo.normal);
        if (impactEffect == _bulletImpactPrefab)
            LeanPool.Spawn(impactEffect, hitInfo.point, holeRotation).transform.parent = hitInfo.collider.transform;
        else
            Instantiate(impactEffect, hitInfo.point, holeRotation).transform.parent = hitInfo.collider.transform;
    }
    private void AddForceToHit(RaycastHit hitInfo)
    {
        hitInfo.rigidbody.AddForce(-hitInfo.normal * bulletForce);
    }

}
