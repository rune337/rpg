using UnityEngine;

// ゲーム状態を管理する列挙型
public enum GameState
{
    playing,
    gameover,
    gameclear,
    battle,
    menu,
    ending
}

public class GameManager : MonoBehaviour
{
    public static GameState gameState; //ゲームのステータス
    //ワープ先とワープ元を同じにすると入った瞬間に出てしまうのでオブジェクトを分ける
    //ワープ先のオブジェクトそのままだとぶつかって邪魔なのでコライダーオフにする
    //そのためにコライダーを取得するために対象のオブジェクト探して持ってくる
    GameObject obj; //対象のオブジェクトを探して持ってくるための変数
    Collider2D col; //コライダーいれるための変数

    void Start()
    {
        gameState = GameState.playing;

    }

    void Awake()
    {
        obj = GameObject.FindWithTag("TownEntry");
        col = obj.GetComponent<Collider2D>();

    }


    // 街のオブジェクトのワープ先を渡す
    public void WarpPlayer(Transform targetPosition)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null && targetPosition != null)
        {
            player.transform.position = targetPosition.position;
            Debug.Log(ToTown.inTown);
            col.enabled = false; //街にワープするとコライダーをオフにすることでぶつからない

        }
    }

    public void WarpPlayer2(Transform targetPosition)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null && targetPosition != null)
        {
            player.transform.position = targetPosition.position;
            Debug.Log(ToTown.inTown);
        }
    }
}
