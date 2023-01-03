using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviourPun
{
    public PlayerAvatarInfo[] playerAvatarInfos;
    [SerializeField] GameObject[] _avatarObjs;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            return;
        int thisPlayerAvatarIndex = (int)photonView.Owner.CustomProperties["avatar"];
        for (int i = 0; i < playerAvatarInfos.Length; i++)
        {
            _avatarObjs[i].SetActive(thisPlayerAvatarIndex == i);
        }
    }

    public void HideAvatar()
    {
        for (int i = 0; i < playerAvatarInfos.Length; i++)
        {
            _avatarObjs[i].SetActive(false);
        }
    }
}
