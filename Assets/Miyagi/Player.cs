using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    string _name;
    int _max_hp;
    int _hp;
    bool _is_current_player;

    Deck _deck;
    Hands _hands;

    public Player(string name , int hp , bool is_current_player, Deck deck , Hands hands) {
        _name = name;
        _max_hp = hp;
        _hp = hp;
        _is_current_player = is_current_player;
        _deck = deck;
        _hands = hands;
    }

    public string GetName() => _name;
    public int GetHP() => _hp;//現在HPを取得
    public int GetMaxHP => _max_hp;//最大HPを取得
    public Deck GetDeck() => _deck;//山札を取得
    public Hands GetHands() => _hands;//手札を取得
    public bool IsCurrentPlayer() => _is_current_player;//自身が動けるかどうかを取得
    public void SetCurrentPlayer(bool played) => _is_current_player = played;//自身が動けるかどうかを操作

    //HPを操作する//最大以上の場合は最大に変更する
    public void AddHP(int value) {
        _hp += value;

        if (_hp >= _max_hp) _hp = _max_hp;
    }
}
