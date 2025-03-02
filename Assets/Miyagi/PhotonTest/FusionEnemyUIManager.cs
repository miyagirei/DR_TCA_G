using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
public class FusionEnemyUIManager : NetworkBehaviour
{
    [SerializeField] Text UI_Enemy_HP;
    [SerializeField] Slider UI_Enemy_HP_Image;
    [SerializeField] SpriteRenderer Image_Enemy;

    int _current_hp;
    float _hp_cooltime;

    bool _set_image = false;
    bool _spawn = false;
    public override void Spawned()
    {
        _set_image = false;
        _spawn = false;
        if (GetEnemyPlayer() != PlayerRef.None)
        {
            if (Runner.GetPlayerObject(Runner.LocalPlayer) != null && Runner.GetPlayerObject(GetEnemyPlayer()))
            {

                NetworkObject other_player_obj = Runner.GetPlayerObject(GetEnemyPlayer());

                ChangePlayerImage(other_player_obj.GetComponent<FusionPlayer>().GetCharacterType());
                Debug.Log("Succsess:ChangeEnemyImage");

                _set_image = true;
            }
        }

        _spawn = true;
    }

    private void Update()
    {
        if (!_spawn)
        {
            return;
        }

        if (GetEnemyPlayer() != PlayerRef.None)
        {
            if (Runner.GetPlayerObject(Runner.LocalPlayer) != null && !_set_image && Runner.GetPlayerObject(GetEnemyPlayer()))
            {

                NetworkObject other_player_obj = Runner.GetPlayerObject(GetEnemyPlayer());

                ChangePlayerImage(other_player_obj.GetComponent<FusionPlayer>().GetCharacterType());
                Debug.Log("Succsess:ChangeEnemyImage");

                _set_image = true;
            }
        }
    }

    public void Display(FusionPlayer enemy)
    {
        DisplayPlayerHP(enemy);
    }

    void DisplayPlayerHP(FusionPlayer player)
    {
        if (player.GetName() == null)
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


    public void ChangePlayerImage(CharacterType character, bool hope = false, bool despair = false)
    {
        CharacterTypeInfomation character_info = new CharacterTypeInfomation();
        Image_Enemy.sprite = TextureToSprite(Resources.Load<Texture2D>(character_info.GetCharacterFile(character, hope, despair)));
    }

    Sprite TextureToSprite(Texture2D texture)
    {
        return Sprite.Create(
            texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 1.0f)
        );
    }

    PlayerRef GetEnemyPlayer()
    {
        foreach (var player in Runner.ActivePlayers)
        {
            if (player != Runner.LocalPlayer)
            {
                return player;
            }
        }

        return PlayerRef.None;
    }
}
