using UnityEngine;

public class Battle : MonoBehaviour
{

    GameObject battle;
    EncountManger playerEncount;
    bool isBattleOpen;


    void Start()
    {
        battle = GameObject.FindGameObjectWithTag("Player");
        playerEncount = battle.GetComponent<EncountManger>();
        isBattleOpen = playerEncount.isBattleOpen;

        if (isBattleOpen)
            BattleComparison();

    }

    // Update is called once per frame
    void Update()
    {


    }

    void BattleComparison()
    {
        
    }
}
