using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

public class PassiveUpgradesManager : StaticInstance<PassiveUpgradesManager>
{
    [SerializeField] private PassivePlayerStats passivePlayerStats;
    [SerializeField] private RectTransform root;
    //[AssetsOnly][SerializeField] private GameObject PrefabStatChoice;
    [SerializeField] private CanvasGroup canvasGroup = null;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        //canvasGroup.alpha = 0;
        Helpers.ToggleCanvas(canvasGroup, false);
    }

     
    //enter
    [Button]
    public void OpenPassiveCanvas()
    {
        //fade in
        Time.timeScale = 0f;
        Helpers.ToggleCanvas(canvasGroup, true);

        //subscribe
        SubscribeToChildren(true);
    }
    private void SubscribeToChildren(bool _state)
    {
        foreach (PassiveUpgradeChoice _ChoiceButton in GetComponentsInChildren<PassiveUpgradeChoice>())
        {
            if (_state)
            {
                _ChoiceButton.OnChoiceClicked += _ChoiceButton_OnChoiceClicked;
            }
            else
            {
                _ChoiceButton.OnChoiceClicked -= _ChoiceButton_OnChoiceClicked;
            }
        }
    }


    //on click
    private void _ChoiceButton_OnChoiceClicked(PassiveUpgradeChoice obj)
    {
        Debug.LogError("CHECK IF CAS EFFORD");
        obj.AddLevel();
        AfterChoicePressed();
    }
    private void AfterChoicePressed()
    {
        ClosePassivesCanvas();
    }


    //exit
    public void ClosePassivesCanvas()
    {
        //fade out
        Time.timeScale = 1f;
        //canvasGroup.alpha = 0;
        Helpers.ToggleCanvas(canvasGroup, false);

        //compile stat changes
        CompileStatsFromChildren();

        //unsubscribe
        SubscribeToChildren(false);
    }


    //utils
    public void CompileStatsFromChildren()
    {
        List<PlayerStatInstance> _passives = new List<PlayerStatInstance>();
        //loop through all passives and create playerstatinstance for each
        foreach (PassiveUpgradeChoice _p in GetComponentsInChildren<PassiveUpgradeChoice>())
        {
            Debug.Log(_p.name);
            _passives.Add(new PlayerStatInstance(_p.statname, _p.GetStatValue()));
        }
        passivePlayerStats.SetPassiveStatsList(_passives);
    }

    [Button]
    public void ToggleCanvasGroup()
    {
        Helpers.ToggleCanvas(canvasGroup);
    }

}
