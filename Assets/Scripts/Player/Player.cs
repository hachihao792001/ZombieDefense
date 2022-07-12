using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public GameObject _deadScreen;

    public PlayerMoving PlayerMoving;
    public PlayerLooking PlayerLooking;
    public PlayerTurretPlacer PlayerTurretPlacer;
    public PlayerAmmoRefiller PlayerAmmoRefiller;
    public Transform CameraAndWeapon;
    public Health Health;
    [SerializeField]
    private ArmSwitcher _gunSwitcher;
    [SerializeField]
    private GameObject _handGun;
    [SerializeField]
    private GameObject _rifle;
    [SerializeField]
    private Rigidbody _rb;

    private void Start()
    {
        Health.OnDied = OnPlayerDied;
    }

    private void OnValidate()
    {
        PlayerMoving = GetComponent<PlayerMoving>();
        PlayerLooking = GetComponent<PlayerLooking>();
        PlayerTurretPlacer = GetComponent<PlayerTurretPlacer>();
        PlayerAmmoRefiller = GetComponent<PlayerAmmoRefiller>();
        Health = GetComponent<Health>();
    }

    public void OnPlayerDied()
    {
        Health.OnDied -= OnPlayerDied;

        _deadScreen.SetActive(true);

        PlayerMoving.enabled = false;
        PlayerLooking.enabled = false;
        _gunSwitcher.enabled = false;
        Health.enabled = false;
        _handGun.SetActive(false);
        _rifle.SetActive(false);

        _rb.freezeRotation = false;

        transform.eulerAngles = new Vector3(-1, transform.eulerAngles.y, 0);

        GameController.Instance.Allies.Remove(transform);
    }
}
