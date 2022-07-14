using System;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrowing : MonoBehaviour
{
    [SerializeField]
    private int _startGrenade;
    [SerializeField]
    private int _grenadeLeft;
    [SerializeField]
    private Vector3 _throwDir;
    [SerializeField]
    private float _throwForce;
    [SerializeField]
    private Grenade _grenadePrefab;

    public Action OnThrowGrenade;
    public int GrenadeLeft => _grenadeLeft;
    public int StartGrenadeCount => _startGrenade;

    private void Start()
    {
        _grenadeLeft = _startGrenade;
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
            newGrenade.AddForce(transform.TransformDirection(_throwDir.normalized * _throwForce));
            _grenadeLeft--;

            OnThrowGrenade?.Invoke();
        }
    }

    public void ThrowGrenadeOnClick()
    {
        ThrowGrenade();
    }
}
