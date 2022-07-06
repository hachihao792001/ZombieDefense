using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Zombie _target;
    [SerializeField]
    private Transform _head;
    [SerializeField]
    private float _range;
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _rpm;

    [SerializeField]
    private AudioSource _fireSound;

    [SerializeField]
    private LayerMask _notTurretLayer;
    [SerializeField]
    private LayerMask _zombieLayer;

    [SerializeField]
    private ParticleSystem _lightRay;

    private void Start()
    {
        LookForTarget();
    }

    private void Update()
    {
        if (_target != null)
        {
            _head.LookAt(_target.transform);
            UpdateFiring();
            if (_target != null && Vector3.Distance(_head.position, _target.transform.position) > _range)
            {
                _target = null;
            }
        }
        else
        {
            LookForTarget();
        }
    }

    private float lastShotTime;
    private void UpdateFiring()
    {
        float interval = 60f / _rpm;
        if (Time.time - lastShotTime >= interval)
        {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    private void Shoot()
    {
        if (Physics.Raycast(_head.position, _head.forward, out RaycastHit hitInfo, Mathf.Infinity, _notTurretLayer))
        {
            if (hitInfo.transform == _target.transform)
            {
                _fireSound.Play();
                _target.ZombieHealth.TakeDamage(_damage);
                _lightRay.Emit(1);
            }
        }
    }

    public void OnTargetDied(Zombie zombie)
    {
        if (_target != null && _target == zombie)
        {
            _target = null;
            LookForTarget();
        }
    }

    private void LookForTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(_head.position, _range, _zombieLayer);
        if (colliders.Length > 0)
        {
            Zombie foundZombie = null;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (!colliders[i].CompareTag("Head"))
                {
                    foundZombie = colliders[i].gameObject.GetComponent<Zombie>();
                    break;
                }
            }
            if (foundZombie != null)
            {
                _target = foundZombie;
                if (_target != null)
                    _target.OnZombieDied += OnTargetDied;
            }
        }
    }
}
