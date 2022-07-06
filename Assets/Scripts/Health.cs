using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    private readonly int DeadHash = Animator.StringToHash("Dead");

    [SerializeField]
    private float _fullHealth;
    [SerializeField]
    private float _currentHealth;

    [HideInInspector]
    [SerializeField]
    private Animator _animator;

    public UnityEvent<Health> OnDied;
    public UnityEvent<float> OnHealthChanged;

    private void OnValidate() => _animator = GetComponent<Animator>();

    private void Start()
    {
        _currentHealth = _fullHealth;
    }

    public void TakeDamage(float _damage)
    {
        _currentHealth -= _damage;
        OnHealthChanged?.Invoke(_currentHealth / _fullHealth);
        if (_currentHealth <= 0)
        {
            if (_animator != null)
                _animator.SetTrigger(DeadHash);
            _currentHealth = 0;
            OnDied?.Invoke(this);
        }
    }
}
