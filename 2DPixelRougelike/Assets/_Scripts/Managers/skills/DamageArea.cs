using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [field: SerializeField] public Rigidbody2D rb { get; private set; }
    [SerializeField] private Collider2D collider;
    private bool dieOnHit = false;

    private int damage = 0;
    private LayerMask collisionLayer;

    public void InitializeArea(int _damage, Vector3 directionAndMagnitude, LayerMask _collisionLayer, float _lifetime = 5f, float size = 1f, bool _dieOnHit = false)
    {
        //Debug.Log("alive");
        damage = _damage;
        rb.velocity = directionAndMagnitude;
        //RotateSpriteToNormalizedDirection(direction);
        transform.localScale = Vector3.one * size;
        dieOnHit = _dieOnHit;
        //transform.rotation = Quaternion.LookRotation(direction);

        float angle = Mathf.Atan2(directionAndMagnitude.y, directionAndMagnitude.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = q;

        collisionLayer = _collisionLayer;
        Invoke(nameof(DestroyArea), _lifetime);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if didnt collider with enemy layer, return
        if (((1 << collision.gameObject.layer) & collisionLayer) == 0) { return; }
        //if (collision.gameObject.layer.value != collisionLayer.value) { return; }
        if (collision.TryGetComponent(out IDamageable _d))
        {
            _d.TakeDamage(damage);
        }


        if(dieOnHit) DestroyArea();
    }

    private void DestroyArea()
    {
        // probably something with pooling
        if (gameObject == null) { return; }

        collider.enabled = false;
        rb.velocity = Vector2.zero;
        transform.DOScale(0, .25f).OnComplete(()=> Destroy(gameObject));
        
    }

    private void RotateSpriteToNormalizedDirection(Vector3 direction)
    {
        Vector3 desiredDirection = direction.z * Vector3.forward;
        if (desiredDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(desiredDirection);
            transform.rotation = desiredRotation;
        }

    }
}
