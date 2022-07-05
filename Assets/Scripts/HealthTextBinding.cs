using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthTextBinding : MonoBehaviour
{
    [SerializeField]
    private Health _health;
    [HideInInspector]
    [SerializeField]
    private Image _healthBar;

    private void OnValidate() => _healthBar = GetComponent<Image>();

    void Start()
    {
        _health.OnHealthChanged += UpdateHealth;
        UpdateHealth(1f);
    }

    private void UpdateHealth(float newPercent) => _healthBar.fillAmount = newPercent;
}
