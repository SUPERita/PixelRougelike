using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MidRunUpgradesManager : StaticInstance<MidRunUpgradesManager>
{
    [SerializeField] private MidRunPlayerStats midRunPlayerStats;
    [SerializeField] private RectTransform statChoicesRoot;
    [AssetsOnly] [SerializeField] private GameObject PrefabStatChoice;
    [SerializeField] private CanvasGroup canvasGroup = null;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        midRunPlayerStats.ResetMidRunStats();
        Helpers.ToggleCanvas(canvasGroup, false);
    }

    //enter
    [Button]
    public void OpenStatChoice()
    {
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
        foreach (MidRunUpgradeChoice _ChoiceButton in GetComponentsInChildren<MidRunUpgradeChoice>())
        {
            _ChoiceButton.InitializeUpgrade("strength", 222);
            _ChoiceButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText("strength, 222");
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
            _r.transform.localScale = Vector2.up;
            _r.localPosition = i * Vector3.down * 125f;
            _r.DOScale(Vector3.one, .45f)
                .SetEase(Ease.OutQuint)
                .SetUpdate(true)
                .SetDelay(.2f *i);
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
