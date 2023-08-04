using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveShopNPC : NPC
{
    public override void OnInteract()
    {
        PassiveUpgradesManager.Instance.OpenPassiveCanvas();
        base.OnInteract();
    }
}
