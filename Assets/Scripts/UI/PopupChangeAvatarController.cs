using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupChangeAvatarController : MonoBehaviour
{
    [SerializeField] List<AvatarItemController> _avatarItems;
    Action<int> onChosenNewAvatar;

    public void Show(Action<int> onChosenNewAvatar)
    {
        gameObject.SetActive(true);
        this.onChosenNewAvatar = onChosenNewAvatar;

        int currentAvatarIndex = PlayerPrefs.GetInt("avatar", 0);
        for (int i = 0; i < _avatarItems.Count; i++)
        {
            _avatarItems[i].Init(i, i == currentAvatarIndex, AvatarItemOnClick);
        }
    }

    void AvatarItemOnClick(int index)
    {
        PlayerPrefs.SetInt("avatar", index);
        PhotonHelper.SetMyAvatarIndex(index);
        onChosenNewAvatar?.Invoke(index);
        gameObject.SetActive(false);
    }
}
