using UnityEngine;

public enum ItemType
{
    Consumable,   // 回復アイテムなど
    Equipment,     // 武器、防具など
    Need           // 特殊アイテム（消費しない系）
}


[CreateAssetMenu(fileName = "New Item", menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public ItemType type;

    public int attackBonus;
    public int defenseBonus;
    public float moveSpeedBonus;

    public int power; // 消費アイテム用
}

