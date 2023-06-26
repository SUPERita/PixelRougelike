using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : NPC
{
    [Header("ShopNPC")]
    [SerializeField] private string offPrompt = "not working :(";
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite OnSprite;
    [SerializeField] private Sprite OffSprite;
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
    }
    public override string GetInteractionPrompt()
    {
        return state? base.GetInteractionPrompt(): offPrompt;
    }

    private void UpdateVisual()
    {
        sr.sprite = state? OnSprite : OffSprite;

    }

    protected override void Start() {
        waveManager.OnWaveStart += WaveManager_OnWaveStart;
        UpdateVisual();
        base.Start();
    }
    private void OnDestroy() => waveManager.OnWaveStart -= WaveManager_OnWaveStart;


    private void WaveManager_OnWaveStart(int _waveNum)
    {
        int _startAtWave = 2;
        int _repeatEveryWave = 5;
        if ((_waveNum - _startAtWave) % _repeatEveryWave == 0)
        {
            state = true;
            UpdateVisual();
            MessageBoard.Instance.SpawnMessage("SHOP OPENED!");
        }
    }

}
