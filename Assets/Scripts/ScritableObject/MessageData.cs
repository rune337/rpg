using UnityEngine;

[System.Serializable]
public class Message
{
    public string name;
    //インスペクター上で最小3行、最大10行で表示
    [TextArea(3, 10)]
    public string message;
}

[CreateAssetMenu(fileName = "MsgData",menuName = "CreateMsg")]
public class MessageData : ScriptableObject
{
    public Message[] msgData;
}
