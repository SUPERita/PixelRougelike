using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InvestmentHandler : MonoBehaviour
{
    [SerializeField] WaveManager waveManager;
    private void Awake() => waveManager.OnWaveStart += WaveManager_OnWaveStart;    
    private void OnDisable() => waveManager.OnWaveStart -= WaveManager_OnWaveStart;

    private void WaveManager_OnWaveStart(int obj)
    {
        int _amt = PlayerStatsHolder.Instance.TryGetStat(StatType.Invesment);
        if (_amt < 0) return;
        ResourceSystem.Instance.AddResourceAmount(ResourceType.Gold, _amt);
        //shop lil number
        Transform tmpTextObject = LeanPoolManager.Instance.SpawnFromPool("dmgText").transform;
        LeanPoolManager.Instance.DespawnFromPool(tmpTextObject.gameObject, 1);

        tmpTextObject.SetParent(SharedCanvas.Instance.transform);
        tmpTextObject.position = PlayerRootRefrence.Instance.transform.position;

        tmpTextObject.GetComponent<TextMeshProUGUI>().SetText(_amt.ToString());
        tmpTextObject.GetComponent<TextMeshProUGUI>().color = Color.yellow;
        tmpTextObject.gameObject.GetComponent<DamageText>().DOStartTween(false);
        //make a sound?
    }
}
