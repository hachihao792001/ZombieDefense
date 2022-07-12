using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyTextBinding : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    private TMP_Text _moneyText;
    [SerializeField]
    private MoneyManager _moneyManager;

    private void OnValidate()
    {
        _moneyText = GetComponent<TMP_Text>();
    }

    void Awake()
    {
        _moneyManager.OnMoneyChanged = () => _moneyText.text = "Money: $" + _moneyManager.CurrentMoney;
    }
}
