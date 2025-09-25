using System.Data.Common;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


public class MenuTest : MonoBehaviour
{
    public RectTransform[] menuItems; //メニューの選択項目のリスト
    public RectTransform[] cursorList; //カーソル位置のリスト
    public GameObject menu; //メニューそのもの
    public RectTransform cursor; //メニューの選択項目のカーソル
    int currentIndex = 0;
    public bool isMenuOpen = false; //メニュー開いているかどうかの判別フラグ


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menu.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (menu != null)
            {
                menu.SetActive(!menu.activeSelf);   // メニューを開閉
                isMenuOpen = menu.activeSelf; //表示の時はtue、非表示の時はfalseなので、開いてたら true、閉じたら false
            }
            if (menu.activeSelf)
            {
                currentIndex = 0;
                cursor.anchoredPosition = cursorList[currentIndex].anchoredPosition;

            }
        }


        //カーソル移動
        switch (currentIndex)
        {
            case 0: // 左上
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    currentIndex = 2; // 左下へ
                    cursor.anchoredPosition = cursorList[currentIndex].anchoredPosition;

                }

                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    currentIndex = 1; // 右上へ
                    cursor.anchoredPosition = cursorList[currentIndex].anchoredPosition;
                }
                break;

            case 1: // 右上
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    currentIndex = 3; // 右下へ
                    cursor.anchoredPosition = cursorList[currentIndex].anchoredPosition;
                }

                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    currentIndex = 0; // 左上へÏ
                    cursor.anchoredPosition = cursorList[currentIndex].anchoredPosition;
                }

                break;

            case 2: // 左下
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    currentIndex = 0; // 左上へ
                    cursor.anchoredPosition = cursorList[currentIndex].anchoredPosition;
                }

                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    currentIndex = 3; // 右下へ
                    cursor.anchoredPosition = cursorList[currentIndex].anchoredPosition;
                }

                break;

            case 3: // 右下
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    currentIndex = 1; // 右上へ
                    cursor.anchoredPosition = cursorList[currentIndex].anchoredPosition;
                }

                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    currentIndex = 2; // 左下へ
                    cursor.anchoredPosition = cursorList[currentIndex].anchoredPosition;
                }

                break;
        }



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