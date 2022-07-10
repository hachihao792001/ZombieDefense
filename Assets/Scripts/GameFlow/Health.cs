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

    public Action OnDied;
    public UnityEvent<float> OnLoseHealth;
    public UnityEvent<float> OnGainHealth;

    private void OnValidate() => _animator = GetComponent<Animator>();

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        enabled = true;
        _currentHealth = _fullHealth;
        OnGainHealth?.Invoke(_currentHealth / _fullHealth);
    }

    public void TakeDamage(float _damage)
    {
        _currentHealth -= _damage;
        OnLoseHealth?.Invoke(_currentHealth / _fullHealth);
        if (_currentHealth <= 0)
        {
            if (_animator != null)
                _animator.SetTrigger(DeadHash);
            _currentHealth = 0;
            OnDied?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        _currentHealth += amount;
        if (_currentHealth > _fullHealth)
            _currentHealth = _fullHealth;

        OnGainHealth?.Invoke(_currentHealth / _fullHealth);
    }
}
