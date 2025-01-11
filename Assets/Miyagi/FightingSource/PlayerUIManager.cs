using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] Text UI_Player_HP;
    [SerializeField] Text UI_Player_Condition;
    [SerializeField] Button UI_Player_Turn_Change_Button;
    [SerializeField] GameObject UI_Turn_Change_Panel;

    bool _view_turn_change_panel = false;

    void Start()
    {

    }

    public void Display(Player player)
    {
        DisplayPlayerHP(player);
        DisplayPlayerCondition(player);
    }

    void DisplayPlayerHP(Player player)
    {
        if (player == null)
        {
            return;
        }
        UI_Player_HP.text = player.GetName() + ":" + $"{player.GetHP()}";
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

        UI_Player_Condition.text = player.GetName() + ":" + condition;
    }

    void AssignButtonAction(Button button, System.Action action)
    {
        if (button != null && action != null)
        {
            button.onClick.AddListener(() => action());
        }
    }

    public void AssingTurnChangeButton(System.Action action)
    {
        AssignButtonAction(UI_Player_Turn_Change_Button, action);
    }

    public void DisplayTurnChangePanel(Player turn_player , bool active) {
        Text turn_text = UI_Turn_Change_Panel.GetComponentInChildren<Text>();
        if (turn_text == null) {
            return;
        }
        turn_text.text = turn_player.GetName() + "ÇÃÉ^Å[Éì";

        if (_view_turn_change_panel) {
            
        }
        UI_Turn_Change_Panel.SetActive(active);
    }
}
