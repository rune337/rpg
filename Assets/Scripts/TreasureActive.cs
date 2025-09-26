using UnityEngine;
using TMPro;
using System.Collections;

public class TreasureActive : MonoBehaviour
{
    public Sprite openedSprite;      // 開いた宝箱の画像
    private bool isOpen = false;     // 開いているかどうか
    private bool isPlayerNear = false; // プレイヤーが近くにいるかどうか
    public TreasureData[] items;
    public MessageData msg;
    GameObject canvas;
    GameObject talkPanel;
    TextMeshProUGUI messageText;

    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        talkPanel = canvas.transform.Find("Talk").gameObject;
        messageText = talkPanel.transform.Find("MessageText").GetComponent<TextMeshProUGUI>();
        var textObj = talkPanel?.transform.Find("MessageText");
        if (textObj == null) Debug.LogError("MessageText が見つからない！");
        else messageText = textObj.GetComponent<TextMeshProUGUI>();
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
        isOpen = true;

        // スプライトを開いた宝箱に変更
        GetComponent<SpriteRenderer>().sprite = openedSprite;
        if (items != null && items.Length > 0)
        {
            Debug.Log($"宝箱を開けた！ {items[0].itemName} を手に入れた！");


            StartCoroutine(ItemTalk());
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

    IEnumerator ItemTalk()
    {

        GameManager.gameState = GameState.talk;
        talkPanel.SetActive(true);
        Time.timeScale = 0;
        messageText.text = msg.msgData[0].message;

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
}
