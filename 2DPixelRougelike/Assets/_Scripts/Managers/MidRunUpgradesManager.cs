using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using static MidRunUpgradeCollection;
//using static UnityEngine.Rendering.DebugUI;

public class MidRunUpgradesManager : StaticInstance<MidRunUpgradesManager>
{
    [SerializeField] private MidRunPlayerStats midRunPlayerStats;
    [SerializeField] private RectTransform statChoicesRoot;
    [AssetsOnly] [SerializeField] private GameObject PrefabStatChoice;
    [SerializeField] private CanvasGroup canvasGroup = null;
    [SerializeField] private MidRunUpgradeCollection midRunUpgradeCollection = null;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        midRunPlayerStats.ResetMidRunStats();
        midRunPlayerStats.NotifyUpdatePlayerStats();
        Helpers.ToggleCanvas(canvasGroup, false);
    }

    //enter
    [Button]
    public void OpenStatChoice()
    {
        AudioSystem.Instance.PlaySound("levelup_vibrato", .75f);
        
        GameStateManager.Instance.SetState(GameState.MidRunUpgrades);
        //fade in
        Time.timeScale = 0f;
        Helpers.ToggleCanvas(canvasGroup, true);
        //canvasGroup.alpha = 1;
        TweenInChoices();

        //subscribe
        SubscribeToChildren(true);

        //Choices SetUp
        SetUpChoices();

        //set selction
        Helpers.SelectSomethingUnder(transform);
        //EventSystem.current.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);
    }



    private void SubscribeToChildren(bool _state)
    {
        foreach(MidRunUpgradeChoice _ChoiceButton in GetComponentsInChildren<MidRunUpgradeChoice>())
        {
            if (_state)
            {
                _ChoiceButton.OnChoiceClicked += _ChoiceButton_OnChoiceClicked;
            } else
            {
                _ChoiceButton.OnChoiceClicked -= _ChoiceButton_OnChoiceClicked;
            }
        }
    }
    private void SetUpChoices()
    {

        PlayerStats _p = PlayerStatsHolder.Instance.GetPlayerStats();
        foreach (MidRunUpgradeChoice _ChoiceButton in GetComponentsInChildren<MidRunUpgradeChoice>())
        {
            StatTypeValRarityPair _choosenStat = midRunUpgradeCollection.GetRandomStatOfRandomRarity();
            //extract values
            int _value = _choosenStat.baseStatValue;
            StatType _type = _choosenStat.statType;
            Color _rarityColor = midRunUpgradeCollection.UpgradeRarityToColor(_choosenStat.rarity);

            PlayerStat _stat = _p.GetPlayerStatRaw(_type);

            //dk
            _ChoiceButton.InitializeUpgrade(_type, _value);
            _ChoiceButton.GetComponent<Image>().color = _rarityColor;

            //pretify number
            string _statNumber = _value.ToString();
            _statNumber = _value > 0 ?
                "<color=green>" + "+" + _statNumber + "</color>" :
                "<color=red>" + _statNumber + "</color>";

            //set text + icon text
            _ChoiceButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText(
                _stat.statName+
                $"(<sprite name={_stat.statName}>) "+
                _statNumber);
        }
    }
    private void _ChoiceButton_OnChoiceClicked(MidRunUpgradeChoice obj)
    {
        midRunPlayerStats.CreateMidRunStat(obj._upgradeName, obj._value);
        AfterChoicePressed();
    }
    //exit
    private void AfterChoicePressed()
    {
        CloseCanvas();
    }

    private void CloseCanvas()
    {
        GameStateManager.Instance.ReturnToBaseState();

        //destroy previous
        foreach (Transform child in statChoicesRoot) Destroy(child.gameObject);

        //fade out
        Time.timeScale = 1f;
        //canvasGroup.alpha = 0;
        Helpers.ToggleCanvas(canvasGroup, false);

        //unsubscribe
        SubscribeToChildren(false);
    }

    private void TweenInChoices()
    {
        //can increase "3" for more statChoices
        for (int i = 0; i < 3; i++)
        {
            RectTransform _r = Instantiate(PrefabStatChoice, Vector3.zero, PrefabStatChoice.transform.rotation, statChoicesRoot).GetComponent<RectTransform>();
        }
    }

    [HorizontalGroup("1")]
    [Button]
    public void TimeGotStuck()
    {
        Time.timeScale = 1f;
    }
    [HorizontalGroup("1")]
    [Button]
    public void ToggleCanvasGroup()
    {
        Helpers.ToggleCanvas(canvasGroup);
    }

}
