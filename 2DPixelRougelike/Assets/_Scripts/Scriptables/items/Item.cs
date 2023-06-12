using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Custom/Items/Item")]
public class Item : ScriptableObject
{
    [field: SerializeField] public string itemName { get; private set; }
    [field: SerializeField, PreviewField(50, ObjectFieldAlignment.Left)] public Sprite itemSprite { get; private set; }
    [field: SerializeField] public Vector2 itemPriceRange { get; private set; }
    [field: SerializeField] public List<PlayerStatInstance> statInstances { get; private set; }
    [field: SerializeField, TextArea(5,5)] public string itemDescription { get; private set; }
}

