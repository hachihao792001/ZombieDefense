using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByKey : MonoBehaviour
{
    [SerializeField]
    Transform _cameraAndWeapon;
    Rigidbody bodyRb;

    [Header("Mouse")]

    [SerializeField]
    private float sentivity = 2;

    [Space]
    [Header("Walking & Running")]
    [SerializeField]
    private float walkSpeed = 3;
    private bool running;
    private Vector2 input;

    [Space]
    [Header("Jumping")]
    [SerializeField]
    private float groundDistance = 1.1f;
    private bool grounded;
    [SerializeField]
    private float jumpForce = 200;

    private void OnValidate()
    {
        bodyRb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Mouse
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * sentivity);
        _cameraAndWeapon.Rotate(Vector3.left * Input.GetAxis("Mouse Y") * sentivity);
        _cameraAndWeapon.eulerAngles = new Vector3(_cameraAndWeapon.eulerAngles.x, _cameraAndWeapon.eulerAngles.y, 0);

        //Walking & Running
        running = Input.GetKey(KeyCode.LeftControl);
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input *= walkSpeed * (running ? 2 : 1);
        bodyRb.velocity = transform.TransformDirection(new Vector3(input.x, bodyRb.velocity.y, input.y));

        //Jumping
        grounded = Physics.Raycast(transform.position, Vector3.down, groundDistance);
        if (grounded && Input.GetKeyDown(KeyCode.Space))
            bodyRb.AddForce(Vector3.up * jumpForce);
    }
}
