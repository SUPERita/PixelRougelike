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
    [ButtonGroup("q")]
    [Button]
    public void OpenPassiveCanvas()
    {
        GameStateManager.Instance.SetState(GameState.PassiveUpgrades);

        //fade in
        Time.timeScale = 0f;
        Helpers.ToggleCanvas(canvasGroup, true);

        //subscribe
        SubscribeToChildren(true);

        //set selction
        Helpers.SelectSomethingUnder(transform);
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
        //check if isnt maxed
        if(!obj.IsStatMaxed())
        {
            //Debug.LogError("CHECK IF CAS EFFORD");
            if (ResourceSystem.Instance.HasEnougthResources(ResourceType.EnergyNugget, obj.GetCurrentCost())){
                ResourceSystem.Instance.TakeResourceAmount(ResourceType.EnergyNugget, obj.GetCurrentCost ());
                obj.AddLevel();
            } else { Debug.LogError("too poor"); }
        } else { Debug.LogError("stat maxed"); }
        

        AfterChoicePressed();
    }
    private void AfterChoicePressed()
    {
        //ClosePassivesCanvas();
    }


    //exit
    public void ClosePassivesCanvas()
    {
        GameStateManager.Instance.ReturnToBaseState();

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
            //Debug.Log(_p.name);
            _passives.Add(new PlayerStatInstance(_p.statname, _p.GetCurrentStatValue()));
        }
        passivePlayerStats.SetPassiveStatsList(_passives);
    }

    [ButtonGroup("q")]
    [Button]
    public void ToggleCanvasGroup()
    {
        Helpers.ToggleCanvas(canvasGroup);
    }

    [Button]
    public void ResetAllPassives()
    {
        foreach(PassiveUpgradeChoice _p in GetComponentsInChildren<PassiveUpgradeChoice>())
        {
            _p.SetCurrentLevel(0);
            _p.UpdateVisuals();
        }
    }

    public bool ValidateOnlyOnePassiveNamed(StatType value)
    {
        int _i = 0;

        foreach (PassiveUpgradeChoice _ChoiceButton in GetComponentsInChildren<PassiveUpgradeChoice>())
        {
            if(_ChoiceButton.statname == value)
            {
                _i++;
            }
        }

        return _i == 1;
    }


}
