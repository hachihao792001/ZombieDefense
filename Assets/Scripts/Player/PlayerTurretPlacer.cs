using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurretPlacer : MonoBehaviour
{
    [SerializeField]
    private GameObject _turretPrefab;
    [SerializeField]
    private Transform _turretsParent;
    [SerializeField]
    private float _placeDistance;

    public void PlaceNewTurret()
    {
        Instantiate(_turretPrefab, transform.position + transform.forward.normalized * _placeDistance, Quaternion.identity, _turretsParent);
    }
}
