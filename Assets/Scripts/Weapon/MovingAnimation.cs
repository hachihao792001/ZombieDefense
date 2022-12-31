using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAnimation : MonoBehaviourPun
{
    private readonly int WalkingHash = Animator.StringToHash("Walking");
    private readonly int RunningHash = Animator.StringToHash("Running");


    [HideInInspector]
    [SerializeField]
    private Animator _animator;

    private PlayerMoving _playerMoving;

    private void OnValidate()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _playerMoving = GameController.Instance.Player.PlayerMoving;
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;
        if (_playerMoving.IsMoving)
        {
            _animator.SetBool(WalkingHash, !_playerMoving.IsRunning);
            _animator.SetBool(RunningHash, _playerMoving.IsRunning);
        }
        else
        {
            _animator.SetBool(WalkingHash, false);
            _animator.SetBool(RunningHash, false);
        }
    }
}
