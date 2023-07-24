using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingHelper : MonoBehaviour
{
    public void AddLevel()
    {
        XPManager.Instance.AddXP(100000);
    }
    public void removeWeapons()
    {
        WeaponManager.Instance.Test_ClearWeaponSockets();
    }
}
