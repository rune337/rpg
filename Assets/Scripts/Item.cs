[System.Serializable] //Inspectorで見えるようにする
public class Item
{
    public string name; //アイテムの名前
    public static int count; //所持数
    

    public Item(string name, int count)
    {
        this.name = name;
        Item.count = count;
    }
}