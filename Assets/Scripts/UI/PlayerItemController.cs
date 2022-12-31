using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerItemController : MonoBehaviour
{
    [SerializeField] GameObject _playerInfo;
    [SerializeField] Image _imgPlayerAvatar;
    [SerializeField] TMP_Text _txtPlayerName;
    [SerializeField] GameObject _hostBorder;
    public void Init(Photon.Realtime.Player player)
    {
        if (player == null)
        {
            _playerInfo.SetActive(false);
            return;
        }

        _playerInfo.SetActive(true);
        _txtPlayerName.text = player.NickName;

        _hostBorder.SetActive(player.IsMasterClient);
    }
}
