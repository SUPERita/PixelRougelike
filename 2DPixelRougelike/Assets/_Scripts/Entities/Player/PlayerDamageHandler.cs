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
    public bool isAlive { get; private set; } = true;


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

        //try dodge
        if (Helpers.RollChance(
            Mathf.Min(PlayerStatsHolder.Instance.TryGetStat(StatType.Dodge),
            40f)))
        {
            //MessageBoard.Instance.SpawnMessage("dodged");
        } 
        //take baseDamage
        else
        {
            health.TakeDamage((int)
                (_hurter.GetDamage() * 
                (100f-Mathf.Min(PlayerStatsHolder.Instance.TryGetStat(StatType.Armor),40f))/100f
                ));
            hitFeedback?.PlayFeedbacks();
            AudioSystem.Instance.PlaySound("player_hurt", .5f, 1+Random.value/10f);
        }

        timeUntilHitable = Time.time + invincibilityTime;
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

        AudioSystem.Instance.PlaySound("player_die", .5f);
        GameStateManager.Instance.SetState(GameState.Dead);
        StartCoroutine(Helpers.DoInTime(()=> DeathPanel.Instance.OpenDeathPanel(), 1));
        
        //Invoke(nameof(LoadLobby), 3);
    }

    

}
