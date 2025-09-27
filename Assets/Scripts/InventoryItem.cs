[System.Serializable]
public class InventoryItem
{
    public ItemData data;  // 設計図
    public int count;      // 所持数

    public InventoryItem(ItemData data, int count)
    {
        this.data = data;
        this.count = count;
    }
}
