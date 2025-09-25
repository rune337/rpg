using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerDirection
{
    up,
    down
}

public class RoomData : MonoBehaviour
{
    public string roomName;
    public string nextRoomName;
    public PlayerDirection direction;
    public string nextScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            ChangeScene();
    }

    public void ChangeScene()
    {
        RoomManager.toRoomNumber = nextRoomName;
        SceneManager.LoadScene(nextScene);
    }
}