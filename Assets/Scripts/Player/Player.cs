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
    public AutomaticZombieAttacking AutomaticZombieAttacking;
    public PlayerHealthBar PlayerHealthBar;
    public PlayerFlying PlayerFlying;
    public PlayerAvatar PlayerAvatar;

    [SerializeField]
    private Fading _loseHealthEffect;

    [SerializeField]
    private GameObject _handGun;
    [SerializeField]
    private GameObject _rifle;
    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private Collider _collider;

    [SerializeField]
    private GameObject[] _objectsToHideForNonLocalPlayer;
    [SerializeField]
    private GameObject[] _objectsToChangeLayerForNonLocalPlayer;
    [SerializeField]
    private GameObject[] _objectsToHideWhenDied;
    [SerializeField]
    private int _defaultLayer = 0;

    private void Start()
    {
        Health.OnLoseHealth.AddListener(OnLoseHealth);
        Health.OnDied += OnPlayerDied;
        PlayerAmmoRefiller.OnAmmoRefill += OnAmmoRefill;

        if (!photonView.IsMine)
        {
            for (int i = 0; i < _objectsToHideForNonLocalPlayer.Length; i++)
            {
                _objectsToHideForNonLocalPlayer[i].SetActive(false);
            }
            for (int i = 0; i < _objectsToChangeLayerForNonLocalPlayer.Length; i++)
            {
                _objectsToChangeLayerForNonLocalPlayer[i].layer = _defaultLayer;
            }
        }
        else
        {
            photonView.RPC(nameof(RPC_NotifyNewPlayerToMasterClient), RpcTarget.MasterClient, photonView.ViewID);
        }

        PlayerHealthBar.SetPlayerName(PhotonNetwork.NickName);
        photonView.RPC(nameof(SetPlayerName), RpcTarget.OthersBuffered, photonView.ViewID, PhotonNetwork.NickName);
    }

    [PunRPC]
    void RPC_NotifyNewPlayerToMasterClient(int viewId)
    {
        GameController.Instance.OnNewPlayerSpawned(viewId);
    }

    [PunRPC]
    void SetPlayerName(int viewId, string name)
    {
        PlayerHealthBar.SetPlayerName(PhotonNetwork.NickName);
    }

    private void OnDestroy()
    {
        Health.OnLoseHealth.RemoveListener(OnLoseHealth);
        Health.OnDied -= OnPlayerDied;
        PlayerAmmoRefiller.OnAmmoRefill -= OnAmmoRefill;
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
        AutomaticZombieAttacking = GetComponent<AutomaticZombieAttacking>();
        PlayerFlying = GetComponent<PlayerFlying>();
        PlayerAvatar = GetComponent<PlayerAvatar>();
    }

    public void OnLoseHealth(float amount)
    {
        if (_loseHealthEffect.gameObject.activeInHierarchy)
            _loseHealthEffect.StartFading();
    }

    public void OnPlayerDied()
    {
        Health.OnDied -= OnPlayerDied;

        photonView.RPC(nameof(RPC_NotifyPlayerDiedToMasterClient), RpcTarget.MasterClient, photonView.ViewID);

        PlayerMoving.enabled = false;
        PlayerLooking.enabled = false;
        ArmSwitcher.enabled = false;
        Health.enabled = false;
        GrenadeThrowing.enabled = false;
        AutomaticZombieAttacking.enabled = false;
        _handGun.SetActive(false);
        _rifle.SetActive(false);
        PlayerAvatar.HideAvatar();

        _rb.isKinematic = true;
        _collider.enabled = false;

        transform.eulerAngles = new Vector3(-1, transform.eulerAngles.y, 0);

        for (int i = 0; i < _objectsToHideWhenDied.Length; i++)
        {
            _objectsToHideWhenDied[i].SetActive(false);
        }
        PlayerFlying.enabled = true;
    }

    [PunRPC]
    private void RPC_NotifyPlayerDiedToMasterClient(int viewId)
    {
        GameController.Instance.OnPlayerDied(viewId);
    }
}
