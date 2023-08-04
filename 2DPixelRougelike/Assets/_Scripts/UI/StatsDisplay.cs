using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsDisplay : MonoBehaviour
{

    [SerializeField] private RectTransform root = null;
    [SerializeField, AssetsOnly] private GameObject prefabSingleStat = null;
    // Start is called before the first frame update
    void Start()
    {
        ClearRoot();
        PlayerStatsHolder.Instance.GetPlayerStats().OnPlayerStatsChanged += StatsDisplay_OnPlayerStatsChanged;
        PopulateRoot();
    }
    private void OnDisable()
    {
        if (PlayerStatsHolder.Instance)
        {
            PlayerStatsHolder.Instance.GetPlayerStats().OnPlayerStatsChanged -= StatsDisplay_OnPlayerStatsChanged;

        }
    }

    private void StatsDisplay_OnPlayerStatsChanged()
    {
        if (!root) { return; }
        ClearRoot();
        PopulateRoot();
    }

    private void PopulateRoot()
    {
        int _counter  = 0;
        PlayerStats _p = PlayerStatsHolder.Instance.GetPlayerStats();
        //Debug.Log("root populated");
        foreach (KeyValuePair<StatType, PlayerStat> _stat in _p.GetRawStats())
        {
            if (!_stat.Value.showInDisplay) { continue; }

            //for (int i = 0; i < 20; i++)
            //{

            //}
            GameObject _g = Instantiate(prefabSingleStat, root);
            string _coloredValue = _stat.Value.value >= _p.StatDisplay_GetBaseStat(_stat.Key) ?
                $"<color=green> {_stat.Value.value} </color>" :
                $"<color=red> {_stat.Value.value} </color>";
            if(_stat.Value.value == _p.StatDisplay_GetBaseStat(_stat.Key)) { _coloredValue = $"<color=white> {_stat.Value.value} </color>"; }

            _g.GetComponentInChildren<TextMeshProUGUI>().SetText(_stat.Value.statName + ": " + _coloredValue);
            _g.GetComponentInChildren<Image>().sprite = _stat.Value.icon;
            _g.GetComponent<StatDisplaySelectionTooltip>().SetTooltip(_stat.Value.description);
            _counter++;
        }
        //Debug.Log(root.childCount);
        if (root) { root.sizeDelta = Vector2.up * (_counter * 50f +25f); }
         
    }

    private void ClearRoot()
    {
        Helpers.DestroyChildren(root);
        //Debug.Log("root cleared");
    }

    
}
