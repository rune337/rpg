using UnityEngine;

public class ToTown : MonoBehaviour
{

    public GameManager gameManager; // Inspectorでアタッチ
    public Transform warpTarget;    // 空の移動先オブジェクト 街に移動するためのオブジェクト
    public Transform warpTarget2;    // 空の移動先オブジェクト フィールドに移動するためのオブジェクト
    public static bool inTown = false; //街移動フラグ

    void Start()
    {
    }


    private void OnTriggerEnter2D(Collider2D other)
    {


        if (other.CompareTag("Player") && !inTown)
        {
            inTown = true; //街にいないとき触れた瞬間にオンにする
            gameManager.WarpPlayer(warpTarget);
        }
        else if (other.CompareTag("Player") && inTown)
        {
            inTown = false; //街にいるとき触れた瞬間にオフにする
            gameManager.WarpPlayer2(warpTarget2);
        }
    }
}
