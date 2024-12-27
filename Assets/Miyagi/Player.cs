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
    public int GetHP() => _hp;//����HP���擾
    public int GetMaxHP => _max_hp;//�ő�HP���擾
    public Deck GetDeck() => _deck;//�R�D���擾
    public Hands GetHands() => _hands;//��D���擾
    public bool IsCurrentPlayer() => _is_current_player;//���g�������邩�ǂ������擾
    public void SetCurrentPlayer(bool played) => _is_current_player = played;//���g�������邩�ǂ����𑀍�

    //HP�𑀍삷��//�ő�ȏ�̏ꍇ�͍ő�ɕύX����
    public void AddHP(int value) {
        _hp += value;

        if (_hp >= _max_hp) _hp = _max_hp;
    }
}
