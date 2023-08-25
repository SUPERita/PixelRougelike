using DG.Tweening;
using Febucci.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Health : MonoBehaviour
{
    public event Action OnDie;
    public event Action<int> OnTakeDamage;
    public event Action<int> OnHealthChanged;
    [SerializeField] private int maxHealth = 100;
    private int currentHealth = 0;

    [SerializeField] private bool showDmgText = false;
    //[SerializeField] private DamageText DmgTextPrefab = null;
    [SerializeField] private Color dmgTextColor = Color.white;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    Transform tmpTextObject = null;
    public void TakeDamage(int _amt)
    {
        if(currentHealth == 0) { return; }

        //take baseDamage
        currentHealth -= _amt;
        if(currentHealth < 0) { currentHealth = 0; }

        //invoke take baseDamage
        OnTakeDamage?.Invoke(_amt);

        //spawn baseDamage numver
        if (showDmgText && SettingsCanvas.Instance.showDamageNumbers/*future removeable*/)
        {
            SharedCanvas s = SharedCanvas.Instance;

            //no pooling // build up kinda fast?!// in task manager ram build up around 20-40 times faster than pooling
            //tmpTextObject = InstantiateDamageNumber(s.transform);
            //Destroy(tmpTextObject.gameObject, 1f);//also this one, fuckton of performance
            //yes pooling
            tmpTextObject = LeanPoolManager.Instance.SpawnFromPool("dmgText").transform; // GetPoolObject(s.transform); // in task manager ram build up really slow
            LeanPoolManager.Instance.DespawnFromPool(tmpTextObject.gameObject, 1);//tmpTextObject.gameObject.GetComponent<DamageText>().CallReleaseToPool(1f);
            tmpTextObject.SetParent(s.transform);



            tmpTextObject.position = transform.position;//s._mainCameraRef.WorldToScreenPoint(transform.position) + Vector3.up*50;

            tmpTextObject.GetComponent<TextMeshProUGUI>().SetText(_amt.ToString()); //dmgTextColor
            tmpTextObject.GetComponent<TextMeshProUGUI>().color = dmgTextColor;
            //WASSS ISSS DASSS?!?!?!?! IT WAS THiS EASY>?>?>>?> 
            //tmpTextObject.GetComponent<TextAnimator>().AppendText("" + $"<rainb>{_amt}</rainb>", false); // also need a text animator compounent
            tmpTextObject.gameObject.GetComponent<DamageText>().DOStartTween();
            /*
            tmpTextObject.DOScale(Vector3.one * 1.1f, .5f).SetEase(Ease.OutBounce)
                  .OnStart(() => tmpTextObject.DOMove(
                        tmpTextObject.position + ((Vector3.up * UnityEngine.Random.Range(-1f, 1f)) + (Vector3.right * UnityEngine.Random.Range(-1f, 1f))) * 2f,
                       .5f));
            tmpTextObject.DOScale(Vector3.zero, .4f).SetDelay(.5f);
            */

        }

        //invoke die
        if (currentHealth == 0) { OnDie?.Invoke(); }

    }
    public void HealHealth(int _amt)
    {
        if (currentHealth == 0) { return; }
        if(currentHealth == maxHealth) { return; }
        if(_amt <= 0) { return; }

        //take baseDamage
        currentHealth += _amt;
        if (currentHealth > maxHealth) { currentHealth = maxHealth; }

        //invoke take baseDamage
        OnHealthChanged?.Invoke(_amt);


        //spawn baseDamage numver
        if (showDmgText && SettingsCanvas.Instance.showDamageNumbers/*future removeable*/)
        {
            SharedCanvas s = SharedCanvas.Instance;

            tmpTextObject = LeanPoolManager.Instance.SpawnFromPool("dmgText").transform; // GetPoolObject(s.transform); // in task manager ram build up really slow
            LeanPoolManager.Instance.DespawnFromPool(tmpTextObject.gameObject, 1);//tmpTextObject.gameObject.GetComponent<DamageText>().CallReleaseToPool(1f);
            tmpTextObject.SetParent(s.transform);

            tmpTextObject.position = transform.position;
            tmpTextObject.GetComponent<TextMeshProUGUI>().SetText(_amt.ToString());
            tmpTextObject.GetComponent<TextMeshProUGUI>().color = Color.green;
            //?
            tmpTextObject.gameObject.GetComponent<DamageText>().DOStartTween(false);


        }
    }
    /*
    private Transform InstantiateDamageNumber(Transform _parent)
    {
        

        // dont override the awake staticInstance awake method without thinking
        //these two shits take a fuckton of performance

        //v
        return (Instantiate(DmgTextPrefab, _parent).transform);
        //^
        //fixed




    }
    private Transform GetPoolObject(Transform _parent)
    {
        //remmember to set the parent
        Transform _t = PoolManager.Instance.dmgTextPool.Get().transform;
        _t.SetParent( _parent );
        return _t;

    }
    */

    public int GetBaseHealth()
    {
        return maxHealth;
    }
    public int GetCurrrentHealth()
    {
        return currentHealth;
    }

    public void ResetHealth() => currentHealth = maxHealth;
    public void SetMaxHealth(int _arg, bool _heal = false)
    {
        if( _arg <= 0) { OnDie?.Invoke(); }
        
        maxHealth = _arg;
        if(currentHealth > maxHealth) currentHealth = maxHealth;
        if(_heal) currentHealth = maxHealth;
        OnHealthChanged?.Invoke(_arg);
    }

    public bool IsFullHealth()
    {
        return currentHealth == maxHealth;
    }
}
