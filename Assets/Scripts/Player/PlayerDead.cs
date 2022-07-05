using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    [SerializeField]
    public GameObject _deadScreen;

    [SerializeField]
    private FPS_PC _FPS_PC;
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

    public void OnPlayerDied()
    {
        _deadScreen.SetActive(true);

        _FPS_PC.enabled = false;
        _gunSwitcher.enabled = false;
        _health.enabled = false;
        _handGun.SetActive(false);
        _rifle.SetActive(false);

        _rb.freezeRotation = false;

        transform.eulerAngles = new Vector3(-1, transform.eulerAngles.y, 0);

        GameController.Instance.Allies.Remove(transform);
    }
}
