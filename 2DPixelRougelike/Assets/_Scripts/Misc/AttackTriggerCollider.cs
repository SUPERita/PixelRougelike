using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
//using static UnityEditor.Progress;

public class AttackTriggerCollider : MonoBehaviour
{
    private List<IDamageable> damageables = new List<IDamageable>();
    [SerializeField] private float range = 4f;
    [SerializeField] private int enemiesToHit = 1;
    [SerializeField] private LayerMask scanLayer;
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
    private Transform _transform = null;
    private void Start()
    {
        _transform = transform;
    }


    private void Update()
    {
        ScanEnemiesInRange();
    }

    private Collider2D[] _enemiesInRange = new Collider2D[100];
    List<Collider2D> _enemyColliders = new List<Collider2D>();
    private void ScanEnemiesInRange()
    {
        damageables.Clear();
        _enemyColliders.Clear();
        //works
        //gets all the collider
        int numColliders = Physics2D.OverlapCircleNonAlloc(transform.position, range, _enemiesInRange, scanLayer); // the int is important // works

        //create a list of all the enemy Colliders
        for (int i = 0; i < numColliders; i++)
        {
            if (_enemiesInRange[i].TryGetComponent(out IDamageable _idamageable))
            {
                _enemyColliders.Add(_enemiesInRange[i]);
            }
        }

        //get the closest enemy
        //SortDamageables(); // isnt always needed

    }
    private void SortDamageables()
    {
        for (int i = 0; i < enemiesToHit; i++)
        {
            float _minDis = Mathf.Infinity;
            Collider2D _closeCol = null;
            foreach (Collider2D _c in _enemyColliders)
            {
                if ((_c.transform.position - _transform.position).sqrMagnitude < _minDis)
                {
                    _minDis = (_c.transform.position - _transform.position).sqrMagnitude;
                    _closeCol = _c;
                }
            }

            if (_closeCol == null) continue;
            damageables.Add(_closeCol.GetComponent<IDamageable>());
            _enemyColliders.Remove(_closeCol);
        }
    }


    public IDamageable[] GetDamageablesInArea()
    {
        SortDamageables();
        return damageables.ToArray();
    }
    public bool IsDamageablesInArea()
    {
        return _enemyColliders.Count != 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
