using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ZombieLeftTextBinding : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    private TMP_Text _zombieLeftText;

    private void OnValidate() => _zombieLeftText = GetComponent<TMP_Text>();

    private void Start()
    {
        GameController.Instance.OnZombieDiedAction += OnZombieDied;
    }

    private void OnZombieDied(Zombie _)
    {
        _zombieLeftText.text = $"Zombie: {GameController.Instance.ZombieSpawner.Zombies.Count}/{GameController.Instance.ZombieSpawner.CurrentRoundZombieCount}";
        transform.DOScale(Vector3.one * 1.2f, 0.2f).OnComplete(() =>
        {
            transform.DOScale(Vector3.one, 0.2f);
        });
    }
}
