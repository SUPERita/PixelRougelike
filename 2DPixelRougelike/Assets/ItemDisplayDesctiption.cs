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
