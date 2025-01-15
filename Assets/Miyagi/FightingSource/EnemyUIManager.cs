using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIManager : MonoBehaviour
{
    [SerializeField] Text UI_Enemy_HP;
    [SerializeField] Text UI_Enemy_Condition;
    void Start()
    {

    }

    public void Display(Player enemy)
    {
        DisplayPlayerHP(enemy);
        DisplayPlayerCondition(enemy);
    }

    void DisplayPlayerHP(Player player)
    {
        if (player == null)
        {
            return;
        }
        UI_Enemy_HP.text = player.GetName() + ":" + $"{player.GetHP()}";
    }


    void DisplayPlayerCondition(Player player)
    {
        if (player == null)
        {
            return;
        }
        string condition = "no info";

        if (player.GetNormalCondition())
        {
            condition = "Normal";
        }
        else if (player.GetHopeCondition())
        {
            condition = "Hope";
        }
        else if (player.GetDespairCondition())
        {
            condition = "Despair";
        }

        UI_Enemy_Condition.text = player.GetName() + ":" + condition;
    }

}
