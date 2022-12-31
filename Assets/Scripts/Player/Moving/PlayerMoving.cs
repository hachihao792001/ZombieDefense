using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoving : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    protected Rigidbody _bodyRb;

    [Space]
    [Header("Walking & Running")]
    [SerializeField]
    protected float _walkSpeed = 3;
    protected bool _running;

    public bool IsMoving { get; private set; }
    public bool IsRunning => _running;

    [Space]
    [Header("Jumping")]
    [SerializeField]
    private float _groundDistance = 1.1f;
    private bool _grounded;
    [SerializeField]
    private float _jumpForce = 200;

    private void OnValidate()
    {
        _bodyRb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        UpdateMoving();
    }

    private void UpdateMoving()
    {
        _running = GetRunning();
        Vector2 input = GetInput();
        input *= _walkSpeed * (_running ? 2 : 1);
        _bodyRb.velocity = transform.TransformDirection(new Vector3(input.x, _bodyRb.velocity.y, input.y));

        IsMoving = input != Vector2.zero;
    }

    protected virtual bool GetRunning() { return false; }
    protected virtual Vector2 GetInput() { return Vector2.zero; }

    protected virtual void Jump()
    {
        _grounded = Physics.Raycast(transform.position, Vector3.down, _groundDistance);
        if (_grounded)
            _bodyRb.AddForce(Vector3.up * _jumpForce);
    }
}
