using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    string _name;
    int _max_hp;
    int _hp;
    bool _is_current_player;
    bool _is_win;

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
    }

    public string GetName() => _name;
    public int GetHP() => _hp;//Œ»ÝHP‚ðŽæ“¾
    public void SetHP(int hp) => _hp = hp;//HP‚ð‘€ì‚·‚é
    public int GetMaxHP => _max_hp;//Å‘åHP‚ðŽæ“¾
    public Deck GetDeck() => _deck;//ŽRŽD‚ðŽæ“¾
    public Hands GetHands() => _hands;//ŽèŽD‚ðŽæ“¾
    public bool IsCurrentPlayer() => _is_current_player;//Ž©g‚ª“®‚¯‚é‚©‚Ç‚¤‚©‚ðŽæ“¾
    public void SetCurrentPlayer(bool played) => _is_current_player = played;//Ž©g‚ª“®‚¯‚é‚©‚Ç‚¤‚©‚ð‘€ì
    public bool GetWinning() => _is_win;//Ÿ—˜‚µ‚Ä‚¢‚é‚©Žæ“¾‚·‚é
    public void SetWinningState() => _is_win = true;//Ÿ—˜‚µ‚Ä‚¢‚éó‘Ô‚É‚·‚é

    //HP‚ð‘€ì‚·‚é//Å‘åˆÈã‚Ìê‡‚ÍÅ‘å‚É•ÏX‚·‚é
    public void AddHP(int value) {
        _hp += value;

        if (_hp >= _max_hp) _hp = _max_hp;
    }
}
