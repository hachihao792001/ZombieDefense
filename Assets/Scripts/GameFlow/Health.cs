using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviourPun, IPunObservable
{
    private readonly int DeadHash = Animator.StringToHash("Dead");
    private readonly byte DeadEventCode = 4;

    [SerializeField]
    private float _fullHealth;
    [SerializeField]
    private float _currentHealth;

    [HideInInspector]
    [SerializeField]
    private Animator _animator;

    public Action OnDied;
    public UnityEvent<float> OnLoseHealth;
    public UnityEvent<float> OnGainHealth;

    private void OnValidate() => _animator = GetComponent<Animator>();

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        enabled = true;
        _currentHealth = _fullHealth;
        OnGainHealth?.Invoke(_currentHealth / _fullHealth);
    }

    public void TakeDamage(float _damage)
    {
        _currentHealth -= _damage;
        OnLoseHealth?.Invoke(_currentHealth / _fullHealth);
        if (_currentHealth <= 0)
        {
            HandleDead();
            PhotonNetwork.RaiseEvent(DeadEventCode, photonView.ViewID, RaiseEventOptions.Default, SendOptions.SendUnreliable);
        }
    }

    void HandleDead()
    {
        if (_animator != null)
            _animator.SetTrigger(DeadHash);

        _currentHealth = 0;
        OnDied?.Invoke();
    }

    public void Heal(float amount)
    {
        _currentHealth += amount;
        if (_currentHealth > _fullHealth)
            _currentHealth = _fullHealth;

        OnGainHealth?.Invoke(_currentHealth / _fullHealth);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_currentHealth);
        }
        else if (stream.IsReading)
        {
            float newHealth = (float)stream.ReceiveNext();
            if (newHealth < _currentHealth)
                OnLoseHealth?.Invoke(_currentHealth / _fullHealth);
            if (newHealth > _currentHealth)
                OnGainHealth?.Invoke(_currentHealth / _fullHealth);
            _currentHealth = newHealth;
        }
    }
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }
    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == DeadEventCode)
        {
            int viewId = (int)obj.CustomData;
            PhotonView view = PhotonView.Find(viewId);

            Health health = view.GetComponent<Health>();
            health.HandleDead();
        }
    }
}
