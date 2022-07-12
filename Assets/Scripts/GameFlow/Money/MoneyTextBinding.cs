using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

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
        _moneyManager.OnMoneyChanged = () =>
        {
            _moneyText.text = "Money: $" + _moneyManager.CurrentMoney;
            transform.DOScale(Vector3.one * 1.2f, 0.2f).OnComplete(() =>
            {
                transform.DOScale(Vector3.one, 0.2f);
            });
        };
    }
}
