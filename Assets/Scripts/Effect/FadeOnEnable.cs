using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOnEnable : MonoBehaviour
{
    [SerializeField]
    private Image _image;
    [SerializeField]
    private float _fadeSpeed;

    private void OnEnable()
    {
        _image.color = Color.white;
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        while (_image.color.a > 0)
        {
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a - _fadeSpeed * Time.deltaTime);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
