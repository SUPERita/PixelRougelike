using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Projectile projPrefab;
    Vector3 _weaponPosition = Vector3.zero;
    [Header("design")]
    [SerializeField] private float range = 10f;
    [SerializeField] private int damage = 2;
    [SerializeField] private float reloadSpeed = 1f;
    private float reloadTimer = 0f;  
    [SerializeField] private float projSpeed = 10f;
    [SerializeField] private Transform gunTip = null;

    [Header("technical")]
    [SerializeField] private LayerMask scanLayer;
    [SerializeField] private bool showRange = false;
    [SerializeField] private float extraRotface = 0f;
    //[SerializeField] private int stressTest = 1;
    [Header("explosives")]
    [SerializeField] bool isExplosive = false;
    [ShowIf("@isExplosive"),SerializeField] float explosionSize = 3f;
    [Header("Pattern")]
    [SerializeField] int numProjectiles = 1;
    [SerializeField] float spread = 0;
    //[SerializeField] float explosionSize = 3f;

    private Collider2D[] _enemiesInRange = new Collider2D[100];//possible problem with only 100 slots
    Transform target = null;//closest enemy

    private float scanTimer = 0;

    void Update()
    {
        //can only call the scanning method every couple of frames
        //for (int i = 0; i < stressTest; i++)
        //{
        //    ScanEnemiesInRange();
        //}


        //handle timers
        scanTimer -= Time.deltaTime;
        reloadTimer -= Time.deltaTime;

        //recoil animation
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, 10f*Time.deltaTime);

        //try scan
        if (scanTimer < 0)
        {
            ScanEnemiesInRange();
            scanTimer = .2f;
        }

        if(target == null) { return; }

        //try shoot
        if (reloadTimer < 0)
        {
            reloadTimer = reloadSpeed / (PlayerStatsHolder.Instance.TryGetStat(StatType.WeaponAttackSpeed) / 100f);
            Shoot();
            //recoil anim
            transform.localPosition = /*Vector3.left*/-transform.right;
        }

        //face target
        FaceTarget();

    }




    GameObject _g;

    public virtual void Shoot()
    {
        for (int i = 0; i < numProjectiles; i++)
        {
            //probably gonna need to use pooling in the future
            _g = LeanPoolManager.Instance.SpawnFromPool("proj1");
            //Instantiate(projPrefab, gunTip.position, transform.rotation)
            _g.transform.SetPositionAndRotation(gunTip.position, transform.rotation);
            _g.transform.localScale = Vector3.right * .4f + Vector3.up * .1f + Vector3.forward;
            _g.GetComponent<Projectile>().InitializeProjectile(
              damage + PlayerStatsHolder.Instance.TryGetStat(StatType.WeaponDamage/*+ PlayerStatsHolder.Instance.TryGetStat(StatType.Strength)*/),
              projSpeed,
              ((target.position+Vector3.up+(spread * (Vector3) UnityEngine.Random.insideUnitCircle/*insideUnitSphere*/) ) - (transform.position)).normalized,
              scanLayer);

        }
    }

    //utils
    private void FaceTarget()
    {
        //transform.LookAt(target.position, Vector3.forward);

        Vector3 vectorToTarget = target.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - (extraRotface+45f);
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = q;
    }
    private void ScanEnemiesInRange()
    {
        //works
        _weaponPosition = transform.position;
        int numColliders = Physics2D.OverlapCircleNonAlloc(_weaponPosition, range, _enemiesInRange, scanLayer); // the int is important // works

        //get the closest one
        float minDis = Mathf.Infinity;
        int closestIndex = -1;
        target = null;
        for (int i = 0; i < numColliders; i++)
        {
            float crntDis = (_weaponPosition - _enemiesInRange[i].transform.position).sqrMagnitude;
            if (crntDis < minDis)
            {
                minDis = crntDis;
                closestIndex = i;
            }
        }

        target = closestIndex == -1? null : _enemiesInRange[closestIndex].transform;

    }
    private void OnDrawGizmos()
    {
        if (!showRange) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public string GetDescription()
    {
        string _out = "";
        _out = "reload: " + reloadSpeed + "\n";
        _out += "damage: " + damage + "\n";
        _out += "range: " + range + "\n";
        return _out;
    }

}
