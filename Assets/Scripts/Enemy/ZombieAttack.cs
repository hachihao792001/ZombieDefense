using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    private readonly int AttackingHash = Animator.StringToHash("Attacking");

    [HideInInspector]
    [SerializeField]
    private Animator _animator;
    [HideInInspector]
    [SerializeField]
    private ZombieMoving _zombieMoving;
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
            if (Vector3.Distance(transform.position, _zombieMoving.Target.position) <= _attackDistance)
            {
                _animator.SetBool(AttackingHash, true);
            }
            else
            {
                _animator.SetBool(AttackingHash, false);
            }

            if (_animator.GetBool(AttackingHash))
            {
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.LookRotation(_zombieMoving.Target.position - transform.position),
                    _lookAtTargetWhileAttackingLerpRate * Time.deltaTime);
            }
        }
        else
        {
            _animator.SetBool(AttackingHash, false);
        }
    }

    public void OnDied()
    {
        _animator.SetBool(AttackingHash, false);
        enabled = false;
    }
}
