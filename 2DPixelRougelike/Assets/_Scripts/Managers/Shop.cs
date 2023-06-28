using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Design;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] private TextMeshProUGUI rerollCostText = null;
    private int currentRerollcost = 0;
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
            //take gold
            ResourceSystem.Instance.TakeResourceAmount(ResourceType.Gold, currentRerollcost);

            //give
            currentRerollcost += 5;
            rerollCostText.SetText("" + currentRerollcost);
            RefreshShopCards();
        } else { Debug.LogError("too poor"); }
        
    }

    private void RefreshItemDisplay()
    {
        if (!itemDisplaysRoot) { return; }
        if (!itemDisplaysRoot.gameObject.activeInHierarchy) { return; }

        Helpers.DestroyChildren(itemDisplaysRoot);

        foreach (Item _item in playerItems)
        {
            Instantiate(itemDisplayPrefab, itemDisplaysRoot).GetComponent<ItemDisplay>().InitializeDisplay(_item);
        }
    }
     
    private void CreateShopCard()
    {
        Item tmpItem = itemCollection.GetRandomItem();
        int choosedItemPrice = Random.Range((int)tmpItem.itemPriceRange.x, (int)tmpItem.itemPriceRange.y+1);

        Instantiate(itemCardPrefab, itemCardRoot)
                    .GetComponent<ShopItemCard>().InitializeShopCard(tmpItem, this, choosedItemPrice);
    }

    public void OnClickedCard(ShopItemCard _cardClicked)
    {
        // add indicator it is gold being priced

        //if has enougth gold
        if (ResourceSystem.Instance.HasEnougthResources(ResourceType.Gold, _cardClicked.price))
        {
            //take gold
            ResourceSystem.Instance.TakeResourceAmount(ResourceType.Gold, _cardClicked.price);

            //give item
            playerItems.Add(_cardClicked.item);
            Destroy(_cardClicked.gameObject);
            UpdatePlayerStats();

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
        else { Debug.LogError("too poor"); }
        RefreshItemDisplay();

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
