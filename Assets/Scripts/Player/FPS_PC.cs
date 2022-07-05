using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_PC : MonoBehaviour
{
    [SerializeField]
    Transform _cameraAndWeapon;
    Rigidbody _bodyRb;

    [Header("Mouse")]

    [SerializeField]
    private float _sentivity = 2;

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
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Mouse
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * _sentivity);
        _cameraAndWeapon.Rotate(Vector3.left * Input.GetAxis("Mouse Y") * _sentivity);
        _cameraAndWeapon.eulerAngles = new Vector3(_cameraAndWeapon.eulerAngles.x, _cameraAndWeapon.eulerAngles.y, 0);

        //Walking & Running
        _running = Input.GetKey(KeyCode.LeftShift);
        _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _input *= _walkSpeed * (_running ? 2 : 1);
        _bodyRb.velocity = transform.TransformDirection(new Vector3(_input.x, _bodyRb.velocity.y, _input.y));

        IsMoving = _input != Vector2.zero;

        //Jumping
        _grounded = Physics.Raycast(transform.position, Vector3.down, _groundDistance);
        if (_grounded && Input.GetKeyDown(KeyCode.Space))
            _bodyRb.AddForce(Vector3.up * _jumpForce);
    }
}
