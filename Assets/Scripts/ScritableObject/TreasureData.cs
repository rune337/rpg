using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Treasure")]
public class TreasureData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public string description;
    public ItemData itemData; // アイテムの情報
    public int count = 1;     // 個数
}
