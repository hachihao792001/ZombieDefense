using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBinding : MonoBehaviour
{
    [SerializeField]
    private Health _health;
    [HideInInspector]
    [SerializeField]
    private Image _healthBar;

    private void OnValidate() => _healthBar = GetComponent<Image>();

    void Start()
    {
        _health.OnLoseHealth.AddListener(UpdateHealth);
        _health.OnGainHealth.AddListener(UpdateHealth);
        UpdateHealth(1f);
    }

    private void UpdateHealth(float newPercent) => _healthBar.fillAmount = newPercent;

    private void OnDestroy()
    {
        _health.OnLoseHealth.RemoveListener(UpdateHealth);
        _health.OnGainHealth.RemoveListener(UpdateHealth);
    }
}
