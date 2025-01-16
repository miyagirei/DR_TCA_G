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
    [SerializeField] GameObject UI_Card_Effect_Distance_Panel;
    [SerializeField] Text UI_Timer;
    [SerializeField] Button UI_Playlog_Display_Switching_Button;
    [SerializeField] GameObject UI_Playlog;

    private void Start()
    {
        AssingPlaylogButton();
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

    public void DisplayTurnChangeButton(bool active) {
        if (!active)
        {
            UI_Player_Turn_Change_Button.gameObject.SetActive(active);
            return;
        }
        UI_Player_Turn_Change_Button.gameObject.SetActive(active);
    }

    public void DisplayTurnChangePanel(Player turn_player , bool active) {
        Text turn_text = UI_Turn_Change_Panel.GetComponentInChildren<Text>();
        if (turn_text == null) {
            return;
        }
        turn_text.text = turn_player.GetName() + "ÇÃÉ^Å[Éì";
        UI_Turn_Change_Panel.SetActive(active);
    }

    public void DisplayCardEffectDistance(float distance , bool active) {
        if (!active) {
            UI_Card_Effect_Distance_Panel.SetActive(active);
            return;
        }

        float x = UI_Card_Effect_Distance_Panel.gameObject.transform.localScale.x;
        float z = UI_Card_Effect_Distance_Panel.gameObject.transform.localScale.z;

        Vector3 ui_position = Camera.main.WorldToScreenPoint(new Vector3(0 , -4 + distance));
        UI_Card_Effect_Distance_Panel.GetComponent<RectTransform>().sizeDelta = new Vector2(ui_position.x , Screen.height - ui_position.y);// new Vector2(0 , Screen.height - (distance + 4));
        
        UI_Card_Effect_Distance_Panel.SetActive(active);
    }

    public void DisplayTimer(float timer , bool active) {
        UI_Timer.text = "écÇË" + (int)timer + "ïb";
        UI_Timer.gameObject.SetActive(active);
    }

    //PlaylogButtonÇ…
    void AssingPlaylogButton()
    {
        AssignButtonAction(UI_Playlog_Display_Switching_Button, () => PlaylogDisplaySwitching());
    }
    void PlaylogDisplaySwitching() {
        UI_Playlog.SetActive(!UI_Playlog.activeSelf);
    }
}
