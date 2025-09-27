using UnityEngine;
using TMPro;
using System.Collections;

public enum TreasureRespawnType
{
    None,       // 復活しない
    DungeonExit // ダンジョン外に出たら復活
}


public class TreasureActive : MonoBehaviour
{

    public TreasureRespawnType respawnType = TreasureRespawnType.None;
    public bool neverClose = false; // 開けた宝箱閉じるかどうか


    public Sprite openedSprite;      // 開いた宝箱の画像
    public Sprite closedSprite;      //閉じた宝箱の画像
    private bool isOpen = false;     // 開いているかどうか
    private bool isPlayerNear = false; // プレイヤーが近くにいるかどうか
    public TreasureData[] items;
    public MessageData msg;
    GameObject canvas;
    GameObject talkPanel;
    TextMeshProUGUI itemText;

    private PlayerInventory playerInventory; // ←追加

    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        talkPanel = canvas.transform.Find("Talk").gameObject;
        itemText = talkPanel.transform.Find("ItemText").GetComponent<TextMeshProUGUI>();

        // プレイヤーの PlayerInventory を取得
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerInventory = player.GetComponent<PlayerInventory>();
        }

        if (playerInventory == null)
        {
            Debug.LogError("PlayerInventory が見つかりません！");
        }
    }





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
        if (isOpen) return; // すでに開いてたら無視
        isOpen = true;

        // スプライトを開いた宝箱に変更
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = openedSprite;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer が見つかりません！");
        }

        if (items == null || items.Length == 0)
        {
            Debug.Log("宝箱は空っぽでした！");
            return;
        }

        if (playerInventory == null)
        {
            Debug.LogError("PlayerInventory が見つかりません！");
            return;
        }

        bool gotItem = false;

        foreach (TreasureData treasure in items)
        {
            if (treasure == null)
            {
                Debug.LogWarning("TreasureData が null です！");
                continue;
            }

            if (treasure.itemData == null)
            {
                Debug.LogWarning($"TreasureData {treasure} の itemData が null です！");
                continue;
            }

            playerInventory.AddItem(treasure.itemData, treasure.count);
            Debug.Log($"宝箱を開けた！ {treasure.itemData.itemName} ×{treasure.count} を手に入れた！");
            gotItem = true;
        }

        if (gotItem)
        {
            StartCoroutine(ItemTalk());
        }
        else
        {
            Debug.Log("宝箱には有効なアイテムがありませんでした！");
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

    IEnumerator ItemTalk()
    {

        GameManager.gameState = GameState.talk;
        talkPanel.SetActive(true);
        Time.timeScale = 0;
        itemText.text = msg.msgData[0].message;

        // 押しっぱなし対策: 一度キーを離すまで待機
        while (Input.GetKey(KeyCode.Return))
        {
            yield return null;
        }

        //エンター押されるまで待機
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }


        //エンター押されたら閉じる
        talkPanel.SetActive(false);
        Time.timeScale = 1;
        GameManager.gameState = GameState.playing;

    }

    void OnEnable()
    {
        if (respawnType == TreasureRespawnType.DungeonExit)
            isOpen = false; // ダンジョン外に出た時用にリセット
    }

    void OnDisable()
    {
        GameManager.DungeonExitEvent -= ResetChest;
    }

    private void ResetChest()
    {
        if (respawnType == TreasureRespawnType.DungeonExit)
        {
            isOpen = false;
            // 元の宝箱スプライトに戻す
            GetComponent<SpriteRenderer>().sprite = closedSprite;
        }


    }
}
