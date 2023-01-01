using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuickShopController : MonoBehaviour
{
    [SerializeField]
    private MoneyManager _moneyManager;

    [SerializeField]
    private int _ammoPrice;
    [SerializeField]
    private int _grenadePrice;
    [SerializeField]
    private int _turretPrice;

    [SerializeField]
    private Transform _ammoItem;
    [SerializeField]
    private Transform _grenadeItem;
    [SerializeField]
    private Transform _turretItem;

    [SerializeField]
    private TMP_Text _ammoPriceText;
    [SerializeField]
    private TMP_Text _grenadePriceText;
    [SerializeField]
    private TMP_Text _turretPriceText;

    void Start()
    {
        RefreshEnoughMoney();

        _moneyManager.OnMoneyChanged += RefreshEnoughMoney;
    }
    private void OnDestroy()
    {
        _moneyManager.OnMoneyChanged -= RefreshEnoughMoney;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BuyItem(_ammoPrice, () =>
            {
                GameController.Instance.Player.PlayerAmmoRefiller.RefillAmmo();
                _ammoItem.DOScale(Vector3.one * 1.2f, 0.2f).OnComplete(() =>
                {
                    _ammoItem.DOScale(Vector3.one, 0.2f);
                });
            });
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BuyItem(_grenadePrice, () =>
            {
                GameController.Instance.Player.GrenadeThrowing.RefillGrenade();
                _grenadeItem.DOScale(Vector3.one * 1.2f, 0.2f).OnComplete(() =>
                {
                    _grenadeItem.DOScale(Vector3.one, 0.2f);
                });
            });
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BuyItem(_turretPrice, () =>
            {
                GameController.Instance.Player.PlayerTurretPlacer.PlaceNewTurret();
                _turretItem.DOScale(Vector3.one * 1.2f, 0.2f).OnComplete(() =>
                {
                    _turretItem.DOScale(Vector3.one, 0.2f);
                });
            });

        }
    }
    private void BuyItem(int price, Action successAction)
    {
        if (_moneyManager.UseMoney(price))
        {
            successAction?.Invoke();
        }
        RefreshEnoughMoney();
    }

    void RefreshEnoughMoney()
    {
        _ammoPriceText.color = _moneyManager.CurrentMoney >= _ammoPrice ? Color.green : Color.red;
        _grenadePriceText.color = _moneyManager.CurrentMoney >= _grenadePrice ? Color.green : Color.red;
        _turretPriceText.color = _moneyManager.CurrentMoney >= _turretPrice ? Color.green : Color.red;
    }
}
