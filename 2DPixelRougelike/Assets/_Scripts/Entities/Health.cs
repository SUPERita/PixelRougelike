using DG.Tweening;
using Febucci.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public event Action OnDie;
    public event Action<int> OnTakeDamage;

    [SerializeField] private int baseHealth = 100;
    private int currentHealth = 0;

    [SerializeField] private bool showDmgText = false;
    [SerializeField] private GameObject DmgText = null;

    private void Awake()
    {
        currentHealth = baseHealth;
    }

    GameObject tmpTextObject = null;
    public void TakeDamage(int _amt)
    {
        //take damage
        currentHealth -= _amt;
        if(currentHealth < 0) { currentHealth = 0; }

        //invoke take damage
        OnTakeDamage?.Invoke(_amt);

        if (showDmgText)
        {
            SharedCanvas s = FindObjectOfType<SharedCanvas>();
            //change color with css html styling thing <color>
            tmpTextObject = Instantiate(DmgText, s.transform);
            tmpTextObject.transform.position = s._mainCameraRef.WorldToScreenPoint(transform.position) + Vector3.up*20;

            //WASSS ISSS DASSS?!?!?!?! IT WAS THiS EASY>?>?>>?> 
            tmpTextObject.GetComponent<TextMeshProUGUI>().SetText(""+_amt);
            //tmpTextObject.GetComponent<TextAnimator>().AppendText("" + $"<rainb>{_amt}</rainb>", false); // also need a text animator compounent

            //tmpTextObject.GetComponent<Rigidbody2D>().velocity = Vector2.right *3* UnityEngine.Random.Range(-1f, 1f) + Vector2.up*5;
            tmpTextObject.transform.DOScale(Vector3.one * 1.1f, .5f).SetEase(Ease.OutBounce)
                  .OnStart(()=> tmpTextObject.transform.DOMove(
                        tmpTextObject.transform.position + Vector3.one*50 * UnityEngine.Random.Range(-1f, 1f),
                        .5f));
            tmpTextObject.transform.DOScale(Vector3.zero, .4f).SetDelay(.5f);
            Destroy(tmpTextObject.gameObject, 1f);
            //tmpTextObject = null;
        }

        //invoke die
        if(currentHealth == 0) { OnDie?.Invoke(); }
        
    }


    public int GetBaseHealth()
    {
        return baseHealth;
    }
    public int GetCurrrentHealth()
    {
        return currentHealth;
    }
}
