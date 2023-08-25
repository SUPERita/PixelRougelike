using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Drawing;
using UnityEngine.Rendering;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System;

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

    [Button]
    private void PrintDoesHaveDuplicateEmails()
    {
        //tell gpt to put a , between every email
        string _S = "itamarhefetzsahar@gmail.com,itamarhefetzsahar@gmail.com,millardadamwork@gmail.com,albinomanagement@gmail.com,ambiguousamphibian@gmail.com,asmongoldtv.catdany@gmail.com,BaronVonGames@screenwavemedia.com,BibleEvolved@gmail.com,sleepingburr@gmail.com,christopherodd@screenwavemedia.com,Cookinghippo1@gmail.com,dfbusinessinquiries@gmail.com,itsdexgaming@gmail.com,silverflyingdragon@yahoo.gr,realcivilengineergaming@gmail.com,francil-manager@mail.ru,Intern@Blitztopia.com,ivanushkinav87@gmail.com,callmekevinbusiness@gmail.com,splattercatgaming@gmail.com,indeimaus@gmail.com,michaelberg265@gmail.com,NeverNathanielYT@gmail.com,bitw2423@gmail.com,olexakid@gmail.com,Jawlesspaul@gmail.com,FQpdcpublic223@gmail.com,volatileexplosion@gmail.com,blacksmyth33@gmail.com,retromation750@gmail.com,rndthursday@gmail.com,northernlionbusiness@gmail.com,sean@s-tier.com,det.c.kawakami@gmail.com,anacondaz1001@gmail.com,cyrkitgames@gmail.com,sheeron601@gmail.com,FuryForgedBiz@gmail.com,calmlittlebuddy@gmail.com,contact.wickedwiz@gmail.com,nezerette@gmail.com,lstsuffer@hotmail.com,wanderbots@gmail.com,getindiegaming@gmail.com,juliandry78@gmail.com,Skooch@proxy.gg,TheStellarJay1@gmail.com,youtube@whatoplay.com,joshpashley93@gmail.com,dan@dangheesling.com,sunnyindiegaming@gmail.com,Makinaicy@gmail.com,flakfire@moreyellow.com,jonofallgames@gmail.com,lonelysharkmelb@gmail.com,anomaly@omniamedia.co,faide@glued.management,papaplatte@new-base.de,thesnowsos@gmail.com,reelchimpclips@gmail.com,dashiexp@gmail.com,cdawgvapr@geexplus.co.jp,minxyonetv@gmail.com,dan@dangheesling.com,ImCadetv@gmail.com,comercial@agenciacurta.com,MrFruitBusiness@gmail.com,UltraCmail@gmail.com,katherineofsky@gmail.com,waydong91@gmail.com,cgplaysshorts@gmail.com,contact@enfant-terrible.media,getindiegaming@gmail.com,thekravination@gmail.com,Bigfrytv@gmail.com,vulkan.collab@gmail.com,gamevbusiness@gmail.com,InsertReviewsYT@gmail.com,alphabiz@caa.com,pdcpublic223@gmail.com,pchal@afkcreators.com,therealcaptainkiddyt@gmail.com,exvarghragamelog@gmail.com,vitecp@gmail.com,thefrostprime@ggtalentgroup.com,inabetagaming@gmail.com,daniel@rtgame.net,thespiffingbrit@gmail.com,mausturd@gmail.com,ironpotosti@gmail.com,baertaffy@moreyellow.com,shufflefm@a2zinfluencers.com,veracbusiness@gmail.com,zaikozila2012@abv.bg,nick.calandra@escapistmagazine.com,privatemango12@gmail.com,graeme2lt@gmail.com,slayxc2@gmail.com."; 
        List<string> duplicateEmails = FindDuplicates(ConvertEmailList(_S));

        if (duplicateEmails.Count > 0)
        {
            Debug.Log("Duplicates found in the email list:");
            foreach (string duplicateEmail in duplicateEmails)
            {
                Debug.Log("Duplicate: " + duplicateEmail);
            }
        }
        else
        {
            Debug.Log("No duplicates found in the email list.");
        }
    }
    private List<string> FindDuplicates(List<string> listToCheck)
    {
        List<string> duplicates = new List<string>();
        HashSet<string> seenEmails = new HashSet<string>();

        foreach (string email in listToCheck)
        {
            if (!seenEmails.Add(email))
            {
                if (!duplicates.Contains(email))
                {
                    duplicates.Add(email);
                }
            }
        }

        return duplicates;
    }

    public static List<string> ConvertEmailList(string rawEmailList)
    {
        List<string> emailList = new List<string>();

        // Split the raw email list by commas
        string[] emails = rawEmailList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        // Remove whitespace and add valid emails to the list
        foreach (string email in emails)
        {
            string cleanedEmail = email.Trim();
            if (IsValidEmail(cleanedEmail))
            {
                emailList.Add(cleanedEmail);
            }
        }

        return emailList;
    }

    private static bool IsValidEmail(string email)
    {
        // Use a simple regex pattern to check if the email is valid
        string pattern = @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$";
        return Regex.IsMatch(email, pattern);
    }
}
