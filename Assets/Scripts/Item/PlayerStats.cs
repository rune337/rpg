[System.Serializable]
public class PlayerStats
{
    public string name;
    public int hp;
    public int attack;
    public int defense;
    public float speed;
    public PlayerData data;

    public int attackBonus = 0;
    public int defenseBonus = 0;
    public float speedBonus = 0f;

    public void RecalculateStats()
    {
        attack = data.attack + attackBonus;
        defense = data.defense + defenseBonus;
        speed = data.moveSpeed + speedBonus;
    }
}
