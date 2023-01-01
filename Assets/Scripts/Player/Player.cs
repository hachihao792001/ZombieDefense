using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviourPun
{
    public PlayerMoving PlayerMoving;
    public PlayerLooking PlayerLooking;
    public PlayerTurretPlacer PlayerTurretPlacer;
    public PlayerAmmoRefiller PlayerAmmoRefiller;
    public Transform CameraAndWeapon;
    public Health Health;
    public ArmSwitcher ArmSwitcher;
    public GrenadeThrowing GrenadeThrowing;

    [SerializeField]
    private GameObject _handGun;
    [SerializeField]
    private GameObject _rifle;
    [SerializeField]
    private Rigidbody _rb;

    [SerializeField]
    private GameObject _mainCamera;
    [SerializeField]
    private GameObject _gunCamera;
    [SerializeField]
    private GameObject[] _gunLayerObjects;
    [SerializeField]
    private int _defaultLayer = 0;

    private void Start()
    {
        Health.OnDied += OnPlayerDied;
        PlayerAmmoRefiller.OnAmmoRefill += OnAmmoRefill;

        if (!photonView.IsMine)
        {
            _mainCamera.SetActive(false);
            _gunCamera.SetActive(false);
            for (int i = 0; i < _gunLayerObjects.Length; i++)
            {
                _gunLayerObjects[i].layer = _defaultLayer;
            }
        }
    }

    private void OnAmmoRefill()
    {
        ArmSwitcher.CurrentArm.Shooting.Unlock();
    }

    private void OnValidate()
    {
        PlayerMoving = GetComponent<PlayerMoving>();
        PlayerLooking = GetComponent<PlayerLooking>();
        PlayerTurretPlacer = GetComponent<PlayerTurretPlacer>();
        PlayerAmmoRefiller = GetComponent<PlayerAmmoRefiller>();
        Health = GetComponent<Health>();
        ArmSwitcher = GetComponent<ArmSwitcher>();
        GrenadeThrowing = GetComponent<GrenadeThrowing>();
    }

    public void OnPlayerDied()
    {
        Health.OnDied -= OnPlayerDied;

        GameController.Instance.GameOver(false, "You died!");

        PlayerMoving.enabled = false;
        PlayerLooking.enabled = false;
        ArmSwitcher.enabled = false;
        Health.enabled = false;
        _handGun.SetActive(false);
        _rifle.SetActive(false);

        _rb.freezeRotation = false;

        transform.eulerAngles = new Vector3(-1, transform.eulerAngles.y, 0);

        GameController.Instance.Allies.Remove(transform);
    }
}
