using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item { get; private set; }
    [SerializeField] private Image ItemImage = null;
    public void InitializeDisplay(Item _item)
    {
        item = _item;
        ItemImage.sprite = item.itemSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        FindObjectOfType<ItemDisplayDesctiption>().Display_Enter(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        FindObjectOfType<ItemDisplayDesctiption>().Display_Exit(item);
    }
}
