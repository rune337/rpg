using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public TreasureData equippedItem;

    public void Equip(TreasureData item)
    {
        equippedItem = item;
        Debug.Log($"{item.itemName} を装備しました");
    }

    public void Unequip()
    {
        Debug.Log($"{equippedItem.itemName} を外しました");
        equippedItem = null;
    }
}
