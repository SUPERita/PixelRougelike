using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
            //change color with css html styling thing <color>
            tmpTextObject = Instantiate(DmgText, transform.position, Quaternion.identity);
            tmpTextObject.GetComponentInChildren<TextMeshProUGUI>().SetText("" + _amt);
            tmpTextObject.GetComponent<Rigidbody2D>().velocity = Vector2.right *3* UnityEngine.Random.Range(-1f, 1f) + Vector2.up*5;
            tmpTextObject.transform.DOScale(Vector3.zero, .5f).SetDelay(1f);
            Destroy(tmpTextObject.gameObject, 2f);
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
