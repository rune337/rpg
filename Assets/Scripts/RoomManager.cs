using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    public static string toRoomNumber = "fromRoom1"; //Playerが配置されるべき位置
    GameObject player; //プレイヤーの情報
    public GameObject room; //roomのプレハブ
    public static bool firstLoad = true; // 最初のロード判定


    void Start()
    {
        if (!firstLoad)
            PlayerPosition();
        else
            firstLoad = false;
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    void PlayerPosition()
    {
        GameObject[] roomDatas = GameObject.FindGameObjectsWithTag("Room");

        foreach (GameObject room in roomDatas)
        {
            RoomData r = room.GetComponent<RoomData>();

            if (r.roomName == toRoomNumber)
            {
                float posY = 1.5f;
                if (r.direction == PlayerDirection.down)
                {
                    posY = -1.5f;
                }

                player.transform.position = new Vector2(room.transform.position.x, room.transform.position.y + posY);
                break;
            }
        }




    }
}
