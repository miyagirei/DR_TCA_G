using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    string _name;
    int _hp;
    bool _is_current_player;

    Deck _deck;
    Hands _hands;

    public Player(string name , int hp , bool is_current_player, Deck deck , Hands hands) {
        _name = name;
        _hp = hp;
        _is_current_player = is_current_player;
        _deck = deck;
        _hands = hands;
    }

    public string GetName() => _name;
    public int GetHP() => _hp;
    public Deck GetDeck() => _deck;
    public Hands GetHands() => _hands;
    public bool IsCurrentPlayer() => _is_current_player;
    public void SetCurrentPlayer(bool played) => _is_current_player = played;
    public void AddHP(int value) => _hp += value;
}
