using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class WinScreenController : MonoBehaviour
{
    [SerializeField] TMP_Text _txtCounter;
    [SerializeField] float _duration;
    float timeLeft;

    void OnEnable()
    {
        timeLeft = _duration;
        _txtCounter.text = "Return to room in " + timeLeft + "s...";
        StartCoroutine(returnToRoomCoroutine());
    }
    IEnumerator returnToRoomCoroutine()
    {
        while (timeLeft > 0)
        {
            timeLeft--;
            _txtCounter.text = "Return to room in " + timeLeft + "s...";
            yield return new WaitForSeconds(1);
        }
        if (PhotonNetwork.IsMasterClient)
            PhotonHelper.LoadScene("Home");
    }
}
