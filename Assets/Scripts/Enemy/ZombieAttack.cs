using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class ZombieAttack : MonoBehaviour
{
    private readonly int AttackingHash = Animator.StringToHash("Attacking");

    [HideInInspector]
    [SerializeField]
    private Animator _animator;
    [HideInInspector]
    [SerializeField]
    private ZombieMoving _zombieMoving;

    private Health _targetHealth;

    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _attackDistance;
    [SerializeField]
    private float _lookAtTargetWhileAttackingLerpRate;

    private void OnValidate()
    {
        _animator = GetComponent<Animator>();
        _zombieMoving = GetComponent<ZombieMoving>();
    }

    private void Update()
    {
        if (_zombieMoving.Target != null)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, _attackDistance))
            {
                if (hitInfo.transform == _zombieMoving.Target)
                    _animator.SetBool(AttackingHash, true);
            }
            else
            {
                _animator.SetBool(AttackingHash, false);
            }

            if (Vector3.Distance(transform.position, _zombieMoving.Target.position) <= _attackDistance * 2)
            {
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.LookRotation(_zombieMoving.Target.position - transform.position),
                    _lookAtTargetWhileAttackingLerpRate * Time.deltaTime);
            }

            if (_targetHealth == null || _targetHealth.transform != _zombieMoving.Target)
                _targetHealth = _zombieMoving.Target.GetComponent<Health>();
        }
        else
        {
            _animator.SetBool(AttackingHash, false);
            _targetHealth = null;
        }
    }

    public void OnDied()
    {
        _animator.SetBool(AttackingHash, false);
        enabled = false;
    }

    public void DealDamage()
    {
        if (_targetHealth != null)
        {
            _targetHealth.TakeDamage(_damage);
        }
        else
        {
            Health parentHealth = _zombieMoving.Target.GetComponentInParent<Health>();
            if (parentHealth != null)
                parentHealth.TakeDamage(_damage);
        }
    }
}
