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
        //Debug.Log("root populated");
        foreach (KeyValuePair<StatType, PlayerStat> _stat in PlayerStatsHolder.Instance.GetPlayerStats().GetRawStats())
        {
            if (!_stat.Value.showInDisplay) { continue; }

            //for (int i = 0; i < 20; i++)
            //{

            //}
            GameObject _g = Instantiate(prefabSingleStat, root);
            _g.GetComponent<TextMeshProUGUI>().SetText(_stat.Value.statName + ": " + _stat.Value.value);
            _g.GetComponentInChildren<Image>().sprite = _stat.Value.icon;
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
