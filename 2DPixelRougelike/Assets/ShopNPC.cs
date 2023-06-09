using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopNPC : NPC
{
    [Header("ShopNPC")]
    [SerializeField] private string offPrompt = "not working :(";
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite OnSprite;
    [SerializeField] private Sprite OffSprite;
    //[SerializeField] private TextMeshPro testtext;
    private bool state = false;
    public override void OnInteract()
    {
        //return if not working
        if (!state) return;

        //open the shop
        Shop.Instance.OpenShop();

        //update visual
        state = false;
        UpdateVisual();
        base.OnInteract();

        //show inactive prompt
        prompRefreshRequest = true;
    }
    public override string GetInteractionPrompt()
    {
        return state? base.GetInteractionPrompt(): offPrompt;
    }

    private void UpdateVisual()
    {
        sr.sprite = state? OnSprite : OffSprite;

    }

    private void WaveManager_OnWaveStart(int _waveNum)
    {
        //testtext.SetText("wave1");
        int _startAtWave = 1;
        int _repeatEveryWave = 5;
        if ((_waveNum - _startAtWave) % _repeatEveryWave == 0)
        {
            state = true;
            UpdateVisual();
            MessageBoard.Instance.SpawnMessage("SHOP OPENED!");

            //show inactive prompt
            prompRefreshRequest = true;
        }

        //testtext.SetText("_startAtWave= " + _startAtWave + " - _repeatEveryWave= " + _repeatEveryWave);
    }

    protected override void Start() {
        UpdateVisual();
        base.Start();
        //testtext.SetText("started");
    }
    protected void Awake()
    {
        waveManager.OnWaveStart += WaveManager_OnWaveStart;
    }
    private void OnDestroy() => waveManager.OnWaveStart -= WaveManager_OnWaveStart;


   

}
