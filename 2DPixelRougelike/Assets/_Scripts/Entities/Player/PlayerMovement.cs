using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IAnimationEventsReciever
{
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private int strength = 10;
    [Tooltip("Attacks Per Second, attack animation time should be 1sec")]
    [SerializeField] private float attackSpeed = 1f;
    Vector2 movement = new Vector2(0f, 0f);
    Vector2 lastMovement = new Vector2 (0f,0f);
    Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private AttackTriggerCollider attackTriggerCollider;
    [SerializeField] private ParticleSystem rollParticle = null;
    [SerializeField] private Transform playerVisual = null;
    float speedMult = 1f;
    float speedMultRemainingTime = 0f;

    [SerializeField] private PlayerStats playerStats;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    IDamageable[] enemiesInArea = null;
    private void DamageArea()
    {
        strength = playerStats.GetStat("strength");
        enemiesInArea = attackTriggerCollider.GetDamageablesInArea();
        foreach(IDamageable _i in enemiesInArea)
        {
            _i.TakeDamage(strength);
        }
    }

    private void Update()
    {
        //in menu
        if (GameStateManager.Instance.GetCurrentGameState() != GameState.GameLoop) { return; }


        //attack speed animator value
        animator.SetFloat("AttackSpeed", attackSpeed);

        //speed multiplayer//rolling
        if (speedMultRemainingTime > 0f)
        {
            rollParticle.enableEmission = true;
            speedMultRemainingTime -= Time.deltaTime;
        }
        else
        {
            rollParticle.enableEmission = false;
            speedMult = 1f;
        }

        HandleAnimation();

    }
    private void FixedUpdate()
    {
        rb.velocity = movement.normalized * playerSpeed * speedMult;
    }

    //utils
    private void HandleAnimation()
    {

        //rotation
        playerVisual.rotation = lastMovement.x > 0.02f ? Quaternion.Euler(0f, 0f, 0f) : Quaternion.Euler(0f, 180f, 0f);

        //animator variables
        animator.SetFloat("Movement", movement.sqrMagnitude > 0? 1 : 0);

        //animation
        if (speedMultRemainingTime > 0)
        {
            PlayAnim("Roll");
            return;
        }

        if(attackTriggerCollider.IsDamageablesInArea())
        {
            PlayAnim("Attack1");
            return;
        }

        if (movement.sqrMagnitude > 0.02f)
        {
            PlayAnim("Move");
            return;
        }

        PlayAnim("Idle");
        return;

    }
    private void PlayAnim(string _animName)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(_animName))
        {
            //Debug.Log("names dont match");
            animator.CrossFadeInFixedTime(_animName, 0);
        }
    }

    //input
    public void OnMove(InputValue _value)
    {
        //in menu
        if (GameStateManager.Instance.GetCurrentGameState() != GameState.GameLoop) { movement = Vector2.zero; return; }

        movement = _value.Get<Vector2>();

        if (Mathf.Abs(movement.x) > 0.02f)
        {
            lastMovement.x = movement.x;
        }
        if (Mathf.Abs(movement.y) > 0.02f)
        {
            lastMovement.y = movement.y;
        }
    }
    public void OnFire(InputValue _value)
    {
        //in menu
        if (GameStateManager.Instance.GetCurrentGameState() != GameState.GameLoop) { return; }

        //Debug.Log(_value);
        speedMult = 5;
        speedMultRemainingTime = .15f;
        //revert back in a second, but 

        //works
        //foreach(PickUp _p in FindObjectsOfType<PickUp>())
        //{
        //    _p.StartFollowing(transform);
        //}
    }

    //interface stuff
    public void OnEventCalled(string _eventName)
    {
        if (_eventName.Equals("Attack1"))
        {
            DamageArea();
        }
    }


    public void OnDie()
    {
        movement = Vector2.zero;
        PlayAnim("Idle");
    }

}
