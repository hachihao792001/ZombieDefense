using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAnimation : MonoBehaviour
{
    private readonly int WalkingHash = Animator.StringToHash("Walking");
    private readonly int RunningHash = Animator.StringToHash("Running");


    [HideInInspector]
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private MoveByJoyStick fps;

    private void OnValidate()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (fps.IsMoving)
        {
            _animator.SetBool(WalkingHash, !fps.IsRunning);
            _animator.SetBool(RunningHash, fps.IsRunning);
        }
        else
        {
            _animator.SetBool(WalkingHash, false);
            _animator.SetBool(RunningHash, false);
        }
    }
}
