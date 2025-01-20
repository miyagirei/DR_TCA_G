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
    [SerializeField] Text UI_Turn_Change_Player;
    [SerializeField] Text UI_Turn_Situation;
    [SerializeField] GameObject UI_Card_Effect_Distance_Panel;
    [SerializeField] Text UI_Timer;
    [SerializeField] Button UI_Playlog_Display_Switching_Button;
    [SerializeField] GameObject UI_Playlog;
    [SerializeField] ParticleSystem UI_Particle_Side_Left;
    [SerializeField] ParticleSystem UI_Particle_Side_Right;
    private void Start()
    {
        AssingPlaylogButton();
        var module_left = UI_Particle_Side_Left.main;
        var module_right = UI_Particle_Side_Right.main;
        module_left.startColor = Color.clear;
        module_right.startColor = Color.clear;
        UI_Particle_Side_Left.Play();
        UI_Particle_Side_Right.Play();
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
        var module_left = UI_Particle_Side_Left.main;
        var module_right = UI_Particle_Side_Right.main;

        if (player.GetNormalCondition())
        {
            condition = "Normal";
            ParticleColorChange(UI_Particle_Side_Left, Color.clear) ;
            ParticleColorChange(UI_Particle_Side_Right, Color.clear);
            module_left.startColor = Color.clear;
            module_right.startColor = Color.clear;
            UI_Particle_Side_Left.GetComponent<Renderer>().enabled = false;
            UI_Particle_Side_Right.GetComponent<Renderer>().enabled = false;
        }
        else if (player.GetHopeCondition())
        {
            condition = "Hope";
            ParticleColorChange(UI_Particle_Side_Left , Color.white);
            ParticleColorChange(UI_Particle_Side_Right , Color.white);
            module_left.startColor = Color.white;
            module_right.startColor = Color.white;
            UI_Particle_Side_Left.GetComponent<Renderer>().enabled = true;
            UI_Particle_Side_Right.GetComponent<Renderer>().enabled = true;
        }
        else if (player.GetDespairCondition())
        {
            condition = "Despair";
            ParticleColorChange(UI_Particle_Side_Left, Color.black);
            ParticleColorChange(UI_Particle_Side_Right, Color.black);
            module_left.startColor = Color.black;
            module_right.startColor = Color.black;
            UI_Particle_Side_Left.gameObject.GetComponent<Renderer>().enabled = true;
            UI_Particle_Side_Right.gameObject.GetComponent<Renderer>().enabled = true;
        }

        UI_Player_Condition.text = player.GetName() + ":" + condition;
    }

    void ParticleColorChange(ParticleSystem particle , Color color) {
        if (particle.main.startColor.color == color) {
            return;
        }

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particle.particleCount];
        int particle_count = particle.GetParticles(particles);

        for (int i = 0; i < particle_count; i++) {
            particles[i].startColor = color;
        }

        particle.SetParticles(particles, particle_count);
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

    public void DisplayTurnChangePanel(Player turn_player , PlayingSituation situation ,bool active) {
        UI_Turn_Change_Player.text = turn_player.GetName() + "のターン";
        switch (situation) {
            case PlayingSituation.Hopeful:
                UI_Turn_Situation.text = "希望ターン";
                UI_Turn_Situation.color = Color.yellow;
                break;
            case PlayingSituation.Desperate:
                UI_Turn_Situation.text = "絶望ターン";
                UI_Turn_Situation.color = new Color(1,0,1);
                break;
        }
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
        UI_Timer.text = "残り" + (int)timer + "秒";
        UI_Timer.gameObject.SetActive(active);
    }

    //PlaylogButtonに
    void AssingPlaylogButton()
    {
        AssignButtonAction(UI_Playlog_Display_Switching_Button, () => PlaylogDisplaySwitching());
    }
    void PlaylogDisplaySwitching() {
        UI_Playlog.SetActive(!UI_Playlog.activeSelf);
    }
}
