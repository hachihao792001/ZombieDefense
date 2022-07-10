using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField]
    private float _delay;

    private void Start()
    {
        Destroy(gameObject, _delay);
    }
}
