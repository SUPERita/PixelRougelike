using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPManager : StaticInstance<XPManager>
{
    
    [SerializeField] private Image xpimage;
    private int currentXP = 0;
    private int currentXPLevel = 0;

    private void Start()
    {
        UpdateXPVisual();
    }

    private void UpdateXPVisual()
    {
        xpimage.fillAmount = (float)currentXP / GetCurrentXPLevelRequierment();
    }
    public void AddXP(int _v)
    {
        currentXP += _v;
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
        return 100 + (25* currentXPLevel* currentXPLevel);
    }
}
