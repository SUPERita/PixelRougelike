using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDamageHandler : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private float invincibilityTime = .25f;
    private float timeUntilHitable = 0f;
    [SerializeField] private MMF_Player hitFeedback;
    [Header("On Die")]
    [SerializeField] private PlayerMovement pm;
    [SerializeField] private MonoBehaviour[] disableOnDie;
    [SerializeField] private GameObject[] deactivateOnDie;
    private bool isAlive = true;


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IHurtPlayer _hurter))
        {
            GetHit(_hurter);
        }
    }
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.TryGetComponent(out IHurtPlayer _hurter))
    //    {
    //        GetHit(_hurter);
    //    }
    //}

    private void GetHit(IHurtPlayer _hurter)
    {
        //timer stuff
        if (!IsHitable() || !isAlive) { return; }
        timeUntilHitable = Time.time + invincibilityTime;
        //take damage
        health.TakeDamage(_hurter.GetDamage());
        hitFeedback?.PlayFeedbacks();
        AudioSystem.Instance.PlaySound("s2");
    }

    private bool IsHitable()
    {
        return timeUntilHitable <= Time.time;
    }

    private void Start()
    {
        health.OnDie += Health_OnDie;   
    }
    private void OnDestroy()
    {
        health.OnDie -= Health_OnDie;
    }

    private void Health_OnDie()
    {
        isAlive = false;
        pm.OnDie();
        foreach (var item in deactivateOnDie)
        {
            item.SetActive(false);
        }
        foreach (var item in disableOnDie)
        {
            item.enabled = false;
        }

        StartCoroutine(Helpers.DoInTime(()=> DeathPanel.Instance.OpenDeathPanel(), 1));
        
        //Invoke(nameof(LoadLobby), 3);
    }

    

}
