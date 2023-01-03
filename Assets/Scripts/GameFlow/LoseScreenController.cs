using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class LoseScreenController : MonoBehaviour
{
    [SerializeField] TMP_Text _txtCounter;
    [SerializeField] float _duration;
    [SerializeField] TMP_Text _txtReason;

    float timeLeft;

    public void Show(string reason)
    {
        gameObject.SetActive(true);
        _txtReason.text = reason;

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
