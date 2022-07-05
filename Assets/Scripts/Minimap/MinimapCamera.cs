using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField]
    private Transform _player;

    private void Update()
    {
        transform.position = new Vector3(_player.position.x, transform.position.y, _player.position.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, _player.eulerAngles.y, transform.eulerAngles.z);
    }
}
