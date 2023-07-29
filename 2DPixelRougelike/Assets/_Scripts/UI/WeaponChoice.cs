using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponChoice : StaticInstance<WeaponChoice>, SubButtonListener
{
    [SerializeField] private WeaponCollection weaponCollection;

    [SerializeField] private RectTransform choicesRoot;
    [AssetsOnly][SerializeField] private GameObject PrefabWeapontChoice;
    [SerializeField] private CanvasGroup canvasGroup = null;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        
        Helpers.ToggleCanvas(canvasGroup, false);
    }

    //enter
    [Button]
    public void OpenWeaponChoice()
    {
        GameStateManager.Instance.SetState(GameState.WeaponChoice);
        //fade in
        Time.timeScale = 0f;
        Helpers.ToggleCanvas(canvasGroup, true);
        Debug.Log("s1");
        //Choices SetUp
        SetUpChoices();
        Debug.Log("s2");
        //set selction
        Helpers.SelectSomethingUnder(transform);
        
    }


    private void SetUpChoices()
    {
        for (int _i = 0; _i < 3; _i++)
        {
            WeaponNamePair _w = weaponCollection.GetRandomWeapon();
            Instantiate(PrefabWeapontChoice, choicesRoot).GetComponent<SubButton>()
                .InitializeButton(
                this,
                _w._weaponIcon,
                _w._weapon.gameObject.name + "\n" + _w._weapon.GetDescription()
                ).AddAdditionalData(_w._weapon);
                
        }

    }

    public void OnClicked(SubButton _button)
    {
        Debug.Log("weird stuff around here");
        WeaponManager.Instance.TryAddWeapon((Weapon)_button.behav1);

        CloseCanvas();
    }
    //exit

    public void DismissChoice()
    {
        CloseCanvas();
    }
    private void CloseCanvas()
    {
        GameStateManager.Instance.ReturnToBaseState();

        //destroy previous
        foreach (Transform child in choicesRoot) Destroy(child.gameObject);

        //fade out
        Time.timeScale = 1f;
        Helpers.ToggleCanvas(canvasGroup, false);
    }


    [Button]
    public void ToggleCanvasGroup()
    {
        Helpers.ToggleCanvas(canvasGroup);
    }

}
