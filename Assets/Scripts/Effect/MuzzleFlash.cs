using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField]
    private Transform muzzleFlash;
    [SerializeField]
    private GameObject flashLight;
    [SerializeField]
    private float duration;
    private float lastShowTime;

    private void OnEnable() => Hide();

    public void Show()
    {
        muzzleFlash.gameObject.SetActive(true);
        flashLight.SetActive(true);
        RotateMuzzle();
        lastShowTime = Time.time;
    }

    private void RotateMuzzle()
    {
        int angle = Random.Range(0, 360);
        muzzleFlash.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void Update()
    {
        if (!muzzleFlash.gameObject.activeSelf) return;
        if (Time.time - lastShowTime >= duration)
        {
            Hide();
        }
    }
    private void Hide()
    {
        muzzleFlash.gameObject.SetActive(false);
        flashLight.SetActive(false);
    }
}
