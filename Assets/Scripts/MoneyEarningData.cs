using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoneyEarningData", menuName = "ScriptableObjects/MoneyEarningData", order = 2)]
public class MoneyEarningData : ScriptableObject
{
    public int KillingZombie;
    public int KillingFastZombie;
    public int Finish1Round;
}
