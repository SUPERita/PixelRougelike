using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemCollection", menuName = "Custom/Items/ItemCollection")]
public class ItemCollection : ScriptableObject
{
    [SerializeField] private List<Item> items;

    public Item GetRandomItem()
    {
        return items[Random.Range(0, items.Count)];
    }

}