using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    }
}
