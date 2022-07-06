using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByJoyStick : MonoBehaviour
{
    [SerializeField]
    private JoyStick _joyStick;

    Rigidbody _bodyRb;

    [Space]
    [Header("Walking & Running")]
    [SerializeField]
    private float _walkSpeed = 3;
    private bool _running;
    [SerializeField]
    private ButtonHolding _runButton;

    public bool IsMoving { get; private set; }
    public bool IsRunning => _running;

    [Space]
    [Header("Jumping")]
    [SerializeField]
    private float _groundDistance = 1.1f;
    private bool _grounded;
    [SerializeField]
    private float _jumpForce = 200;

    private void OnValidate() => _bodyRb = GetComponent<Rigidbody>();

    void Update()
    {
        UpdateMoving();
    }

    private void UpdateMoving()
    {
        _running = _runButton.IsHolding;
        Vector2 input = new Vector2(_joyStick.Input.x, _joyStick.Input.y);
        input *= _walkSpeed * (_running ? 2 : 1);
        _bodyRb.velocity = transform.TransformDirection(new Vector3(input.x, _bodyRb.velocity.y, input.y));

        IsMoving = input != Vector2.zero;
    }

    public void JumpOnClick()
    {
        _grounded = Physics.Raycast(transform.position, Vector3.down, _groundDistance);
        if (_grounded)
            _bodyRb.AddForce(Vector3.up * _jumpForce);
    }
}
