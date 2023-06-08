using Sirenix.OdinInspector;

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PassiveUpgradeChoice : MonoBehaviour
{
    private readonly string passiveLocBase = "passiveUpgrade";

    [field: SerializeField,
     ValidateInput("ValidateNoTrailingWhitespace", "last char is a whitespace")]
    public string statname { get; private set; } = "empty";
    private bool ValidateNoTrailingWhitespace(string value)
    {
        if (string.IsNullOrEmpty(value))
            return true;

        // Check if the last character is whitespace
        if (char.IsWhiteSpace(value[value.Length - 1]))
            return false;

        return true;
    }

    [SerializeField] private GameObject LevelContainerPrefab;
    [SerializeField] private RectTransform levelContainerRoot;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI valText;

    [SerializeField] private CostValueCompounent[] valuePerLevel;
    private void Start()
    {
        if(statname == "empty") { Debug.LogError("wee woo wee woo no statName on " + gameObject.name); }

        

        UpdateVisuals();
    }

    //visuals
    private void UpdateVisuals()
    {
        //destory em
        foreach (Transform child in levelContainerRoot)
        {
            if(child != null) Destroy(child.gameObject);
            

        }

        //spawn em
        List<Image> _ims = new List<Image>();
        for (int i = 0; i < valuePerLevel.Length; i++)
        {
            _ims.Add(Instantiate(LevelContainerPrefab, levelContainerRoot).GetComponent<Image>());
        }

        //fill em
        for (int i = 0; i < valuePerLevel.Length; i++)
        {
            _ims[i].fillAmount =
                GetCurrentLevel() > i ? 1f : 0.2f;
        }

        //update cost
        if(IsStatMaxed())
        {
            costText.SetText("MAX");
        }  else
        {
            costText.SetText("" + valuePerLevel[GetCurrentLevel()].cost);
        }

        //update the valText
        if ((GetCurrentLevel() == 0))
        {
            valText.SetText("");
        }
        else
        {
            valText.SetText("+" + valuePerLevel[GetCurrentLevel()-1].val+"%");
        }
        
            

    }

    //utils
    public int GetStatValue()
    {
        if (GetCurrentLevel() == 0) return 0;
        return valuePerLevel[GetCurrentLevel()-1].Val;
    }

    //saving
    private int GetCurrentLevel()
    {
        return SaveSystem.LoadIntFromLocation(passiveLocBase + statname, 0);
    }
    [Button]
    private void SetCurrentLevel(int _val)
    {
        SaveSystem.SaveIntAtLocation(_val, passiveLocBase + statname);

        //controvertial

        
    }
    [Button]
    public void AddLevel()
    {
        if (!IsStatMaxed())
        {
            SetCurrentLevel(1 + GetCurrentLevel());
        }
        else
        {
            //Debug.Log("maxed out: " + statname);
        }
        UpdateVisuals();
    }

    private bool IsStatMaxed()
    {
        return (GetCurrentLevel() >= valuePerLevel.Length);
    }


    //notifing
    public event Action<PassiveUpgradeChoice> OnChoiceClicked;
    public void ClickedChoice()
    {
        OnChoiceClicked?.Invoke(this);
    }
}

[Serializable]
public class CostValueCompounent
{
    [field: SerializeField] public int cost { get; private set; }
    public int Cost => cost;
    [field: SerializeField] public int val { get; private set; }
    public int Val => val;

    public CostValueCompounent(int cost, int val)
    {
        this.val = val;
        this.cost = cost;
    }
}