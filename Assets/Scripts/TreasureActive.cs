using UnityEngine;

public class TreasureActive : MonoBehaviour
{
    public Sprite openedSprite;      // 開いた宝箱の画像
    private bool isOpen = false;     // 開いているかどうか
    private bool isPlayerNear = false; // プレイヤーが近くにいるかどうか
    public TreasureData[] items;
    public MessageData msg;

    

    private void Update()
    {
        if (isPlayerNear && !isOpen)
        {
            if (Input.GetKeyDown(KeyCode.Return)) // Zキーで調べる
            {
                OpenChest();
            }
        }
    }

    private void OpenChest()
    {
        isOpen = true;

        // スプライトを開いた宝箱に変更
        GetComponent<SpriteRenderer>().sprite = openedSprite;
        if (items != null && items.Length > 0)
        {
            Debug.Log($"宝箱を開けた！ {items[0].itemName} を手に入れた！");
        }
        else
        {
            Debug.Log("宝箱は空っぽだった！");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
