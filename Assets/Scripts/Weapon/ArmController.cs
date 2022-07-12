using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    public Shooting Shooting;
    public Meleeing Meleeing;

    private void OnValidate()
    {
        Shooting = GetComponent<Shooting>();
        Meleeing = GetComponent<Meleeing>();
    }
}
