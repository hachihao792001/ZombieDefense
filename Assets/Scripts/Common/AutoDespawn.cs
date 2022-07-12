using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDespawn : MonoBehaviour
{
    [SerializeField]
    private float _delay;

    private void Start()
    {
        Lean.Pool.LeanPool.Despawn(gameObject, _delay);
    }
}
