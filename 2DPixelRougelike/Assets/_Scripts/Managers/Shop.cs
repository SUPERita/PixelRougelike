using DG.Tweening;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
//using System.Drawing.Design;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
//using static UnityEditor.Recorder.OutputPath;

public class Shop : StaticInstance<Shop>
{
    //[SerializeField] private MidRunPlayerStats midRunPlayerStats;
    //[SerializeField] private RectTransform statChoicesRoot;
    //[AssetsOnly][SerializeField] private GameObject PrefabStatChoice;
    [Header("technical")]
    [SerializeField] private ItemCollection itemCollection = null;
    [SerializeField] private ItemShopPlayerStats itemShopPlayerStats = null;
    [SerializeField] private CanvasGroup canvasGroup = null;
    [SerializeField] private TextMeshProUGUI rerollCostText = null;
    private int currentRerollcost = 0;
    [Header("item cards")]
    [SerializeField, AssetsOnly] private GameObject itemCardPrefab = null;
    [SerializeField] private RectTransform itemCardRoot = null;
    [Header("item display")]
    [SerializeField, AssetsOnly] private GameObject itemDisplayPrefab = null;
    [SerializeField] private RectTransform itemDisplaysRoot = null;
    [SerializeField] private Scrollbar itemDisplayScrollbar = null;




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
        GameStateManager.Instance.SetState(GameState.Shop);
        //fade in
        Time.timeScale = 0f;
        Helpers.ToggleCanvas(canvasGroup, true);
        //TweenInChoices();
        RefreshItemDisplay();
        RefreshShopCards();
        //Choices SetUp
        //SetUpChoices();
        currentRerollcost = 0;
        rerollCostText.SetText("" + currentRerollcost);

        // set selected
        EventSystem.current.SetSelectedGameObject(rerollCostText.GetComponentInParent<Button>().gameObject);

        //statr shop music
        
        MusicStarter.Instance.PlayShopMusic();
    }


    //buttons
    private void RefreshShopCards()
    {
        Helpers.DestroyChildren(itemCardRoot);
        CreateShopCard();
        CreateShopCard();
        CreateShopCard();

        rerollCostText.SetText(""+currentRerollcost);
    }
    public void Buttton_Reroll()
    {
        //if has enougth money
        if (ResourceSystem.Instance.HasEnougthResources(ResourceType.Gold, currentRerollcost))
        {
            AudioSystem.Instance.PlaySound("pickup_gling", .75f);

            //take gold
            ResourceSystem.Instance.TakeResourceAmount(ResourceType.Gold, currentRerollcost);

            //give
            currentRerollcost += /*0*/5;
            rerollCostText.SetText("" + currentRerollcost);
            //for(int i = 0; i < 200; i++)
            //{
            //}
                RefreshShopCards();
        } else { Debug.LogError("too poor"); AudioSystem.Instance.PlaySound("error_buzz", .75f); }
        
    }

    private void RefreshItemDisplay()
    {
        if (!itemDisplaysRoot) { return; }
        if (!itemDisplaysRoot.gameObject.activeInHierarchy) { return; }

        Helpers.DestroyChildren(itemDisplaysRoot);
        int _numberOfItems = 0;
        foreach (Item _item in playerItems)
        {
            Instantiate(itemDisplayPrefab, itemDisplaysRoot).GetComponent<ItemDisplay>().InitializeDisplay(_item, _numberOfItems, this);
            _numberOfItems++;
        }

        if (itemDisplaysRoot) { itemDisplaysRoot.sizeDelta = Vector2.up * (Mathf.CeilToInt(_numberOfItems / 16f) * 60f/* + 10f*/); }
        Debug.Log(_numberOfItems / 16);
    }
    public void SelectedItemDisplay(int _displayIndex)
    {
        float height = _displayIndex / 16;
        itemDisplayScrollbar.value = 1f- (height / (itemDisplaysRoot.childCount/16));
    }

    private void CreateShopCard()
    {
        Item _tmpItem = itemCollection.GetRandomItemInWave(WaveManager.Instance.currentWaveRefrence);
        float choosedItemPrice = Random.Range(Item.RarityToBasePrice(_tmpItem.itemRarity) * .75f, Item.RarityToBasePrice(_tmpItem.itemRarity) * 1.25f); //random number -+25%
        choosedItemPrice *= .7f + .3f*WaveManager.Instance.currentWaveRefrence; //scale with levels
        //choosedItemPrice = 0;//debugging

        Instantiate(itemCardPrefab, itemCardRoot)
                    .GetComponent<ShopItemCard>().InitializeShopCard(_tmpItem, this, (int)choosedItemPrice);

        //Tester.Instance.OfferedItem(_tmpItem);// testing
    }

    public void OnClickedCard(ShopItemCard _cardClicked)
    {
        // add indicator it is gold being priced

        //if has enougth gold
        if (ResourceSystem.Instance.HasEnougthResources(ResourceType.Gold, _cardClicked.price))
        {
            AudioSystem.Instance.PlaySound("pickup_gling", .75f);

            //take gold
            ResourceSystem.Instance.TakeResourceAmount(ResourceType.Gold, _cardClicked.price);

            //give item
            GiveItem(_cardClicked.item);
            Destroy(_cardClicked.gameObject);

            // set selected
            ShopItemCard[] _cards = GetComponentsInChildren<ShopItemCard>();
            // 1 because there is alwats 1 card in the frame of the click so in this event it needs to be ignored
            if (_cards.Length > 1) { 
                foreach(ShopItemCard _c in _cards)
                {
                    if (_c != _cardClicked)
                    {
                        EventSystem.current.SetSelectedGameObject(_c.GetComponentInChildren<Button>().gameObject); 
                    }
                }
            }
            //if no more cards
            else
            {
                currentRerollcost = 0;
                rerollCostText.SetText("" + currentRerollcost);
                EventSystem.current.SetSelectedGameObject(rerollCostText.GetComponentInParent<Button>().gameObject);
            }
            
        }
        else { Debug.LogError("too poor"); AudioSystem.Instance.PlaySound("error_buzz", .75f); }
        RefreshItemDisplay();

    }
    public void GiveItem(Item _itm)
    {
        playerItems.Add(_itm);
        UpdatePlayerStats();

        if(_itm.itemRarity == ItemRarity.Legendary) SteamIntegration.UnlockAchievment("ACH_LEGENDERY1");
        if (_itm.itemRarity == ItemRarity.Epic) SteamIntegration.UnlockAchievment("ACH_EPIC1");
        if (_itm.itemRarity == ItemRarity.Rare) SteamIntegration.UnlockAchievment("ACH_RARE1");
        if (_itm.itemRarity == ItemRarity.Uncommon) SteamIntegration.UnlockAchievment("ACH_UNCOMMON1");

        if (playerItems.Count == 10) SteamIntegration.UnlockAchievment("ACH_ITEM10");
        if (playerItems.Count == 20) SteamIntegration.UnlockAchievment("ACH_ITEM20");
        if (playerItems.Count == 40) SteamIntegration.UnlockAchievment("ACH_ITEM40");
        if (playerItems.Count == 60) SteamIntegration.UnlockAchievment("ACH_ITEM60");
    }



    //exit
    [Button]
    public void CloseShop() 
    {
        GameStateManager.Instance.ReturnToBaseState();
        UpdatePlayerStats();
        Helpers.DestroyChildren(itemCardRoot);
        Time.timeScale = 1f;
        Helpers.ToggleCanvas(canvasGroup, false);

        // music
        MusicStarter.Instance.ReturnToCurrentMusic();
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
