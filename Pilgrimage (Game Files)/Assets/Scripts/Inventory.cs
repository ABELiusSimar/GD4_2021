using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public List<Item> itemList;
    public Inventory()
    {
        itemList = new List<Item>();

        //AddItem(new Item {itemType = Item.ItemType.Key1, amount = 1});
        //AddItem(new Item {itemType = Item.ItemType.Key2, amount = 1});
        Debug.Log(itemList.Count);
    }

    //Function to add Item
    public void AddItem(Item item)
    {
        itemList.Add(item);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
