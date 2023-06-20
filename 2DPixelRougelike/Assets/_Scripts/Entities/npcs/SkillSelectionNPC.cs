using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectionNPC : NPC
{
    public override void OnInteract()
    {
        SkillSelection.Instance.OpenSkillSelection();
        base.OnInteract();
    }
}
