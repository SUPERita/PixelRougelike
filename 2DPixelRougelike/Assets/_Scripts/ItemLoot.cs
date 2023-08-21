//using AssetKits.ParticleImage.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemLoot : LootBase
{
    [Header("Item stuff")]
    [SerializeField] private ItemCollection itemCollection;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshPro textMesh;
    private Item _item = null;
    protected override void OnTouchPlayer(Collider2D _collison)
    {
        base.OnTouchPlayer(_collison);

        Shop.Instance.GiveItem(_item);
        PlayPickupSound();

        Destroy(gameObject);
    }

    protected override void Start()
    {
        base.Start();

        _item = itemCollection.GetRandomItem();
        spriteRenderer.sprite = _item.itemSprite;
        textMesh.text = _item.GetItemStatsReadable(false);

        //scale item correctly
        spriteRenderer.transform.localScale = Vector3.one* (32/spriteRenderer.bounds.size.y)/32 ;

        //if no one wants it
        Destroy(gameObject, 20);
    }
}
