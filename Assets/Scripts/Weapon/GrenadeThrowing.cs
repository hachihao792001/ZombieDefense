using System;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrowing : MonoBehaviour
{
    [SerializeField]
    private int _startGrenadeCount;
    [SerializeField]
    private int _grenadeLeft;
    [SerializeField]
    private float _throwForce;
    [SerializeField]
    private Grenade _grenadePrefab;
    [SerializeField]
    private Transform _cameraAndWeapon;

    public Action OnGrenadeCountChanged;
    public int GrenadeLeft => _grenadeLeft;
    public int StartGrenadeCount => _startGrenadeCount;

    private void Start()
    {
        _grenadeLeft = _startGrenadeCount;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            ThrowGrenade();
    }

    public void ThrowGrenade()
    {
        if (_grenadeLeft > 0)
        {
            Grenade newGrenade = Instantiate(_grenadePrefab, transform.position, Quaternion.identity);
            Physics.IgnoreCollision(newGrenade.GetComponent<Collider>(), GetComponent<Collider>());
            newGrenade.AddForce(_cameraAndWeapon.forward * _throwForce);
            _grenadeLeft--;

            OnGrenadeCountChanged?.Invoke();
        }
    }

    public void ThrowGrenadeOnClick()
    {
        ThrowGrenade();
    }

    public void RefillGrenade()
    {
        _grenadeLeft = _startGrenadeCount;
        OnGrenadeCountChanged?.Invoke();
    }
}
