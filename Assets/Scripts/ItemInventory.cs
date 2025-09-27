using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //アイテムを追加
        items.Add(new Item("Potion", 3));
        items.Add(new Item("Sword", 1));

        AddItem("Potion", 2);
    }

    public void AddItem(string name, int count)
    {
        Item found = items.Find(i => i.name == name);
        if (found != null)
        {
            found.count += count;
        }
        else
        {
            items.Add(new Item(name, count));
        }
    }

    public void RemoveItem(string name, int count)
    {
        Item found = items.Find(i => i.name == name);
        if (found != null)
        {
            found.count -= count;
            if (found.count <= 0)
            {
                items.RemoveSwapBack(found);
            }
        }
    }

}
