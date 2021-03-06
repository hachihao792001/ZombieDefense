using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class ZombieLeftTextBinding : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    private TMP_Text _zombieLeftText;

    private void OnValidate() => _zombieLeftText = GetComponent<TMP_Text>();

    private void Awake()
    {
        GameController.Instance.OnZombieDiedAction += OnZombieDied;
        GameController.Instance.OnNewRound += OnNewRound;
    }

    private void OnNewRound()
    {
        UpdateText(GameController.Instance.ZombieSpawner.CurrentRoundZombieCount);
    }

    private void OnZombieDied(Zombie _)
    {
        UpdateText(GameController.Instance.ZombieSpawner.Zombies.Count);
    }

    private void UpdateText(int zombieLeft)
    {
        _zombieLeftText.text = $"Zombie: {zombieLeft}/{GameController.Instance.ZombieSpawner.CurrentRoundZombieCount}";
        transform.DOScale(Vector3.one * 1.2f, 0.2f).OnComplete(() =>
        {
            transform.DOScale(Vector3.one, 0.2f);
        });
    }
}
