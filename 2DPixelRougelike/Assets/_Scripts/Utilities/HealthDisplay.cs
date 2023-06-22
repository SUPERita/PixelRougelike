using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private CanvasGroup healthCanvas;
    [SerializeField] private Image healthImage;
    [SerializeField] private GameObject healthBarRoot;
    [Header("optional")]
    [SerializeField] private Image afterImage;

    private void Awake()
    {
        healthImage.fillAmount = 1f;
        health.OnTakeDamage += Health_OnTakeDamage;
        health.OnDie += Health_OnDie;
    }

    private void OnDisable()
    {
        health.OnTakeDamage -= Health_OnTakeDamage;
        health.OnDie -= Health_OnDie;
    }

    private void Health_OnDie()
    {
        healthCanvas.DOFade(0, .5f);
    }

    private void Health_OnTakeDamage(int obj)
    {
        healthImage.fillAmount = (float)health.GetCurrrentHealth() / (float)health.GetBaseHealth();

        if(afterImage) LerpAfterImage();

    }

    Tween _t = null;
    private void LerpAfterImage()
    {
        _t.Kill();
        _t = afterImage.DOFillAmount(healthImage.fillAmount, .25f).SetDelay(.5f);
    }
}
