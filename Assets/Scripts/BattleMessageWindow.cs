using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleMessageWindow : MonoBehaviour
{
    public TextMeshProUGUI messageText; // 表示するテキスト
    public int maxLines = 3; // 同時に表示する行数

    private Queue<string> messageQueue = new Queue<string>();

    public void AddMessage(string msg)
    {
        // 新しいメッセージを追加
        messageQueue.Enqueue(msg);

        // 最大行数を超えたら古いメッセージを削除
        if (messageQueue.Count > maxLines)
        {
            messageQueue.Dequeue();
        }

        // まとめて表示
        messageText.text = string.Join("\n", messageQueue);
    }

    // ★★★ 全てのメッセージを消去するための新しいメソッド ★★★
    public void ClearMessages()
    {
        if (messageText != null)
        {
            // キューの中身を完全に空にする
            messageQueue.Clear();

            // 画面のテキストも空にする
            messageText.text = "";
        }
    }
}
