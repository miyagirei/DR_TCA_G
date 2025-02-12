using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIManager : MonoBehaviour
{
    [SerializeField] Text UI_Enemy_HP;
    [SerializeField] Slider UI_Enemy_HP_Image;
    [SerializeField] Text UI_Enemy_Condition;
    [SerializeField] SpriteRenderer Image_Enemy;

    int _current_hp;
    float _hp_cooltime;
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

        UI_Enemy_HP.text = "" + player.GetHP();
        if (_current_hp > player.GetHP() * 10)
        {
            _hp_cooltime += Time.deltaTime;
            if (_hp_cooltime >= 0.1)
            {
                _hp_cooltime = 0;
                _current_hp--;
            }
        }
        else
        {
            _current_hp = player.GetHP() * 10;
        }

        UI_Enemy_HP_Image.value = _current_hp;
        UI_Enemy_HP_Image.maxValue = player.GetMaxHP() * 10;
        UI_Enemy_HP_Image.minValue = 0;
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

    public void ChangePlayerImage(CharacterType character)
    {
        CharacterTypeInfomation character_info = new CharacterTypeInfomation();
        Image_Enemy.sprite = TextureToSprite(Resources.Load<Texture2D>(character_info.GetCharacterFile(character)));
    }

    Sprite TextureToSprite(Texture2D texture)
    {
        return Sprite.Create(
            texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 1.0f)
        );
    }
}
