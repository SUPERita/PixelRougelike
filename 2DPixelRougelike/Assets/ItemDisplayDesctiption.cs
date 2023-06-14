using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDisplayDesctiption : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI desctiprionText = null;

    internal void Display_Enter(Item item)
    {
        if(item == null) { Debug.LogError("wee woo we wo for some reason no item?"); return; }
        desctiprionText.SetText(""+item.itemDescription);
    }

    internal void Display_Exit(Item item)
    {
        desctiprionText.SetText("");
    }

    void Start()
    {
        desctiprionText.SetText("");
    }

 
}
