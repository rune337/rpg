using UnityEngine;



public class PlayerStatus : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    void Start()
    {
        // ParameterController を探す
        ParameterController parameterController = FindFirstObjectByType<ParameterController>();
        if (parameterController != null)
        {
            currentHP = parameterController.GetMaxHP(0); // 最初は maxHP を代入
        }
    }

    public void RestoreHP(int amount)
    {
        // FindObjectOfType は非推奨なので FindFirstObjectByType に変更
        // ParameterController parameterController = FindObjectOfType<ParameterController>();
        ParameterController parameterController = FindFirstObjectByType<ParameterController>();
        int maxHP = parameterController.GetMaxHP(0);

        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;
        Debug.Log($"HP回復: {amount} → 現在HP: {currentHP}");
    }


   public void Equip(ItemData item)
{
    Debug.Log($"装備: {item.itemName} 効果: {item.attackBonus}, {item.defenseBonus}, {item.moveSpeedBonus}");


    ParameterController parameterController = FindFirstObjectByType<ParameterController>();
    if (parameterController != null)
    {
        parameterController.EquipItem(item, 0);
    }
}

}
