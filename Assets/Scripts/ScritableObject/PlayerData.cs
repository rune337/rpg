using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Game/PlayerData")]
public class PlayerData : ScriptableObject
{
    public string playerName;      // プレイヤーの名前
    public int maxHP;             // 最大HP
    public int attack;            // 攻撃力
    public int defense;           // 防御力
    public float moveSpeed;       // 移動速度
    public Sprite sprite;         // 表示用スプライト
    public GameObject prefab;     // 敵を生成するときのPrefab
}
