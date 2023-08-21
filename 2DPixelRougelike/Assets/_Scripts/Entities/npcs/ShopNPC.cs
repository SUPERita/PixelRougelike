using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopNPC : NPC, WaypointTarget
{
    //[Header("ShopNPC")]
    //[SerializeField] private string offPrompt = "not working :(";
    //[SerializeField] private WaveManager waveManager;
    //[SerializeField] private SpriteRenderer sr;
    //[SerializeField] private Sprite OnSprite;
    //[SerializeField] private Sprite OffSprite;
    //[SerializeField] private TextMeshPro testtext;
    private bool isShopWorking = true;
    public override void OnInteract()
    {
        //return if not working
        if (!isShopWorking) return;

        //open the shop
        Shop.Instance.OpenShop();

        //update visual
        //UpdateVisual();
        base.OnInteract();

        //show inactive prompt
        //prompRefreshRequest = true;
        //
        Debug.Log("maybe a lil anim?, the shop is blocked by the 'isShopWorking' variable");
        isShopWorking = false;
        //Destroy(this);
        Destroy(gameObject);
    }

    //public override string GetInteractionPrompt()
    //{
    //    return state? base.GetInteractionPrompt(): offPrompt;
    //}

    //private void UpdateVisual()
    //{
    //    sr.sprite = state? OnSprite : OffSprite;

    //}

    //private void WaveManager_OnWaveStart(int _waveNum)
    //{
    //    //testtext.SetText("wave1");
    //    int _startAtWave = 1;
    //    int _repeatEveryWave = 2;
    //    if ((_waveNum - _startAtWave) % _repeatEveryWave == 0)
    //    {
    //        state = true;
    //        UpdateVisual();
    //        MessageBoard.Instance.SpawnMessage("SHOP OPENED!");

    //        //show inactive prompt
    //        prompRefreshRequest = true;
    //    }

    //    //testtext.SetText("_startAtWave= " + _startAtWave + " - _repeatEveryWave= " + _repeatEveryWave);
    //}

    //protected override void Start() {
    //    UpdateVisual();
    //    base.Start();
    //    //testtext.SetText("started");
    //}
    //protected void Awake()
    //{
    //    waveManager.OnWaveStart += WaveManager_OnWaveStart;
    //}
    //private void OnDestroy() => waveManager.OnWaveStart -= WaveManager_OnWaveStart;

    protected override void Start()
    {
        base.Start();
        WaypointIndicatorManager.Instance.SummonWaypointIndicator(this);
    }
    public Transform GetTargetTransform()
    {
        return transform;
    }

    public override void OnEnterRange()
    {
        if (this == null) return;
        GetComponent<Animator>().SetTrigger("Open");
        base.OnEnterRange();
    }
    public override void OnExitRange()
    {
        if (this == null) return;
        GetComponent<Animator>().SetTrigger("Close");
        base.OnExitRange();
    }


}
