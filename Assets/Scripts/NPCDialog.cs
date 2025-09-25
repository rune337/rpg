using UnityEngine;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    public GameObject canvas;
    public GameObject talkPanel; // Canvasのtalkパネルを割り当てる
    private bool isCollidingWithPlayer = false;
    private bool isDialogueOpen = false; // 開いているかどうか
    public MessageData msg; //メッセージのスクリプタブルオブジェクトを割り当てる
    GameObject text; //talkパネルの子要素のtextを割り当てる
    TextMeshProUGUI messageText;
    TextMeshProUGUI nameText;

    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        talkPanel = canvas.transform.Find("Talk").gameObject;
        messageText = talkPanel.transform.Find("MessageText").GetComponent<TextMeshProUGUI>();
        nameText = talkPanel.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
  
    }
            
    
    

    void Update()
    {
        if (isCollidingWithPlayer)
        {
            // Eキーで開閉をトグル
            if (Input.GetKeyDown(KeyCode.E))
            {
                isDialogueOpen = !isDialogueOpen;
                talkPanel.SetActive(isDialogueOpen);
                StartTalk();
            }
        }
    }

    void StartTalk()
    {
        messageText.text = msg.msgData[0].message;
        nameText.text = msg.msgData[0].name;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
            Debug.Log("プレイヤーがNPCにぶつかった");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
            isDialogueOpen = false; // 離れたら閉じる

           if (talkPanel != null)   // 破棄されてないか確認
        {
            talkPanel.SetActive(false);
        }
            Debug.Log("プレイヤーがNPCから離れた");
        }
    }
}
