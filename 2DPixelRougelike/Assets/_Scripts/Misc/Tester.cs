using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Drawing;
using UnityEngine.Rendering;

public class Tester : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private bool compileEveryFrame = false;
    [SerializeField] private int times = 1;
    [SerializeField] private float num = 10f;
    [SerializeField] private LayerMask layer;
    [Button]
    public void Compilestats()
    {
        for (int i = 0; i < times; i++)
        {
            stats.TEST_GetCompiledStats();
        }
    }

    private Collider2D[] _out = new Collider2D[100];//possible problem
    Vector3 position = Vector3.zero;
    private void Update()
    {
        
        if (compileEveryFrame)
        {
            for (int i = 0; i < times; i++)
            {
                //better, less time, non aloc.
                //Physics2D.OverlapCircleAll(transform.position, 10f);
                //works
                position = transform.position;
                int numColliders = Physics2D.OverlapCircleNonAlloc(position, num, _out); // the int is important // works

                //TODO - loop through em
                //Debug.Log(numColliders);
                float minDis = Mathf.Infinity;
                //string closestName = "N/A";
                int closestIndex = 0;   
                for (int j = 0; j < numColliders-1; j++)
                {
                    float crntDis = (position - _out[j].transform.position).sqrMagnitude;
                    if (crntDis < minDis)
                    {
                        minDis = crntDis;
                        //closestName = _out[j].name; 
                        closestIndex = j;
                    }
                }
                //Debug.Log(numColliders + " of is closest  " +closestName + " " + minDis);
                //Debug.DrawLine(_out[closestIndex].transform.position, transform.position);

                //Debug.Log(_out.Length + " :-: " + numColliders);//works
                //Physics2D.OverlapCircle(transform.position, value);//works
                //Debug.Log(Physics2D.OverlapCircleAll(transform.position, value).Length);//works
            }
        }
        return;


        //if (compileEveryFrame)
        //{
        //    for (int i = 0; i < times; i++)
        //    {
        //        ResourceSystem.Instance.AddResourceAmount(ResourceType.EnergyNugget, 1);
        //    }
        //    return;
        //}



        //if (compileEveryFrame)
        //{
        //    for (int i = 0; i < times; i++)
        //    {
        //        //stats.TEST_GetCompiledStats();
        //        stats.GetStat("speed");
        //    }
        //}
        //return;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, num);
    }

    [Button]

    private void PlayChildSrouce()
    {
        GetComponentInChildren<AudioSource>().Play();
    }
}
