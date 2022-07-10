using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RV : MonoBehaviour
{
    public Transform[] AttackPositions;

    [SerializeField]
    private GameObject _loseScreen;
    [HideInInspector]
    [SerializeField]
    private Health _health;

    private void OnValidate()
    {
        _health = GetComponent<Health>();
    }

    private void Start()
    {
        _health.OnDied += () => _loseScreen.SetActive(true);
    }
}
