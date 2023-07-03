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
        health.OnHealthChanged += Health_OnHealthChanged;
    }


    private void OnDisable()
    {
        health.OnTakeDamage -= Health_OnTakeDamage;
        health.OnDie -= Health_OnDie;
        health.OnHealthChanged -= Health_OnHealthChanged;
    }

    private void Health_OnDie()
    {
        healthCanvas.DOFade(0, .5f);
    }
    private void Health_OnTakeDamage(int obj)
    {
        healthImage.fillAmount = (float)health.GetCurrrentHealth() / health.GetBaseHealth();
        afterImageTo = healthImage.fillAmount;

        LerpAfterImage();

    }
    private void Health_OnHealthChanged(int obj)
    {
        healthImage.fillAmount = (float)health.GetCurrrentHealth() / health.GetBaseHealth();

        if (healthImage.fillAmount != afterImageTo)
        {
            LerpAfterImage();
        }
        
        afterImageTo = healthImage.fillAmount;
    }

    private float afterImageTo = 1f;
    Tweener _t = null;
    private void LerpAfterImage()
    {
        if (!afterImage) return;

        Debug.Log("lerping");
        _t.Kill();
        _t = afterImage.DOFillAmount(healthImage.fillAmount, .25f).SetDelay(.5f)
            .OnComplete(()=> _t = null);
    }
}
