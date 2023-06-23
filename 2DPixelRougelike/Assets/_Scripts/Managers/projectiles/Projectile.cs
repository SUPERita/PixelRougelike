using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [field: SerializeField] public Rigidbody2D rb { get; private set; }

    private int damage = 0;
    private LayerMask collisionLayer;

    public void InitializeProjectile(int _damage, float velocity, Vector3 direction, LayerMask _collisionLayer, float projLifetime = 3f)
    {
        //Debug.Log("alive");
        damage = _damage;
        rb.velocity = direction * velocity;
        RotateSpriteToNormalizedDirection(direction);
        //transform.rotation = Quaternion.LookRotation(direction);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = q;

        collisionLayer = _collisionLayer;
        Invoke(nameof(KillProjectile), projLifetime);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if didnt collider with enemy layer, return
        if(((1 << collision.gameObject.layer) & collisionLayer) == 0 ){ return; }
        //if (collision.gameObject.layer.value != collisionLayer.value) { return; }
        if(collision.TryGetComponent(out IDamageable _d))
        {
            _d.TakeDamage(damage);
        }

        KillProjectile();
    }

    private void KillProjectile()
    {
        CancelInvoke(nameof(KillProjectile));
        // probably something with pooling
        if(gameObject == null) { return; }

        //Lean.Pool.LeanPool.Despawn(gameObject);
        LeanPoolManager.Instance.DespawnFromPool(gameObject);
        //Destroy(gameObject);
    }

    private void RotateSpriteToNormalizedDirection(Vector3 direction)
    {
        Vector3 desiredDirection = direction.z * Vector3.forward;
        if(desiredDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(desiredDirection);
            transform.rotation = desiredRotation;
        }
        
    }
}
