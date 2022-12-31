using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_PC : MonoBehaviour
{
    [SerializeField]
    Transform _cameraAndWeapon;
    [SerializeField]
    Rigidbody _bodyRb;

    [Header("Mouse")]

    [SerializeField]
    private float _sentivity = 2;
    [SerializeField]
    private float _minPitch;
    [SerializeField]
    private float _maxPitch;

    [Space]
    [Header("Walking & Running")]
    [SerializeField]
    private float _walkSpeed = 3;
    private bool _running;
    private Vector2 _input;

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

    void Start()
    {
        GameController.Instance.LockCursor();
    }

    void Update()
    {
        UpdateMouse();
        UpdateMoving();
        UpdateJumping();
    }

    private void UpdateJumping()
    {
        _grounded = Physics.Raycast(transform.position, Vector3.down, _groundDistance);
        if (_grounded && Input.GetKeyDown(KeyCode.Space))
            _bodyRb.AddForce(Vector3.up * _jumpForce);
    }

    private void UpdateMoving()
    {
        _running = Input.GetKey(KeyCode.LeftShift);
        _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _input *= _walkSpeed * (_running ? 2 : 1);
        _bodyRb.velocity = transform.TransformDirection(new Vector3(_input.x, _bodyRb.velocity.y, _input.y));

        IsMoving = _input != Vector2.zero;
    }

    private void UpdateMouse()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * _sentivity);
        _cameraAndWeapon.Rotate(Vector3.left * Input.GetAxis("Mouse Y") * _sentivity);
        ClampPitchAngle();
    }

    private void ClampPitchAngle()
    {
        float pitch = _cameraAndWeapon.localEulerAngles.x;
        while (pitch > 180)
        {
            pitch -= 360;
        }
        while (pitch < -180)
        {
            pitch += 360;
        }

        pitch = Mathf.Clamp(pitch, _minPitch, _maxPitch);
        _cameraAndWeapon.localEulerAngles = new Vector3(pitch, 0, 0);
    }
}
