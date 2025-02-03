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
    public int GetHP() => _hp;//����HP���擾
    public void SetHP(int hp) => _hp = hp;//HP�𑀍삷��
    public int GetMaxHP() => _max_hp;//�ő�HP���擾
    public Deck GetDeck() => _deck;//�R�D���擾
    public Hands GetHands() => _hands;//��D���擾
    public bool IsCurrentPlayer() => _is_current_player;//���g�������邩�ǂ������擾
    public void SetCurrentPlayer(bool played) => _is_current_player = played;//���g�������邩�ǂ����𑀍�
    public bool GetWinning() => _is_win;//�������Ă��邩�擾����
    public void SetWinningState() => _is_win = true;//�������Ă����Ԃɂ���
    public bool GetNormalCondition() => !_is_hope && !_is_despair;//���ʏ�Ԃ��ǂ������擾
    public bool GetHopeCondition() => _is_hope;//��]��Ԃ��ǂ������擾
    public bool GetDespairCondition() => _is_despair;//��]��Ԃ��ǂ������擾
    public Vector3 GetCardPos() => _card_pos;//�J�[�h�̐ݒu�ʒu���擾
    public Vector3 GetCardScale() => _card_scale;//�J�[�h�̐ݒu�T�C�Y���擾
    public void SetCharacterType(CharacterType character) => _character_type = character;//�L�����N�^�[�^�C�v��ݒ肷��
    public CharacterType GetCharacterType() => _character_type;//�L�����N�^�[�^�C�v���擾����
    //HP�𑀍삷��//�ő�ȏ�̏ꍇ�͍ő�ɕύX����
    public void AddHP(int value) {
        _hp += value;

        if (_hp >= _max_hp) _hp = _max_hp;
    }

    //��]��Ԃɂ���
    public void SetHope() {
        _is_hope = true;
        _is_despair = false;
    }    
    
    //��]��Ԃɂ���
    public void SetDespair() {
        _is_hope = false;
        _is_despair = true;
    }    
    
    //���ʏ�Ԃɂ���
    public void SetNormal() {
        _is_hope = false;
        _is_despair = false;
    }
}
