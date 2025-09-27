using System.Collections.Generic;
using UnityEngine;

// プレイヤーのインベントリを管理
public class PlayerInventory : MonoBehaviour
{
    public static List<InventoryItem> items = new List<InventoryItem>();
    

    // アイテムを追加
    public void AddItem(ItemData data, int count = 1)
    {
        InventoryItem existing = items.Find(x => x.data == data);
        if (existing != null)
        {
            existing.count += count;
        }
        else
        {
            items.Add(new InventoryItem(data, count));
        }
    }

    // アイテムを使用
   public void UseItem(int index, PlayerStatus playerStatus)
{
    if (index < 0 || index >= items.Count) return;

    InventoryItem selectedItem = items[index];
    if (selectedItem == null || selectedItem.data == null) return;

    if (selectedItem.data.type == ItemType.Consumable)
    {
        playerStatus.RestoreHP(selectedItem.data.power);
    }
    else if (selectedItem.data.type == ItemType.Equipment)
    {
        playerStatus.Equip(selectedItem.data);
    }

    selectedItem.count--;
    if (selectedItem.count <= 0)
        items.RemoveAt(index);
}



    
}
