using UnityEngine;

public class RoofHide : MonoBehaviour
{
    private GameObject player; //Playerのゲームオブジェクトを入れる変数
    private SpriteRenderer playerSprite; //PlayerのSpriteRendererを入れる変数
    private int originalOrder = 3; // 元のOrder in Layer


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //playerタグのオブジェクトを探す
        player = GameObject.FindGameObjectWithTag("Player");

        playerSprite = player.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("屋根の下");
        if (other.CompareTag("Player") && playerSprite != null)
        {
            playerSprite.sortingOrder = 0;
        }

    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerSprite != null)
        {
            playerSprite.sortingOrder = originalOrder; // 元に戻す
        }
    }
}
