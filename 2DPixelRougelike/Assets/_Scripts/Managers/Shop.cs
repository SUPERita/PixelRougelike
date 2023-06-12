using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Design;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : StaticInstance<Shop>
{
    //[SerializeField] private MidRunPlayerStats midRunPlayerStats;
    //[SerializeField] private RectTransform statChoicesRoot;
    //[AssetsOnly][SerializeField] private GameObject PrefabStatChoice;
    [Header("technical")]
    [SerializeField] private ItemCollection itemCollection = null;
    [SerializeField] private ItemShopPlayerStats itemShopPlayerStats = null;
    [SerializeField] private CanvasGroup canvasGroup = null;
    [Header("item cards")]
    [SerializeField, AssetsOnly] private GameObject itemCardPrefab = null;
    [SerializeField] private RectTransform itemCardRoot = null;
    [Header("item display")]
    [SerializeField, AssetsOnly] private GameObject itemDisplayPrefab = null;
    [SerializeField] private RectTransform itemDisplaysRoot = null;


    private List<Item> playerItems = new List<Item>();

    private void Start()
    {
        //Debug.LogError("clear items stats on start so no carry over");
        canvasGroup = GetComponent<CanvasGroup>();
        //midRunPlayerStats.ResetMidRunStats();
        Helpers.ToggleCanvas(canvasGroup, false);
        UpdatePlayerStats();
    }

    //enter
    [Button]
    public void OpenShop()
    {
        //fade in
        Time.timeScale = 0f;
        Helpers.ToggleCanvas(canvasGroup, true);
        //TweenInChoices();
        RefreshItemDisplay();
        RefreshShopCards();
        //Choices SetUp
        //SetUpChoices();
    }


    //buttons
    private void RefreshShopCards()
    {
        Helpers.DestroyChildren(itemCardRoot);
        CreateShopCard();
        CreateShopCard();
        CreateShopCard();
    }
    private void RefreshItemDisplay()
    {
        Helpers.DestroyChildren(itemDisplaysRoot);

        foreach (Item _item in playerItems)
        {
            Instantiate(itemDisplayPrefab, itemDisplaysRoot).GetComponent<Image>().sprite = _item.itemSprite;
        }
    }

    private void CreateShopCard()
    {
        Item tmpItem = itemCollection.GetRandomItem();
        int choosedItemPrice = Random.Range((int)tmpItem.itemPriceRange.x, (int)tmpItem.itemPriceRange.y+1);

        Instantiate(itemCardPrefab, itemCardRoot)
                    .GetComponent<ShopItemCard>().InitializeShopCard(tmpItem, this, choosedItemPrice);
    }

    public void OnClickedCard(ShopItemCard _card)
    {
        // add indicator it is gold being priced

        //if has enougth gold
        if (ResourceSystem.Instance.HasEnougthResources(ResourceType.Gold, _card.price))
        {
            //take gold
            ResourceSystem.Instance.TakeResourceAmount(ResourceType.Gold, _card.price);

            //give item
            playerItems.Add(_card.item);
            Destroy(_card.gameObject);
        }
        else { Debug.LogError("too poor"); }
        RefreshItemDisplay();

    }



    //exit
    [Button]
    public void CloseShop() 
    {

        UpdatePlayerStats();
        Helpers.DestroyChildren(itemCardRoot);
        Time.timeScale = 1f;
        Helpers.ToggleCanvas(canvasGroup, false);
    }


    //utils
    private void UpdatePlayerStats()
    {
        List<PlayerStatInstance> _stats = new List<PlayerStatInstance>();
        //loop through all passives and create playerstatinstance for each
        //USE ITEMS NO PassiveUpgradeChoice
        foreach (Item _item in playerItems)
        {
            foreach (PlayerStatInstance _stat in _item.statInstances)
            {
                _stats.Add(new PlayerStatInstance(_stat.statName, _stat.number));
            }
        }
        itemShopPlayerStats.SetItemStatsList(_stats);

    }


    /*
    private void SubscribeToChildren(bool _state)
    {
        foreach (MidRunUpgradeChoice _ChoiceButton in GetComponentsInChildren<MidRunUpgradeChoice>())
        {
            if (_state)
            {
                _ChoiceButton.OnChoiceClicked += _ChoiceButton_OnChoiceClicked;
            }
            else
            {
                _ChoiceButton.OnChoiceClicked -= _ChoiceButton_OnChoiceClicked;
            }
        }
    }
    private void SetUpChoices()
    {
        foreach (MidRunUpgradeChoice _ChoiceButton in GetComponentsInChildren<MidRunUpgradeChoice>())
        {
            _ChoiceButton.InitializeUpgrade("strength", 222);
            _ChoiceButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText("strength, 222");
        }
    }
    private void _ChoiceButton_OnChoiceClicked(MidRunUpgradeChoice obj)
    {
        midRunPlayerStats.CreateMidRunStat(obj._upgradeName, obj._value);
        AfterChoicePressed();
    }
    //exit
    private void AfterChoicePressed()
    {
        //destroy previous
        foreach (Transform child in statChoicesRoot) Destroy(child.gameObject);

        //fade out
        Time.timeScale = 1f;
        Helpers.ToggleCanvas(canvasGroup, false);

        //unsubscribe
        SubscribeToChildren(false);
    }


    private void TweenInChoices()
    {
        //can increase "3" for more statChoices
        for (int i = 0; i < 3; i++)
        {
            RectTransform _r = Instantiate(PrefabStatChoice, Vector3.zero, PrefabStatChoice.transform.rotation, statChoicesRoot).GetComponent<RectTransform>();
            _r.transform.localScale = Vector2.up;
            _r.localPosition = i * Vector3.down * 125f;
            _r.DOScale(Vector3.one, .45f)
                .SetEase(Ease.OutQuint)
                .SetUpdate(true)
                .SetDelay(.2f * i);
        }
    }
    */

    [HorizontalGroup("1")]
    [Button]
    public void TimeGotStuck()
    {
        Time.timeScale = 1f;
    }
    [HorizontalGroup("1")]
    [Button]
    public void ToggleCanvasGroup()
    {
        Helpers.ToggleCanvas(canvasGroup);
    }
}
