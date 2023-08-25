using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour, ISelectHandler
{
    public Item item { get; private set; }
    [SerializeField] private Image ItemImage = null;
    int index = 0;
    Shop shop;
    public void InitializeDisplay(Item _item, int _index, Shop _shop)
    {
        item = _item;
        index = _index;
        shop = _shop;
        ItemImage.sprite = item.itemSprite;
    }

    public void OnSelect(BaseEventData eventData)
    {
        shop.SelectedItemDisplay(index);
        //TooltipManager.Instance.RequestTooltip(GetComponent<RectTransform>(), GetDescription(), personalOffset);
    }

}
