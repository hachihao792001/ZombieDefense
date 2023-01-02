using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] AlwaysLookAt _alwaysLookAt;
    [SerializeField] TMP_Text _txtPlayerName;

    private void Start()
    {
        _alwaysLookAt.SetTarget(GameController.Instance.Player.transform);
    }

    public void SetPlayerName(string name)
    {
        _txtPlayerName.text = name;
    }
}
