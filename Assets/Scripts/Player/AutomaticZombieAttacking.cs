using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticZombieAttacking : MonoBehaviourPun
{
    [SerializeField]
    private Transform aimingCamera;
    [HideInInspector]
    [SerializeField]
    private ArmSwitcher _armSwitcher;
    [HideInInspector]
    [SerializeField]
    private int _zombieLayer;

    private float lastShotTime;

    private void OnValidate()
    {
        _armSwitcher = GetComponent<ArmSwitcher>();
        _zombieLayer = LayerMask.NameToLayer("Enemy");
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;
        if (GameController.IsGameOver) return;
        Ray aimingRay = new Ray(aimingCamera.position, aimingCamera.forward);
        if (Physics.Raycast(aimingRay, out RaycastHit hitInfo))
        {
            if (hitInfo.transform.gameObject.layer == _zombieLayer)
            {
                if (hitInfo.distance <= _armSwitcher.CurrentArm.Meleeing.Distance && _armSwitcher.CurrentArm.Meleeing.enabled)
                {
                    float interval = 60f / _armSwitcher.CurrentArm.Meleeing.Rpm;
                    if (Time.time - lastShotTime >= interval)
                    {
                        _armSwitcher.CurrentArm.Meleeing.DoMelee();
                        lastShotTime = Time.time;
                    }
                }
                else if (_armSwitcher.CurrentArm.Shooting.enabled)
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
    }
}
