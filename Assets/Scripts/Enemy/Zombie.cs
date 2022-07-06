using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Zombie : MonoBehaviour
{
    [HideInInspector]
    public ZombieMoving ZombieMoving;
    [HideInInspector]
    public ZombieAttack ZombieAttack;
    [HideInInspector]
    public Health ZombieHealth;
    [HideInInspector]
    public Collider ZombieCollider;
    [SerializeField]
    private Collider _headCollider;
    [SerializeField]
    private GameObject _healthBarCanvas;

    public Action<Zombie> OnZombieDied;

    private void OnValidate()
    {
        ZombieMoving = GetComponent<ZombieMoving>();
        ZombieAttack = GetComponent<ZombieAttack>();
        ZombieHealth = GetComponent<Health>();
        ZombieCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        ZombieHealth.OnDied = OnZombieDead;
    }

    public void OnZombieDead()
    {
        ZombieMoving.OnDied();
        ZombieAttack.OnDied();
        ZombieHealth.enabled = false;
        ZombieCollider.enabled = false;
        _headCollider.enabled = false;
        _healthBarCanvas.SetActive(false);

        OnZombieDied?.Invoke(this);

        Destroy(gameObject, 5f);
    }
}
