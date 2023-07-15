using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPManager : StaticInstance<XPManager>
{
    
    [SerializeField] private Image xpimage;
    private float currentXP = 0;
    private int currentXPLevel = 0;

    private void Start()
    {
        UpdateXPVisual();
    }

    private void UpdateXPVisual()
    {
        xpimage.fillAmount = currentXP / (float)GetCurrentXPLevelRequierment();
    }
    public void AddXP(int _v)
    {
        currentXP += (_v*(PlayerStatsHolder.Instance.TryGetStat(StatType.XPGain)/100f));
        //Debug.Log((_v * (PlayerStatsHolder.Instance.TryGetStat(StatType.XPGain) / 100f)));
        UpdateXPVisual();

        if(currentXP >= GetCurrentXPLevelRequierment()) { LevelUp(); }
    }
    private void LevelUp()
    {
        //take xp
        currentXP -= GetCurrentXPLevelRequierment();
        //level up
        currentXPLevel++;
        //get reward
        MidRunUpgradesManager.Instance.OpenStatChoice();

        UpdateXPVisual();

    }

    private int GetCurrentXPLevelRequierment()
    {
        return (100 + (25* currentXPLevel* currentXPLevel))*10;
    }
}
