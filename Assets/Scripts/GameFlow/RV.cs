using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RV : MonoBehaviour
{
    public Transform[] AttackPositions;

    [HideInInspector]
    [SerializeField]
    private Health _health;

    private void OnValidate()
    {
        _health = GetComponent<Health>();
    }

    private void Start()
    {
        _health.OnDied += () => GameController.Instance.GameOver(false, "You failed to save the RV!");
    }
}
