using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerItemController : MonoBehaviour
{
    [SerializeField] RoomController _roomController;

    [SerializeField] GameObject _playerInfo;
    [SerializeField] Image _imgPlayerAvatar;
    [SerializeField] TMP_Text _txtPlayerName;
    [SerializeField] GameObject _hostIcon;
    [SerializeField] GameObject _meBorder;
    public void Init(Photon.Realtime.Player player)
    {
        if (player == null)
        {
            _playerInfo.SetActive(false);
            return;
        }

        _playerInfo.SetActive(true);
        _imgPlayerAvatar.sprite = _roomController.playerAvatarInfos[PhotonHelper.GetAvatarIndexOf(player.ActorNumber)].img;
        _txtPlayerName.text = player.NickName;

        _hostIcon.SetActive(player.IsMasterClient);
        _meBorder.SetActive(player.IsLocal);
    }
}
