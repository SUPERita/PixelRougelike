using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Testing_ItemLister : MonoBehaviour
{
    [SerializeField] ItemCollection _items;
    void Start()
    {
        foreach (var item in _items.Test_GetItemsRaw()) {
            GameObject _g = new GameObject();
            _g.transform.SetParent(this.transform);
            _g.AddComponent<Image>();
            _g.GetComponent<Image>().sprite = item.itemSprite;

            GameObject _t = new GameObject();
            _t.transform.SetParent(_g.transform);
            _t.AddComponent<TextMeshProUGUI>();
            _t.GetComponent<TextMeshProUGUI>().SetText(item.itemName);
            _t.GetComponent<TextMeshProUGUI>().fontSize = 10;
            _t.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        }
    }

}
