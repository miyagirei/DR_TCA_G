using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] Text UI_Player_HP;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Display(Player player) {
        DisplayPlayerHP(player);
    }

    void DisplayPlayerHP(Player player) {
        if (player == null) {
            return;
        }
        UI_Player_HP.text = player.GetName()+ ":" + $"{player.GetHP()}";
    }

}
