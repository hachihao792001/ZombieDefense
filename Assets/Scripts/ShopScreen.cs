using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ShopScreen : MonoBehaviour
{
    [SerializeField]
    private MoneyManager _moneyManager;

    [SerializeField]
    private TMP_Text _healPriceText;
    [SerializeField]
    private TMP_Text _ammoPriceText;
    [SerializeField]
    private TMP_Text _healRVPriceText;
    [SerializeField]
    private TMP_Text _turretPriceText;
    [SerializeField]
    private TMP_Text _currentMoneyText;


    [SerializeField]
    private int _healPrice;
    [SerializeField]
    private int _ammoPrice;
    [SerializeField]
    private int _healRVPrice;
    [SerializeField]
    private int _turretPrice;

    [SerializeField]
    private float _healAmount;
    [SerializeField]
    private float _healRVAmount;

    public UnityEvent<float> OnBuyHeal;
    public UnityEvent OnBuyAmmo;
    public UnityEvent<float> OnBuyHealRV;
    public UnityEvent OnBuyTurret;

    private void OnEnable()
    {
        _healPriceText.text = "$" + _healPrice;
        _ammoPriceText.text = "$" + _ammoPrice;
        _healRVPriceText.text = "$" + _healRVPrice;
        _turretPriceText.text = "$" + _turretPrice;

        _currentMoneyText.text = "$" + _moneyManager.CurrentMoney;
    }

    public void HealOnClick()
    {
        BuyItem(_healPrice, () =>
        {
            OnBuyHeal?.Invoke(_healAmount);
            _currentMoneyText.text = "$" + _moneyManager.CurrentMoney;
        });
    }

    public void AmmoOnClick()
    {
        BuyItem(_ammoPrice, () =>
        {
            OnBuyAmmo?.Invoke();
            _currentMoneyText.text = "$" + _moneyManager.CurrentMoney;
        });
    }

    public void HealRVOnClick()
    {
        BuyItem(_healRVPrice, () =>
        {
            OnBuyHealRV?.Invoke(_healRVAmount);
            _currentMoneyText.text = "$" + _moneyManager.CurrentMoney;
        });
    }

    public void TurretOnClick()
    {
        BuyItem(_turretPrice, () =>
        {
            OnBuyTurret?.Invoke();
            _currentMoneyText.text = "$" + _moneyManager.CurrentMoney;
        });
    }

    private void BuyItem(int price, Action successAction)
    {
        if (_moneyManager.UseMoney(price))
        {
            successAction?.Invoke();
        }
    }
}
