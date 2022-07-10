using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public GameObject _deadScreen;

    public PlayerMoving PlayerMoving;
    public PlayerLooking PlayerLooking;
    [SerializeField]
    private GunSwitcher _gunSwitcher;
    [SerializeField]
    private Health _health;
    [SerializeField]
    private GameObject _handGun;
    [SerializeField]
    private GameObject _rifle;
    [SerializeField]
    private Rigidbody _rb;

    private void Start()
    {
        _health.OnDied = OnPlayerDied;
    }

    public void OnPlayerDied()
    {
        _health.OnDied -= OnPlayerDied;

        _deadScreen.SetActive(true);

        PlayerMoving.enabled = false;
        PlayerLooking.enabled = false;
        _gunSwitcher.enabled = false;
        _health.enabled = false;
        _handGun.SetActive(false);
        _rifle.SetActive(false);

        _rb.freezeRotation = false;

        transform.eulerAngles = new Vector3(-1, transform.eulerAngles.y, 0);

        GameController.Instance.Allies.Remove(transform);
    }
}
