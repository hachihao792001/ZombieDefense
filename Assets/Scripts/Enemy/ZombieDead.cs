using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDead : MonoBehaviour
{
    [SerializeField]
    private ZombieMoving _zombieMoving;
    [SerializeField]
    private ZombieAttack _zombieAttack;
    [SerializeField]
    private GameObject _healthBarCanvas;
    public void OnZombieDead()
    {
        _zombieMoving.OnDied();
        _zombieAttack.OnDied();
        _healthBarCanvas.SetActive(false);

        Destroy(gameObject, 5f);
    }
}
