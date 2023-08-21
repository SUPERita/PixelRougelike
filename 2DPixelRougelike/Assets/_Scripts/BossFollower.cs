using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFollower : Boss
{


    protected override void DoBehaviour()
    {
        base.DoBehaviour();
        FollowPlayer();
    }

}
