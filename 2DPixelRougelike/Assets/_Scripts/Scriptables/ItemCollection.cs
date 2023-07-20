using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemCollection", menuName = "DataSet/ItemCollection")]
public class ItemCollection : ScriptableObject
{
    [SerializeField] private List<Item> items;

    public Item GetRandomItem()
    {
        return items[Random.Range(0, items.Count)];
    }
    public Item GetRandomItemInWave(int _wave)
    {
        Item _out = GetRandomItem();
        int counter = 0;
        while(_out.itemMinWave > _wave && counter<2000)
        {
            _out = GetRandomItem();
            counter++;
        }

        return _out;
    }

}