using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Menu : MonoBehaviour
{
    public GameObject canvas; //キャンバス一番親
    public GameObject menu;  //メニューパネル
    public GameObject player;

    TextMeshProUGUI talk;  //話す
    TextMeshProUGUI item;  //道具
    TextMeshProUGUI check; //調べる
    TextMeshProUGUI equip; //装備
    ItemInventory playerItem;

    public GameObject itemMenu;

    public GameObject[] cursors; //カーソル
    int currentIndex = 0; // 現在のカーソル位置（0=左上）

    public bool isMenuOpen = false; //メニュー開いているかどうかの判別フラグ
    TextMeshProUGUI itemText;
    bool isItemMenuOpen = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        menu = canvas.transform.Find("Menu").gameObject;

        talk = menu.transform.Find("Talk").GetComponent<TextMeshProUGUI>();
        item = menu.transform.Find("Item").GetComponent<TextMeshProUGUI>();
        check = menu.transform.Find("Check").GetComponent<TextMeshProUGUI>();
        equip = menu.transform.Find("Equip").GetComponent<TextMeshProUGUI>();

        itemMenu = menu.transform.Find("ItemMenu").gameObject;

        cursors = new GameObject[4];
        cursors[0] = talk.transform.Find("Cursor").gameObject;
        cursors[1] = item.transform.Find("Cursor").gameObject;
        cursors[2] = check.transform.Find("Cursor").gameObject;
        cursors[3] = equip.transform.Find("Cursor").gameObject;

        playerItem = player.GetComponent<ItemInventory>();
        itemText = itemMenu.transform.Find("ItemText").GetComponent<TextMeshProUGUI>();


        foreach (var cursor in cursors)
            cursor.SetActive(false);

        cursors[currentIndex].SetActive(true);

        // 最初は非表示
        menu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isMenuOpen)
            {
                StartMenu();
            }
            else
            {
                EndMenu();
            }
        }


    }

    void StartMenu()
    {
        menu.SetActive(true);   // メニューを開く
        GameManager.gameState = GameState.menu;
        Time.timeScale = 0;

        isMenuOpen = true;

        StartCoroutine(MenuLoop());
    }

    IEnumerator MenuLoop()
    {
        while (isMenuOpen)
        {
            if (!isItemMenuOpen) // ← 道具メニューを開いてない時だけカーソル操作
            {
                bool moved = false;

                switch (currentIndex)
                {
                    case 0: // 左上
                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        { currentIndex = 2; moved = true; }
                        else if (Input.GetKeyDown(KeyCode.RightArrow))
                        { currentIndex = 1; moved = true; }
                        break;

                    case 1: // 右上
                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        { currentIndex = 3; moved = true; }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        { currentIndex = 0; moved = true; }
                        break;

                    case 2: // 左下
                        if (Input.GetKeyDown(KeyCode.UpArrow))
                        { currentIndex = 0; moved = true; }
                        else if (Input.GetKeyDown(KeyCode.RightArrow))
                        { currentIndex = 3; moved = true; }
                        break;

                    case 3: // 右下
                        if (Input.GetKeyDown(KeyCode.UpArrow))
                        { currentIndex = 1; moved = true; }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        { currentIndex = 2; moved = true; }
                        break;
                }

                if (moved)
                    UpdateCursor();

                enterMenu();
            }
            else
            {
                // 例えば Esc キーで ItemMenu を閉じる
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    CloseItemMenu();
                }
            }

            yield return null;
        }



    }


    void EndMenu()
    {
        // もしアイテムメニューが開いていたら閉じる
        if (isItemMenuOpen)
        {
            CloseItemMenu();
        }

        menu.SetActive(false);   // メニューを閉じる
        GameManager.gameState = GameState.playing;
        Time.timeScale = 1.0f;
        isMenuOpen = false;

        // カーソル位置をリセット（次回開いたときは左上からスタート）
        currentIndex = 0;
        UpdateCursor();
    }

    void UpdateCursor()
    {
        foreach (var cursor in cursors)
            if (cursor != null) cursor.SetActive(false);

        if (cursors[currentIndex] != null)
            cursors[currentIndex].SetActive(true);
    }

    void enterMenu()
    {
        //currentIndexでどのメニューを押したか判別
        if (Input.GetKeyDown(KeyCode.Return)) // Enterキー
        {
            switch (currentIndex)
            {
                case 0: // 話す
                    Debug.Log("話すを選択");
                    // Talk() など関数を呼び出す
                    break;

                case 1: // 道具
                    Debug.Log("道具を選択");
                    OpenItemMenu();
                    break;

                case 2: // 調べる
                    Debug.Log("調べるを選択");
                    var player = Object.FindFirstObjectByType<PlayerController>();
                    if (player != null)
                    {
                        player.CheckObject();
                    }
                    //CheckObject();
                    break;

                case 3: // 装備
                    Debug.Log("装備を選択");
                    // OpenEquipMenu();
                    break;
            }
        }
    }

    void OpenItemMenu()
    {
        itemMenu.SetActive(true);
        isItemMenuOpen = true;

        string displayText = "";
        foreach (var item in playerItem.items)
        {
            displayText += $"{item.name} ×{item.count}\n";
        }

        itemText.text = displayText;



    }

    void CloseItemMenu()
    {
        itemMenu.SetActive(false);
        isItemMenuOpen = false;
    }
}
