using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//the card's onclick need to be set to Button_OnClick()
public class ShopItemCard : MonoBehaviour
{

    public Item item { get; private set; }
    private Shop shop;
    public int price { get; private set; }

    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private Image itemImage;
    
    public void InitializeShopCard(Item _item, Shop _shop, int _price)
    {
        item = _item;
        shop = _shop;
        price = _price;
        UpdateVisual();
        
    }
    private void UpdateVisual()
    {
        itemNameText.SetText(item.itemName);
        itemDescriptionText.SetText(item.GetItemStatsReadable()/*itemDescription*/);
        priceText.SetText("" + price);
        if (item.itemSprite != null)
        {
            itemImage.sprite = item.itemSprite;
        }
    }

    public void Button_OnClick()
    {
        shop.OnClickedCard(this);
    }
}
