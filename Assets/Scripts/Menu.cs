using System.Collections;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject canvas; //キャンバス一番親
    public GameObject menu;  //メニューパネル

    TextMeshProUGUI talk;  //話す
    TextMeshProUGUI item;  //道具
    TextMeshProUGUI check; //調べる
    TextMeshProUGUI equip; //装備

    public GameObject[] cursors; //カーソル
    int currentIndex = 0; // 現在のカーソル位置（0=左上）

    public bool isMenuOpen = false; //メニュー開いているかどうかの判別フラグ

    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        menu = canvas.transform.Find("Menu").gameObject;

        talk = menu.transform.Find("Talk").GetComponent<TextMeshProUGUI>();
        item = menu.transform.Find("Item").GetComponent<TextMeshProUGUI>();
        check = menu.transform.Find("Check").GetComponent<TextMeshProUGUI>();
        equip = menu.transform.Find("Equip").GetComponent<TextMeshProUGUI>();

        cursors = new GameObject[4];
        cursors[0] = talk.transform.Find("Cursor").gameObject;
        cursors[1] = item.transform.Find("Cursor").gameObject;
        cursors[2] = check.transform.Find("Cursor").gameObject;
        cursors[3] = equip.transform.Find("Cursor").gameObject;

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

            yield return null; // 毎フレーム待機
        }
    }

    void EndMenu()
    {
        menu.SetActive(false);   // メニューを閉じる
        GameManager.gameState = GameState.playing;
        Time.timeScale = 1.0f;
        isMenuOpen = false;
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
                    // OpenItemMenu();
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
}
