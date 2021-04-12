using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public enum ItemType
    {
        Key1,
        Key2
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Key1:
                return ItemAssets.Instance.Key1Sprite;
            case ItemType.Key2:
                return ItemAssets.Instance.Key2Sprite;
        }
    }
}
