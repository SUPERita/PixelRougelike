using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTriggerCollider : MonoBehaviour
{
    private List<IDamageable> damageables = new List<IDamageable>();
    //public 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IDamageable _idamageable))
        {
            damageables.Add(_idamageable);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable _idamageable))
        {
            damageables.Remove(_idamageable);
        }
    }
     
    public IDamageable[] GetDamageablesInArea()
    {
        return damageables.ToArray();
    }
    public bool IsDamageablesInArea()
    {
        return damageables.Count != 0;
    }
}
