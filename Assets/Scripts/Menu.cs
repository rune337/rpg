using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject canvas;
    public GameObject menu;
    public GameObject player;

    TextMeshProUGUI talk;
    TextMeshProUGUI item;
    TextMeshProUGUI check;
    TextMeshProUGUI equip;
    ItemInventory playerItem;

    public GameObject itemMenu;

    public GameObject[] cursors;
    int currentIndex = 0;

    public bool isMenuOpen = false;
    bool isItemMenuOpen = false;

    public GameObject cursorPrefab; // カーソルプレハブ
    public Transform itemListParent; // アイテムリストの親オブジェクト

    private List<GameObject> itemSlots = new List<GameObject>();
    private int itemIndex = 0;

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
        itemListParent = itemMenu.transform.Find("ItemList");

        cursors = new GameObject[4];
        cursors[0] = talk.transform.Find("Cursor").gameObject;
        cursors[1] = item.transform.Find("Cursor").gameObject;
        cursors[2] = check.transform.Find("Cursor").gameObject;
        cursors[3] = equip.transform.Find("Cursor").gameObject;

        itemMenu.SetActive(false); // アイテムメニューも最初閉じる

        playerItem = player.GetComponent<ItemInventory>();

        foreach (var cursor in cursors)
            cursor.SetActive(false);

        cursors[currentIndex].SetActive(true);

        menu.SetActive(false);

        SetupVerticalLayoutGroup();
    }

    void SetupVerticalLayoutGroup()
    {
        var layout = itemListParent.GetComponent<VerticalLayoutGroup>();
        if (layout == null)
            layout = itemListParent.gameObject.AddComponent<VerticalLayoutGroup>();

        layout.spacing = 2; // アイテム間の間隔
        layout.childForceExpandHeight = false;
        layout.childControlHeight = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isMenuOpen) StartMenu();
            else EndMenu();
        }
    }

    void StartMenu()
    {
        menu.SetActive(true);
        GameManager.gameState = GameState.menu;
        Time.timeScale = 0;
        isMenuOpen = true;
        StartCoroutine(MenuLoop());
    }

    IEnumerator MenuLoop()
    {
        while (isMenuOpen)
        {
            if (!isItemMenuOpen)
            {
                bool moved = false;

                switch (currentIndex)
                {
                    case 0:
                        if (Input.GetKeyDown(KeyCode.DownArrow)) { currentIndex = 2; moved = true; }
                        else if (Input.GetKeyDown(KeyCode.RightArrow)) { currentIndex = 1; moved = true; }
                        break;
                    case 1:
                        if (Input.GetKeyDown(KeyCode.DownArrow)) { currentIndex = 3; moved = true; }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow)) { currentIndex = 0; moved = true; }
                        break;
                    case 2:
                        if (Input.GetKeyDown(KeyCode.UpArrow)) { currentIndex = 0; moved = true; }
                        else if (Input.GetKeyDown(KeyCode.RightArrow)) { currentIndex = 3; moved = true; }
                        break;
                    case 3:
                        if (Input.GetKeyDown(KeyCode.UpArrow)) { currentIndex = 1; moved = true; }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow)) { currentIndex = 2; moved = true; }
                        break;
                }

                if (moved) UpdateCursor();
                enterMenu();
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.UpArrow)) { itemIndex = Mathf.Max(0, itemIndex - 1); UpdateItemCursor(); }
                else if (Input.GetKeyDown(KeyCode.DownArrow)) { itemIndex = Mathf.Min(itemSlots.Count - 1, itemIndex + 1); UpdateItemCursor(); }
                else if (Input.GetKeyDown(KeyCode.Return)) { UseItem(itemIndex); }
                else if (Input.GetKeyDown(KeyCode.Escape)) { CloseItemMenu(); }
            }

            yield return null;
        }
    }

    void EndMenu()
    {
        if (isItemMenuOpen) CloseItemMenu();
        menu.SetActive(false);
        GameManager.gameState = GameState.playing;
        Time.timeScale = 1f;
        isMenuOpen = false;
        currentIndex = 0;
        UpdateCursor();
    }

    void UpdateCursor()
    {
        foreach (var cursor in cursors) if (cursor != null) cursor.SetActive(false);
        if (cursors[currentIndex] != null) cursors[currentIndex].SetActive(true);
    }

    void enterMenu()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (currentIndex)
            {
                case 0: break;
                case 1: OpenItemMenu(); break;
                case 2: break;
                case 3: break;
            }
        }
    }
    void OpenItemMenu()
    {
        itemMenu.SetActive(true);
        isItemMenuOpen = true;

        foreach (var slot in itemSlots) Destroy(slot);
        itemSlots.Clear();

        foreach (var i in playerItem.items)
        {
            GameObject slot = new GameObject("ItemSlot");
            slot.transform.SetParent(itemListParent, false);

            RectTransform rt = slot.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(150, 30);

            TextMeshProUGUI text = slot.AddComponent<TextMeshProUGUI>();
            text.text = $"{i.name} ×{i.count}";
            text.fontSize = 24;
            text.lineSpacing = 0f;

            // 文字色を強制白に設定
            text.color = Color.white;
            text.fontMaterial = Instantiate(text.fontMaterial);
            text.fontMaterial.SetColor("_FaceColor", Color.white);
            text.ForceMeshUpdate();

            LayoutElement le = slot.AddComponent<LayoutElement>();
            le.minHeight = 30;
            le.preferredHeight = 30;

            GameObject cursor = Instantiate(cursorPrefab, slot.transform);
            cursor.name = "Cursor";
            cursor.SetActive(false);

            itemSlots.Add(slot);
        }

        itemIndex = 0;
        UpdateItemCursor();
    }


    void CloseItemMenu()
    {
        itemMenu.SetActive(false);
        isItemMenuOpen = false;
    }

    void UpdateItemCursor()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            Transform cursor = itemSlots[i].transform.Find("Cursor");
            if (cursor != null)
            {
                cursor.gameObject.SetActive(i == itemIndex);

                if (i == itemIndex) // 選択中のみ位置調整
                {
                    RectTransform cursorRT = cursor.GetComponent<RectTransform>();
                    if (cursorRT != null)
                    {
                        cursorRT.anchoredPosition = new Vector2(-100, -10); // 左に20移動
                    }
                }
            }
        }
    }

    void UseItem(int index)
    {
        Debug.Log($"{playerItem.items[index].name} を使用しました");
    }
}
