using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

public class AttackTriggerCollider : MonoBehaviour
{
    private List<IDamageable> damageables = new List<IDamageable>();
    [SerializeField] private float range = 4f;
    //public 
    /*
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
    */
    private void Update()
    {
        ScanEnemiesInRange();
    }
    private Collider2D[] _enemiesInRange = new Collider2D[100];
    private void ScanEnemiesInRange()
    {
        damageables.Clear();
        //works
        int numColliders = Physics2D.OverlapCircleNonAlloc(transform.position, range, _enemiesInRange); // the int is important // works

        for (int i = 0; i < numColliders; i++)
        {
            if (_enemiesInRange[i].TryGetComponent(out IDamageable _idamageable))
            {
                damageables.Add(_idamageable);
            }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
