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

    Deck _deck;
    Hands _hands;

    public Player(string name , int hp , bool is_current_player, Deck deck , Hands hands) {
        _name = name;
        _max_hp = hp;
        _hp = hp;
        _is_current_player = is_current_player;
        _deck = deck;
        _hands = hands;
        _is_win = false;
        _is_hope = false;
        _is_despair = false;
    }

    public string GetName() => _name;
    public int GetHP() => _hp;//Œ»İHP‚ğæ“¾
    public void SetHP(int hp) => _hp = hp;//HP‚ğ‘€ì‚·‚é
    public int GetMaxHP => _max_hp;//Å‘åHP‚ğæ“¾
    public Deck GetDeck() => _deck;//RD‚ğæ“¾
    public Hands GetHands() => _hands;//èD‚ğæ“¾
    public bool IsCurrentPlayer() => _is_current_player;//©g‚ª“®‚¯‚é‚©‚Ç‚¤‚©‚ğæ“¾
    public void SetCurrentPlayer(bool played) => _is_current_player = played;//©g‚ª“®‚¯‚é‚©‚Ç‚¤‚©‚ğ‘€ì
    public bool GetWinning() => _is_win;//Ÿ—˜‚µ‚Ä‚¢‚é‚©æ“¾‚·‚é
    public void SetWinningState() => _is_win = true;//Ÿ—˜‚µ‚Ä‚¢‚éó‘Ô‚É‚·‚é
    public bool GetNormalCondition() => !_is_hope && !_is_despair;//•’Êó‘Ô‚©‚Ç‚¤‚©‚ğæ“¾
    public bool GetHopeCondition() => _is_hope;//Šó–]ó‘Ô‚©‚Ç‚¤‚©‚ğæ“¾
    public bool GetDespairCondition() => _is_despair;//â–]ó‘Ô‚©‚Ç‚¤‚©‚ğæ“¾

    //HP‚ğ‘€ì‚·‚é//Å‘åˆÈã‚Ìê‡‚ÍÅ‘å‚É•ÏX‚·‚é
    public void AddHP(int value) {
        _hp += value;

        if (_hp >= _max_hp) _hp = _max_hp;
    }

    //Šó–]ó‘Ô‚É‚·‚é
    public void SetHope() {
        _is_hope = true;
        _is_despair = false;
    }    
    
    //â–]ó‘Ô‚É‚·‚é
    public void SetDespair() {
        _is_hope = false;
        _is_despair = true;
    }    
    
    //•’Êó‘Ô‚É‚·‚é
    public void SetNormal() {
        _is_hope = false;
        _is_despair = false;
    }
}
