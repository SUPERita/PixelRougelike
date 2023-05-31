using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomFPSCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tx = null;
    [SerializeField] private float upadteTime = .3f;
    // Start is called before the first frame update
    void Start()
    {
        tx = GetComponent<TextMeshProUGUI>();
        UpdateText();
    }

    // Update is called once per frame
    void UpdateText()
    {
        tx.text = ""+ (int)(1f / Time.deltaTime);
        Invoke(nameof(UpdateText), upadteTime);
    }
}
