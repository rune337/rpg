using System.Collections;
using TMPro;
using UnityEngine;

public class EncountManger : MonoBehaviour
{

    public GameObject canvas; //Canvas変数
    public GameObject player; //プレイヤー変数
    private Vector3 playerPos;//最初のプレイヤーポジション
    private int steps; //歩いた距離
    public float stepDistance = 1f; //1歩とみなす距離
    public int randomValue; // 0～10のランダム値
    public GameObject battlePanel; //バトルパネルを入れる変数
    public bool isBattleOpen = false; //バトル状態初期開いていない
    public bool isInDungeon = false; //ダンジョンかどうか


    public int randomEnemy;//敵決定用の変数
    ParameterController parameter;
    BattleMoveSelect battleMove;

    public GameObject battleMessage;
    TextMeshProUGUI message;
    BattleMessageWindow battleMessageWindow;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


        canvas = GameObject.FindGameObjectWithTag("Canvas"); //Battleパネル初期非表示なので一度親を取得する
        playerPos = player.transform.position; //プレイヤーオブジェクトの位置を代入
        battlePanel = canvas.transform.Find("Battle").gameObject; //親のCanvasから取得させる

        parameter = canvas.GetComponent<ParameterController>();
        battleMove = canvas.GetComponent<BattleMoveSelect>();

        battleMessage = battlePanel.transform.Find("BattleMessage").gameObject; //BattleMessage取得
        message = battleMessage.transform.Find("Message").GetComponent<TextMeshProUGUI>();

        battleMessageWindow = canvas.GetComponent<BattleMessageWindow>();


        //プレイヤー変数にプレイヤーオブジェクトアタッチされていない時
        if (player == null)
        {
            steps = 0;
            player = this.gameObject; //このスクリプトがアタッチされているオブジェクトを代入
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInDungeon)
            return; //ダンジョンじゃない時は動かさない

        // 毎フレームランダム値を更新（0～10）
        randomValue = Random.Range(0, 11);
        //プレイヤーが移動した距離
        float distanceMoved = Vector3.Distance(player.transform.position, playerPos);

        //1歩とみなす距離より大きい移動なら       
        if (distanceMoved >= stepDistance)
        {
            steps++; //歩いた距離をプラス
            playerPos = player.transform.position;

            // Debug.Log("歩数: " + steps);

            // 20歩以上になったら判定
            if (steps >= 20)
            {
                Debug.Log("20歩以上歩いた");


                if (randomValue <= 3)
                {
                    isBattleOpen = true;
                    Debug.Log("20歩以上歩いた & ランダム値は3以下: " + randomValue);

                    // if (isBattleOpen)
                    // {
                    //     steps = 0;
                    //     GameManager.gameState = GameState.battle; //ステータスをbattleにする
                    //     battlePanel.SetActive(isBattleOpen);
                    //     Time.timeScale = 0; //ゲーム進行スピードを0
                    //この処理コルーチンに移動させた

                    StartCoroutine(Battle()); //バトル開始
                    // }
                }
            }
        }
    }

    IEnumerator Battle()
    {
        // バトル開始
        steps = 0;
        GameManager.gameState = GameState.battle;
        battlePanel.SetActive(true);
        Time.timeScale = 0;
        randomEnemy = Random.Range(0, parameter.enemies.Count); //0~1のリストからランダムに敵を選択、乱数はリストの範囲
        string str = $"{parameter.enemies[randomEnemy].name}があらわれた\nどうする？";
        battleMessageWindow.AddMessage(str);




        // バトル中ずっと待機
        while (isBattleOpen)
        {
            battleMove.move();//バトル中にメニュー動かす処理を呼び出す
            yield return null; // 次のフレームまで待つ

        }

        // バトル終了時
        str = $"{parameter.enemies[randomEnemy].name} を倒した！";
        battleMessageWindow.AddMessage(str);
        

        yield return new WaitForSecondsRealtime(2f);
        battlePanel.SetActive(false);
        GameManager.gameState = GameState.playing;
        Time.timeScale = 1;


    }

    //ダンジョンのisTriggerに当たった時
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DungeonArea"))
        {
            isInDungeon = true;
            Debug.Log("Dungeon内に入った");
        }
    }

    //ダンジョンのisTriggerに当たらなくなった時
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("DungeonArea"))
        {
            isInDungeon = false;
            Debug.Log("Dungeon外に出た");
        }
    }
}
