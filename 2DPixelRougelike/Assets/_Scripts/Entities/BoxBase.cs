using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains;
using MoreMountains.Feedbacks;
using DG.Tweening;

public class BoxBase : MonoBehaviour, IDamageable
{
    [SerializeField] private Health health;
    [SerializeField] private MMF_Player hitFeedback;
    [SerializeField] private Transform follow = null;
    [SerializeField] private Rigidbody2D rb = null;
    [SerializeField] private GameObject particleSystem = null;
    public void TakeDamage(int _val)
    {
        AudioSystem.Instance.PlaySound("s3");
        hitFeedback?.PlayFeedbacks();
        health.TakeDamage(_val);
        
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health.OnDie += Health_OnDie;
    }
    private void OnDisable()
    {
        //also goes off if you just disable the game object
        health.OnDie -= Health_OnDie;
    }

    private void Health_OnDie()
    {
        Instantiate(particleSystem, transform.position, Quaternion.identity);
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = Vector3.zero;
        //Destroy(gameObject);
        transform.DOScale(0f, 1f).SetEase(Ease.InOutBounce).OnComplete(() => Destroy(gameObject));
    }

    private void FixedUpdate()
    {
        rb.velocity = -(transform.position - follow.position).normalized * 5f;
    }
}
