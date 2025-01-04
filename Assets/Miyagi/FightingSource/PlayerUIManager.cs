using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] Text UI_Player_HP;
    [SerializeField] Text UI_Player_Condition;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Display(Player player) {
        DisplayPlayerHP(player);
        DisplayPlayerCondition(player);
    }

    void DisplayPlayerHP(Player player) {
        if (player == null) {
            return;
        }
        UI_Player_HP.text = player.GetName()+ ":" + $"{player.GetHP()}";
    }


    void DisplayPlayerCondition(Player player) {
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
        else if (player.GetDespairCondition()) {
            condition = "Despair";
        }

        UI_Player_Condition.text = player.GetName() + ":" + condition;
    }
}
