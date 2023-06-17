using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomFPSCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tx = null;
    [SerializeField] private float upadteTime = .3f;

    private float countFps = 0;
    private float avgCounts = 0;
    [SerializeField] private TextMeshProUGUI avgTx = null;
    // Start is called before the first frame update
    void Start()
    {
        tx = GetComponent<TextMeshProUGUI>();
        UpdateText();
        InvokeRepeating(nameof(UpdateAvg), 1, .1f);
    }

    // Update is called once per frame
    void UpdateText()
    {
        tx.text = ""+ (int)(1f / Time.deltaTime);
        Invoke(nameof(UpdateText), upadteTime);
    }

    private void UpdateAvg()
    {
        countFps += 1 / Time.deltaTime;
        avgCounts++;
        avgTx.text = ""+(int)(countFps/ avgCounts);
    }
}
