using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockedSubButton : SubButton
{
    [Header("locking")]
    [SerializeField] private TextMeshProUGUI priceText;
    //[SerializeField] private TextMeshProUGUI txtVal1;
    [SerializeField] private Image lockImage;
    private string lockedButtonSaveLoc = "lockedbbb";

    //[SerializeField] private int cost;
    //[SerializeField] private ResourceType resourceType = ResourceType.EnergyNugget;
    public override void Button_OnClick()
    {
        //only base call if unlocked
        if (GetIsUnlocked())
        {
            base.Button_OnClick();
        } else if (ResourceSystem.Instance.HasEnougthResources(ResourceType.EnergyNugget, _value2))
        {
            ResourceSystem.Instance.TakeResourceAmount(ResourceType.EnergyNugget, _value2);
            SetIsUnlocked(true);
            SetVisual(true);
            AudioSystem.Instance.PlaySound("reward_vibrato", .75f);
            base.Button_OnClick();
        } else
        {
            Debug.Log("isnt unlocked and doesnt have enougth mony");
        }
    }
    public override SubButton InitializeButton(SubButtonListener _listener, Sprite _image1 = null, string _n = "empty", int _v = -1)
    {
        base.InitializeButton(_listener, _image1, _n, _v);

        //check save stuff
        SetVisual(GetIsUnlocked());

        //priceText.text = _v;

        return this;
    }
    public override SubButton AddAdditionalData(int _v2)
    {
        base.AddAdditionalData(_v2);
        priceText.text = _v2.ToString();
        return this;
    }


    private bool GetIsUnlocked()
    {
        return SaveSystem.LoadBoolFromLocation(lockedButtonSaveLoc + _string1);
    }
    public void SetIsUnlocked(bool _state)
    {
        SaveSystem.SaveBoolAtLocation(_state, lockedButtonSaveLoc + _string1);
    }

    public void SetVisual(bool _state)
    {
        priceText.gameObject.SetActive(!_state);
        lockImage.gameObject.SetActive(!_state);
    }

}
