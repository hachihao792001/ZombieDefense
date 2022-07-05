using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Collider))]
public class NeedDefendingBuilding : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    private Health _health;
    [SerializeField]
    private GameObject _healthBarCanvas;

    private void OnValidate() => _health = GetComponent<Health>();

    public void Activate()
    {
        _health.enabled = true;
    }

    public void Deactivate()
    {
        _health.enabled = false;
    }
}
