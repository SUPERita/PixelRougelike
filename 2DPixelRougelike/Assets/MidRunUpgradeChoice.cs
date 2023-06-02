using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidRunUpgradeChoice : MonoBehaviour
{
    public int _value { get; private set; }
    public string _upgradeName { get; private set; }
    public void InitializeUpgrade(string _n, int _v)
    {
        _value = _v;
        _upgradeName = _n;
    }


    public event Action<MidRunUpgradeChoice> OnChoiceClicked;
    public void ClickedChoice()
    {
        OnChoiceClicked?.Invoke(this);
    }

}
