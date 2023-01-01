using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviourPun
{
    private readonly int DeadHash = Animator.StringToHash("Dead");
    private readonly byte DeadEventCode = 4;
    private readonly byte TakeDamageEventCode = 5;
    private readonly byte HealEventCode = 6;

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
        //Debug.Log("TakeDamage " + _damage + " viewId " + photonView.ViewID, this);
        HandleLocalTakeDamage(_damage);
        PhotonNetwork.RaiseEvent(TakeDamageEventCode, new object[] { PhotonNetwork.LocalPlayer.ActorNumber, photonView.ViewID, _damage }, RaiseEventOptions.Default, SendOptions.SendUnreliable);

        if (_currentHealth <= 0)
        {
            HandleLocalDead();
            PhotonNetwork.RaiseEvent(DeadEventCode, new object[] { PhotonNetwork.LocalPlayer.ActorNumber, photonView.ViewID }, RaiseEventOptions.Default, SendOptions.SendUnreliable);
        }
    }

    private void HandleLocalTakeDamage(float _damage)
    {
        //Debug.Log("HandleLocalTakeDamage " + _damage + " viewId " + photonView.ViewID, this);
        _currentHealth -= _damage;
        OnLoseHealth?.Invoke(_currentHealth / _fullHealth);
    }

    void HandleLocalDead()
    {
        if (_animator != null)
            _animator.SetTrigger(DeadHash);

        _currentHealth = 0;
        OnDied?.Invoke();
    }

    public void Heal(float amount)
    {
        HandleLocalHeal(amount);
        PhotonNetwork.RaiseEvent(HealEventCode, new object[] { PhotonNetwork.LocalPlayer.ActorNumber, photonView.ViewID, amount }, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }

    private void HandleLocalHeal(float amount)
    {
        _currentHealth += amount;
        if (_currentHealth > _fullHealth)
            _currentHealth = _fullHealth;

        OnGainHealth?.Invoke(_currentHealth / _fullHealth);
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
        if (obj.Code != TakeDamageEventCode && obj.Code != HealEventCode && obj.Code != DeadEventCode)
            return;
        object[] data = (object[])obj.CustomData;

        int actorNumber = (int)data[0];
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)   //không gửi cho chính mình
            return;
        int viewId = (int)data[1];
        if (photonView.ViewID != viewId)
            return;

        if (obj.Code == TakeDamageEventCode)
        {
            float amount = (float)data[2];
            HandleLocalTakeDamage(amount);
        }
        else if (obj.Code == HealEventCode)
        {
            float amount = (float)data[2];
            HandleLocalHeal(amount);
        }
        else if (obj.Code == DeadEventCode)
        {
            HandleLocalDead();
        }
    }
}
