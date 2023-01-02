using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurretPlacer : MonoBehaviour
{
    [SerializeField]
    private string _turretPrefabName;
    [SerializeField]
    private Transform _turretsParent;
    [SerializeField]
    private float _placeDistance;

    public void PlaceNewTurret()
    {
        GameObject spawnedTurret = PhotonHelper.SpawnNewNetworkObject(_turretPrefabName, transform.position + transform.forward.normalized * _placeDistance, Quaternion.identity);
        spawnedTurret.transform.parent = _turretsParent;
    }
}
