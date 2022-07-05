using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicator : MonoBehaviour
{
    private Transform _target;
    private Transform _minimap;
    private Camera _minimapCamera;

    public void SetData(Camera minimapCamera, Transform minimap, Transform target)
    {
        _minimapCamera = minimapCamera;
        _minimap = minimap;
        _target = target;
    }

    private void Update()
    {
        transform.position = _minimapCamera.WorldToScreenPoint(_target.position) + _minimap.position
            - new Vector3(400f / 2f, 250f / 2f, 0);
    }
}
