using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RV : MonoBehaviour
{
    public Transform[] AttackPositions;

    [HideInInspector]
    [SerializeField]
    private Health _health;

    bool died;

    private void OnValidate()
    {
        _health = GetComponent<Health>();
    }

    private void Start()
    {
        died = false;
        _health.OnDied += () =>
        {
            if (!died)
            {
                if (PhotonNetwork.IsMasterClient)
                    GameController.Instance.GameOver(false, "You failed to save the RV!");
                died = true;
            }
        };
    }
}
