using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundTextBinding : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    private TMP_Text _roundText;

    private void OnValidate() => _roundText = GetComponent<TMP_Text>();

    private void Start()
    {
        GameController.Instance.OnNewRound = OnNewRound;
    }

    private void OnNewRound()
    {
        _roundText.text = $"Round {GameController.Instance.CurrentRound}";
    }
}
