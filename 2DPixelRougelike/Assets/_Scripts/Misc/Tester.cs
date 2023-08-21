using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Drawing;
using UnityEngine.Rendering;

public class Tester : StaticInstance<Tester>
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

    [Button]
    private void AddItemToPlayer(Item _item)
    {
        Shop.Instance.GiveItem(_item);
    }

    [SerializeField] private bool showShopData = true;
    [ShowInInspector, ShowIf("showShopData")] private int commons = 0;
    [ShowInInspector, ShowIf("showShopData")] private int uncommons = 0;
    [ShowInInspector, ShowIf("showShopData")] private int rares = 0;
    [ShowInInspector, ShowIf("showShopData")] private int epics = 0;
    [ShowInInspector, ShowIf("showShopData")] private int legendaries = 0;
    public void OfferedItem(Item _item)
    {
        switch (_item.itemRarity) {
            case ItemRarity.Common:
                commons++;
                break;
            case ItemRarity.Uncommon:
                uncommons++;
                break;
            case ItemRarity.Rare:
                rares++;
                break;
            case ItemRarity.Epic:
                epics++;
                break;
            case ItemRarity.Legendary:
                legendaries++;
                break;
        }

        PrintPrecentages();

    }
    [SerializeField, TextArea(10, 10), ShowIf("showShopData")] private string _result = "";
    private void PrintPrecentages()
    {
        float sum = commons + uncommons + rares + epics + legendaries;
        string _out = "";//make them show the right number of I's
        _out += new string('I', (int)(100f * commons / sum)) + " commons % is: " + 100f * commons / sum + "%\n";
        _out += new string('I', (int)(100f * uncommons / sum)) + " uncommons % is: " + 100f * uncommons / sum + "%\n";
        _out += new string('I', (int)(100f * rares / sum)) + " rares % is: " + 100f * rares / sum + "%\n";
        _out += new string('I', (int)(100f * epics / sum)) + " epics % is: " + 100f * epics / sum + "%\n";
        _out += new string('I', (int)(100f * legendaries / sum)) + " legendaries % is: " + 100f * legendaries / sum + "%\n";
        _result = _out;//Debug.Log(_out);
    }

}
