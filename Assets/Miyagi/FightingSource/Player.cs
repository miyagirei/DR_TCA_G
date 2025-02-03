using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    string _name;
    int _max_hp;
    int _hp;
    bool _is_current_player;
    bool _is_win;
    bool _is_hope;
    bool _is_despair;
    Vector3 _card_pos;
    Vector3 _card_scale;

    Deck _deck;
    Hands _hands;
    CharacterType _character_type;

    public Player(string name , int hp , bool is_current_player, Deck deck , Hands hands , Vector3 card_pos , Vector3 card_scale , CharacterType character) {
        _name = name;
        _max_hp = hp;
        _hp = hp;
        _is_current_player = is_current_player;
        _is_win = false;
        _is_hope = false;
        _is_despair = false;
        _card_pos = card_pos;
        _card_scale = card_scale;
        _deck = deck;
        _hands = hands;
        _character_type = character;
    }

    public string GetName() => _name;
    public int GetHP() => _hp;//現在HPを取得
    public void SetHP(int hp) => _hp = hp;//HPを操作する
    public int GetMaxHP() => _max_hp;//最大HPを取得
    public Deck GetDeck() => _deck;//山札を取得
    public Hands GetHands() => _hands;//手札を取得
    public bool IsCurrentPlayer() => _is_current_player;//自身が動けるかどうかを取得
    public void SetCurrentPlayer(bool played) => _is_current_player = played;//自身が動けるかどうかを操作
    public bool GetWinning() => _is_win;//勝利しているか取得する
    public void SetWinningState() => _is_win = true;//勝利している状態にする
    public bool GetNormalCondition() => !_is_hope && !_is_despair;//普通状態かどうかを取得
    public bool GetHopeCondition() => _is_hope;//希望状態かどうかを取得
    public bool GetDespairCondition() => _is_despair;//絶望状態かどうかを取得
    public Vector3 GetCardPos() => _card_pos;//カードの設置位置を取得
    public Vector3 GetCardScale() => _card_scale;//カードの設置サイズを取得
    public void SetCharacterType(CharacterType character) => _character_type = character;//キャラクタータイプを設定する
    public CharacterType GetCharacterType() => _character_type;//キャラクタータイプを取得する
    //HPを操作する//最大以上の場合は最大に変更する
    public void AddHP(int value) {
        _hp += value;

        if (_hp >= _max_hp) _hp = _max_hp;
    }

    //希望状態にする
    public void SetHope() {
        _is_hope = true;
        _is_despair = false;
    }    
    
    //絶望状態にする
    public void SetDespair() {
        _is_hope = false;
        _is_despair = true;
    }    
    
    //普通状態にする
    public void SetNormal() {
        _is_hope = false;
        _is_despair = false;
    }
}
