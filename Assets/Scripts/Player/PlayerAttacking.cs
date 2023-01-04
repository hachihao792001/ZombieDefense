using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviourPun
{
    [SerializeField]
    private Transform aimingCamera;
    [HideInInspector]
    [SerializeField]
    private ArmSwitcher _armSwitcher;

    private float lastShotTime;

    private void OnValidate()
    {
        _armSwitcher = GetComponent<ArmSwitcher>();
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;
        if (GameController.IsGameOver) return;

        if (_armSwitcher.CurrentArm.Shooting.enabled)
        {
            if (_armSwitcher.CurrentArm.Shooting is PistolShooting)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    float interval = 60f / _armSwitcher.CurrentArm.Shooting.Rpm;
                    if (Time.time - lastShotTime >= interval)
                    {
                        _armSwitcher.CurrentArm.Shooting.Shoot();
                        lastShotTime = Time.time;
                    }
                }
            }
            else if (_armSwitcher.CurrentArm.Shooting is RifleShooting)
            {
                if (Input.GetMouseButton(0))
                {
                    float interval = 60f / _armSwitcher.CurrentArm.Shooting.Rpm;
                    if (Time.time - lastShotTime >= interval)
                    {
                        _armSwitcher.CurrentArm.Shooting.Shoot();
                        lastShotTime = Time.time;
                    }
                }
            }
        }
        
        if (_armSwitcher.CurrentArm.Meleeing.enabled && Input.GetMouseButton(1))
        {
            float interval = 60f / _armSwitcher.CurrentArm.Meleeing.Rpm;
            if (Time.time - lastShotTime >= interval)
            {
                _armSwitcher.CurrentArm.Meleeing.DoMelee();
                lastShotTime = Time.time;
            }
        }
    }
}