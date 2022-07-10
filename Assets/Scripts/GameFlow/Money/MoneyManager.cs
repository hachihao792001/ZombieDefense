using System;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField]
    private int _startMoney;
    [SerializeField]
    private int _currentMoney;
    public int CurrentMoney
    {
        get
        {
            return _currentMoney;
        }
        set
        {
            _currentMoney = value;
            OnMoneyChanged?.Invoke();
        }
    }

    public Action OnMoneyChanged;

    private void Start()
    {
        CurrentMoney = _startMoney;
    }

    public void EarnMoney(int amount)
    {
        CurrentMoney += amount;
    }

    public bool UseMoney(int amount)
    {
        if (amount <= CurrentMoney)
        {
            CurrentMoney -= amount;
            return true;
        }
        return false;
    }
}
