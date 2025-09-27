using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParameterController : MonoBehaviour
{
    GameObject battle;
    EncountManger playerEncount;
    bool isBattleOpen;

    public GameObject canvas;
    public GameObject battlePanel;
    public GameObject battleMessage;
    TextMeshProUGUI message;
    BattleMessageWindow battleMessageWindow;

    GameObject talkPanel;
    TextMeshProUGUI itemText;



    public EnemyData[] enemyDataArray; //エネミーのデータを入れる配列
    // モンスターごとのステータスを保持するクラス
    [System.Serializable]
    public class EnemyStats
    {
        public string name;
        public int hp;
        public int attack;
        public int defense;
        public float speed;
        public EnemyData data;
    }
    public List<EnemyStats> enemies = new List<EnemyStats>();




    public PlayerData[] playerDataArray; //プレイヤーのデータを入れる配列
    [System.Serializable]
    public class PlayerStats
    {
        public string name;
        public int hp;
        public int baseAttack;
        public int baseDefense;
        public float baseSpeed;

        public int attack;
        public int defense;
        public float speed;

        public PlayerData data;

        public List<ItemData> equippedItems = new List<ItemData>(); // 装備品リスト

        public void RecalculateStats()
        {
            attack = baseAttack;
            defense = baseDefense;
            speed = baseSpeed;

            foreach (var item in equippedItems)
            {
                attack += item.attackBonus;
                defense += item.defenseBonus;
                speed += item.moveSpeedBonus;
            }
        }
    }





    public List<PlayerStats> player = new List<PlayerStats>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        battlePanel = canvas.transform.Find("Battle").gameObject;
        battleMessage = battlePanel.transform.Find("BattleMessage").gameObject; //BattleMessage取得
        message = battleMessage.transform.Find("Message").GetComponent<TextMeshProUGUI>();
        battleMessageWindow = canvas.GetComponent<BattleMessageWindow>();

        talkPanel = canvas.transform.Find("Talk").gameObject;
        itemText = talkPanel.transform.Find("ItemText").GetComponent<TextMeshProUGUI>();

        PlayerSet();
        EnemySet();

        battle = GameObject.FindGameObjectWithTag("Player");
        playerEncount = battle.GetComponent<EncountManger>();
        // isBattleOpen = playerEncount.isBattleOpen;

        if (itemText != null)
        {
            itemText.text = "";
            talkPanel.SetActive(false);

        }

        if (enemies.Count > 0 && enemies[0].hp > 0)
        {

        }


    }


    public void PlayerAttackEnemy(int playerIndex, int enemyIndex)
    {
        if (playerIndex < 0 || playerIndex >= player.Count) return;
        if (enemyIndex < 0 || enemyIndex >= enemies.Count) return;

        PlayerStats attacker = player[playerIndex];
        EnemyStats target = enemies[enemyIndex];

        // 攻撃計算（防御力を考慮）
        int damage = Mathf.Max(attacker.attack - target.defense, 1); // 最低1ダメージ
        target.hp -= damage;


        string str = $"{attacker.name}の攻撃！ {target.name}に{damage}ダメージ！";
        battleMessageWindow.AddMessage(str);//メッセージクラスの関数に渡して呼び出す

        Debug.Log($"{attacker.name}の攻撃！ {target.name} に {damage} ダメージ！ 残りHP: {target.hp}");

        // 敵のHPが0以下になった場合
        if (target.hp <= 0)
        {
            Debug.Log($"{target.name} を倒した！");

            enemies.RemoveAt(enemyIndex); // 敵リストから削除
            playerEncount.isBattleOpen = false;
            enemies.Clear();
            EnemySet();

        }
    }

    void PlayerSet()
    {
        // 配列からプレイヤーのステータスを生成
        for (int i = 0; i < playerDataArray.Length; i++)
        {
            PlayerData data = playerDataArray[i];
            if (data != null)
            {
                PlayerStats stats = new PlayerStats
                {
                    name = data.playerName,
                    hp = data.maxHP,
                    attack = data.attack,
                    defense = data.defense,
                    speed = data.moveSpeed,
                    data = data
                };
                player.Add(stats);
                Debug.Log($"生成したプレイヤー: {stats.name} HP:{stats.hp}");
            }
        }
    }

    void EnemySet()
    {
        // 配列からプレイヤーのステータスを生成
        for (int i = 0; i < enemyDataArray.Length; i++)
        {
            EnemyData data = enemyDataArray[i];
            if (data != null)
            {
                EnemyStats stats = new EnemyStats
                {
                    name = data.enemyName,
                    hp = data.maxHP,
                    attack = data.attack,
                    defense = data.defense,
                    speed = data.moveSpeed,
                    data = data
                };
                enemies.Add(stats);
                Debug.Log($"生成した敵: {stats.name} HP:{stats.hp}");
            }
        }
    }

    public int GetMaxHP(int playerIndex = 0)
    {
        if (playerIndex < 0 || playerIndex >= player.Count) return 0;
        return player[playerIndex].data.maxHP;
    }

   public void EquipItem(ItemData itemData, int playerIndex = 0)
{
    if (playerIndex < 0 || playerIndex >= player.Count) return;

    PlayerStats stats = player[playerIndex];

    if (!stats.equippedItems.Contains(itemData))
    {
        stats.equippedItems.Add(itemData);
        stats.RecalculateStats();
            string statusMessage = $"{stats.name}が「{itemData.itemName}」を装備した\n攻撃:{stats.attack} 防御:{stats.defense} 速度:{stats.speed}";
            Debug.Log(statusMessage);

            // ★★★ メッセージ表示用のコルーチンを呼び出す ★★★
            if (itemText != null)
            {
                // 既存のコルーチンがあれば停止してから新しいものを開始
                StopAllCoroutines();
                StartCoroutine(ShowStatusMessage(statusMessage, 1f)); // 3秒間表示
            }

        }
}

    IEnumerator ShowStatusMessage(string message, float duration)
    {
        talkPanel.SetActive(true); // パネルを表示
        itemText.text = message; // メッセージを表示

        yield return new WaitForSeconds(duration); // durationで指定された秒数だけ待つ

        itemText.text = ""; // メッセージをクリアする
        talkPanel.SetActive(false); // パネルを非表示
    }



    public void UnequipItem(ItemData itemData, int playerIndex = 0)
{
    if (playerIndex < 0 || playerIndex >= player.Count) return;

    PlayerStats stats = player[playerIndex];

    if (stats.equippedItems.Contains(itemData))
    {
        stats.equippedItems.Remove(itemData);
        stats.RecalculateStats();
        Debug.Log($"{stats.name} が {itemData.itemName} を外した → 攻撃: {stats.attack}, 防御: {stats.defense}, 速度: {stats.speed}");
    }
}




}

