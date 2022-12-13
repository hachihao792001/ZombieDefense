using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class RoundTextBinding : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    private TMP_Text _roundText;

    [SerializeField]
    private int _totalRound;

    private void OnValidate()
    {
        _roundText = GetComponent<TMP_Text>();
        if (GameController.Instance != null)
            _totalRound = GameController.Instance.ZombieSpawner.TotalRound;
    }

    private void Start()
    {
        GameController.Instance.OnNewRound += OnNewRound;
    }

    private void OnNewRound()
    {
        _roundText.text = $"Round {GameController.Instance.CurrentRound}/{_totalRound}";
        transform.DOScale(Vector3.one * 1.2f, 0.2f).OnComplete(() =>
        {
            transform.DOScale(Vector3.one, 0.2f);
        });
    }
}
