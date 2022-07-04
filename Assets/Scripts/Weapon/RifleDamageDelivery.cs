using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleDamageDelivery : MonoBehaviour
{
    [SerializeField]
    private Transform aimingCamera;
    [SerializeField]
    private GameObject hitEffectPrefab;
    [SerializeField]
    private float bulletForce;
    [SerializeField]
    private int _damage;

    public void OnShoot()
    {
        PerformRaycasting();
    }

    private void PerformRaycasting()
    {
        Ray aimingRay = new Ray(aimingCamera.position, aimingCamera.forward);
        if (Physics.Raycast(aimingRay, out RaycastHit hitInfo))
        {
            //Health health = hitInfo.collider.gameObject.GetComponentInParent<Health>();
            //if (health != null)
            //{
            //    health.TakeDamage(_damage);
            //}

            CreateHitEffect(hitInfo);

            if (hitInfo.rigidbody)
                AddForceToHit(hitInfo);
        }
    }

    private void CreateHitEffect(RaycastHit hitInfo)
    {
        Quaternion holeRotation = Quaternion.LookRotation(hitInfo.normal);
        Instantiate(hitEffectPrefab, hitInfo.point, holeRotation).transform.parent = hitInfo.collider.transform;
    }
    private void AddForceToHit(RaycastHit hitInfo)
    {
        hitInfo.rigidbody.AddForce(-hitInfo.normal * bulletForce);
    }

}
