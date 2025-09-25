using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleMoveSelect : MonoBehaviour
{
    public GameObject canvas; //キャンバス一番親なので取得する
    public GameObject battlePanel; //バトルパネル
    public GameObject moveSelect; //行動選択パネル

    TextMeshProUGUI attack; //攻撃
    TextMeshProUGUI item; //道具
    TextMeshProUGUI spell; //呪文
    TextMeshProUGUI escape; //逃げる

    ParameterController parameter;
    EncountManger encountManger;
    GameObject player;

    public GameObject[] cursors; //カーソル
    int currentIndex = 0; // 現在のカーソル位置（0=左上）



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas"); //canvas取得
        battlePanel = canvas.transform.Find("Battle").gameObject;
        moveSelect = battlePanel.transform.Find("MoveSelect").gameObject;


        attack = moveSelect.transform.Find("Attack").GetComponent<TextMeshProUGUI>();
        item = moveSelect.transform.Find("Item").GetComponent<TextMeshProUGUI>();
        spell = moveSelect.transform.Find("Spell").GetComponent<TextMeshProUGUI>();
        escape = moveSelect.transform.Find("Escape").GetComponent<TextMeshProUGUI>();

        player = GameObject.FindGameObjectWithTag("Player");

        encountManger = player.GetComponent<EncountManger>();

        cursors = new GameObject[4];
        cursors[0] = attack.transform.Find("Cursor").gameObject;
        cursors[1] = item.transform.Find("Cursor").gameObject;
        cursors[2] = spell.transform.Find("Cursor").gameObject;
        cursors[3] = escape.transform.Find("Cursor").gameObject;

        parameter = canvas.GetComponent<ParameterController>();

        for (int i = 0; i < cursors.Length; i++)
        {
            cursors[i].gameObject.SetActive(false);
        }
        cursors[currentIndex].gameObject.SetActive(true);


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void move()
    {
                bool moved = false;

        switch (currentIndex)
        {
            case 0: // 左上
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    currentIndex = 2; // 左下へ
                    moved = true;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    currentIndex = 1; // 右上へ
                    moved = true;
                }
                break;

            case 1: // 右上
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    currentIndex = 3; // 右下へ
                    moved = true;
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    currentIndex = 0; // 左上へ
                    moved = true;
                }
                break;

            case 2: // 左下
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    currentIndex = 0; // 左上へ
                    moved = true;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    currentIndex = 3; // 右下へ
                    moved = true;
                }
                break;

            case 3: // 右下
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    currentIndex = 1; // 右上へ
                    moved = true;
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    currentIndex = 2; // 左下へ
                    moved = true;
                }
                break;
        }

        if (moved)
            UpdateCursor(); // 移動した場合だけ表示切替

        enterMenu();
    }

    void UpdateCursor()
    {
        // 全部非表示
        for (int i = 0; i < cursors.Length; i++)
            if (cursors[i] != null)
                cursors[i].SetActive(false);

        // 現在のカーソルだけ表示
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
                case 0: // 攻撃
                    Debug.Log("攻撃を選択");
                    // Talk() など関数を呼び出す
                    parameter.PlayerAttackEnemy(0, encountManger.randomEnemy);
                    break;

                case 1: // 道具
                    Debug.Log("道具を選択");
                    // OpenItemMenu();
                    break;

                case 2: // 調べる
                    Debug.Log("呪文を選択");
                    //CheckObject();
                    break;

                case 3: // 装備
                    Debug.Log("逃げるを選択");
                    // OpenEquipMenu();
                    break;
            }
        }
    }
}
