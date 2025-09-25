using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Treasure")]
public class TreasureData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public string description;
}
