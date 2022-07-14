using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _damageRadius;
    [SerializeField]
    private float _secondsUntilExplode;
    [SerializeField]
    private GameObject _explosionEffect;
    [SerializeField]
    private LayerMask _zombieLayerMask;
    [HideInInspector]
    [SerializeField]
    private Rigidbody _rb;
    [HideInInspector]
    [SerializeField]
    private MeshRenderer _meshRenderer;

    private void OnValidate()
    {
        _rb = GetComponent<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        StartCoroutine(countDownToExplode());
    }
    IEnumerator countDownToExplode()
    {
        yield return new WaitForSeconds(_secondsUntilExplode);
        explode();
    }

    void explode()
    {
        _explosionEffect.SetActive(true);
        _meshRenderer.enabled = false;
        Collider[] hitZombieColliders = Physics.OverlapSphere(transform.position, _damageRadius, _zombieLayerMask);
        for (int i = 0; i < hitZombieColliders.Length; i++)
        {
            if (hitZombieColliders[i].TryGetComponent(out Zombie zombie))
            {
                zombie.ZombieHealth.TakeDamage(_damage);
            }
        }

        Destroy(gameObject, 2f);
    }

    public void AddForce(Vector3 force)
    {
        _rb.AddForce(force);
    }
}
