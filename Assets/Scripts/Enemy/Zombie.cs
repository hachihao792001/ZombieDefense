using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public enum ZombieType
{
    Normal = 0,
    Fast = 1,
    Big = 2
}

public class Zombie : MonoBehaviourPun
{
    public ZombieType ZombieType;

    [HideInInspector]
    public ZombieMoving ZombieMoving;
    [HideInInspector]
    public ZombieAttack ZombieAttack;
    [HideInInspector]
    public Health ZombieHealth;
    [HideInInspector]
    public Collider ZombieCollider;
    [SerializeField]
    private Collider _headCollider;
    [SerializeField]
    private GameObject _healthBarCanvas;
    [SerializeField]
    private GameObject _minimapIndicator;

    public Action<Zombie> OnZombieDied;

    public Transform TurretTarget;

    private void OnValidate()
    {
        ZombieMoving = GetComponent<ZombieMoving>();
        ZombieAttack = GetComponent<ZombieAttack>();
        ZombieHealth = GetComponent<Health>();
        ZombieCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        ZombieHealth.OnDied = OnZombieDead;
        if (GameController.Instance.Player != null)
            _healthBarCanvas.GetComponent<AlwaysLookAt>().SetTarget(GameController.Instance.Player.transform);
    }

    public void OnZombieDead()
    {
        ZombieMoving.OnDied();
        ZombieAttack.OnDied();
        ZombieHealth.enabled = false;
        ZombieCollider.enabled = false;
        _headCollider.enabled = false;
        _healthBarCanvas.SetActive(false);
        _minimapIndicator.SetActive(false);

        OnZombieDied?.Invoke(this);

        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(destroyAfterSecond(5f));
    }

    IEnumerator destroyAfterSecond(float sec)
    {
        yield return new WaitForSeconds(sec);
        PhotonHelper.DestroyNetworkObject(gameObject);
    }

    public void Init()
    {
        ZombieMoving.Init();
        ZombieAttack.enabled = true;
        ZombieHealth.Init();
        ZombieCollider.enabled = true;
        _headCollider.enabled = true;
        _healthBarCanvas.SetActive(true);
        _minimapIndicator.SetActive(true);
    }
}
