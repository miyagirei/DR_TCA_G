using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    [HideInInspector] DataController _data_controller;
    [SerializeField]List<Card> _hands_card = new List<Card>();
    [SerializeField]GameObject _card_prefab;

    [HideInInspector] float TRASH_DECIDED_HEIGHT = 0;
    [HideInInspector] float TRASH_UNDECIDED_HEIGHT = 0;
    async void Start()
    {
        _data_controller = GameObject.Find("DataController").GetComponent<DataController>();
        TRASH_DECIDED_HEIGHT = await _data_controller.GetParamValueFloat("TRASH_DECIDED_HEIGHT");
        TRASH_UNDECIDED_HEIGHT = await _data_controller.GetParamValueFloat("TRASH_UNDECIDED_HEIGHT");
    }

    void Update()
    {
        Check();
    }

    public int GetCardCount() => _hands_card.Count;

    //�J�[�\����ɂ���f�b�L���擾����
    public Deck PickDrawDeck(Deck mine) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.origin , 0.01f);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.green, 3, false);
            
        if (!hit2d.collider)
        {
            return null;
        }

        Deck selectObj = hit2d.transform.gameObject.GetComponent<Deck>();
        if (selectObj == null) {
            return null;
        }

        if (selectObj.TryGetComponent<Deck>(out Deck deck))
        {
            if (mine != deck) {
                return null;
            }

            return deck;
        }

        return null;
    }

    //�j�����Ă悵
    void Check() {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.origin);
            if (!hit2d)
            {
                return;
            }

            GameObject selectObj = hit2d.transform.gameObject;
            if (!selectObj.TryGetComponent<Card>(out Card card))
            {
                return;
            }

            //Debug.Log(card.GetName() + ":" + card.GetDamage());

        }

        if (!Input.GetKeyDown(KeyCode.A)) {
            return;
        }

        for (int i = 0; i < _hands_card.Count; i++) {
            //Debug.Log(_hands[i].GetName() + ":" + _hands[i].GetDamage());
        }
    }

    //�J�[�h�𐶐����A���񂳂���//��̂̓f�b�L�����D�Ɉڂ����Ƃ��Ɏg����
    public void CreateCard(Card origin , Vector3 pos , Vector3 scale) {
        int new_card_count = _hands_card.Count - 1;
        GameObject card_obj = Instantiate(_card_prefab, new Vector3(new_card_count * scale.x, pos.y) , Quaternion.identity);
        card_obj.transform.SetParent(this.transform);
        Card new_card = card_obj.GetComponent<Card>();
        if (origin.GetCardType() == CardType.HopeAndDespair) {
            new_card.Init(origin.GetName(), origin.GetHopeEffect(),origin.GetEffectAmount(origin.GetHopeEffect()), origin.GetCostOfHope(), 
                origin.GetDespairEffect() , origin.GetEffectAmount(origin.GetDespairEffect()) , origin.GetCostOfDespair());
        }
        else {
            new_card.Init(origin.GetName(), origin.GetEffectAmount(origin.GetEffectByCardType()), origin.GetCostByCardType(), origin.GetEffectByCardType(), origin.GetCardType());
        }

        new_card.CreatePopup();
        new_card.SetPos(new Vector3(new_card_count * scale.x, pos.y));

        _hands_card.Add(new_card);
        arrangeCards(new Vector3(pos.x, pos.y) , scale);
    }
    
    //���ݎg��ꂽ�J�[�h�����邩�ǂ����𔻒肷��
    public Card IsPlayedCard(Player player) {
        foreach(Card card in _hands_card) {
            if (!card.GetPlayed()) {
                continue;
            }

            if (player.GetNormalCondition() && !card.GetIfNormalCard()) {
                card.ReturnCard();
                Debug.Log("�ʏ��Ԃ�����J�[�h");
                continue;
            }

            if (player.GetHopeCondition() && card.GetCardType() == CardType.OnlyDespair) {
                card.ReturnCard();
                Debug.Log("��]��Ԃ���]�J�[�h");
                continue;
            }
            
            if (player.GetDespairCondition() && card.GetCardType() == CardType.OnlyHope) {
                card.ReturnCard();
                Debug.Log("��]��Ԃ���]�J�[�h");
                continue;
            }

            return card;    
        }

        return null;
    }

    //�J�[�h���̂Ă鎞�Ƀ��X�g����r������
    public void TrashCard(Card card) {
        int choiceNum = _hands_card.IndexOf(card);
        Debug.Log("Trash : " + card.GetName());
        _hands_card.RemoveAt(choiceNum);
        card.Trash();
    }

    //��ԃ_���[�W���������A�R�X�g��������J�[�h���擾����
    public Card checkHighDamageCard()
    {
        if (_hands_card.Count == 0) {
            return null;
        }
        Card high_card = new Card();
        bool change = false;
        for (int i = 0; i < _hands_card.Count; i++) {
            if(high_card.GetDamage() < _hands_card[i].GetDamage() && _hands_card.Count > _hands_card[i].GetCost()) { 
                high_card = _hands_card[i];
                change = true;
            }
        }

        if (!change) {
            return null;
        }

        return high_card;
    }

    //���ׂĂ̎�D���A�h���b�O���ł��邩�ǂ����𑀍삷��
    public void SelectedPlayable(bool playable) {
        for (int i = 0; i < _hands_card.Count; i++)
        {
            _hands_card[i].SetDraggable(playable);
            
        }
    }

    //�̂Ă�J�[�h��I�ԂƂ��ɁA�I���ł���J�[�h�̈ʒu�����炵�A�I�ׂ�悤�ɂ���
    public void ShowAvailableCards(Card exception) {
        for (int i = 0; i < _hands_card.Count; i++) {
            if (exception == _hands_card[i]) {
                continue;
            }

            if (_hands_card[i].GetDiscard())
            {
                _hands_card[i].temporarilySetPosition(new Vector3(_hands_card[i].GetPos().x, _hands_card[i].GetPos().y + TRASH_DECIDED_HEIGHT));
            }
            else {
                _hands_card[i].temporarilySetPosition(new Vector3(_hands_card[i].GetPos().x, _hands_card[i].GetPos().y + TRASH_UNDECIDED_HEIGHT));
            }

            _hands_card[i].SetCanSelected(true);
        }
    }

    //�I������Ă���J�[�h�������ȏォ�𔻒肷��
    public bool isCardSelectionValid(int cost) {
        int count = 0;
        for (int i = 0; i < _hands_card.Count; i++) {
            if (_hands_card[i].GetDiscard()) {
                count++;
            }
        }
        return count >= cost;
    }

    //�I������Ă���J�[�h�����ׂăg���b�V�����A����ȊO�̃J�[�h��I���ł��Ȃ��悤�ɂ���
    public void discardSelectedCards() {
        for (int i = _hands_card.Count - 1; i >= 0; i--)
        {
            if (_hands_card[i].GetDiscard())
            {
                TrashCard(_hands_card[i]);
                
            }
            else
            {
                _hands_card[i].SetCanSelected(false);
            }
        }
    }

    //��D�̃J�[�h�𐮗񂳂���
    public void arrangeCards(Vector3 pos , Vector3 scale) {
        float correction = (-(_hands_card.Count - 1) * scale.x) / 2;
        for (int i = 0; i < _hands_card.Count; i++) {
            _hands_card[i].SetPos( new Vector3(i * scale.x + correction + pos.x, pos.y));
            _hands_card[i].SetScale(scale);
            _hands_card[i].ReturnPos();
        }
    }

    //�����̖������z���Ă����ꍇ�A�����_���Ɏ̂Ă邩�A�����̖�����������Ă����ꍇ�A���������܂ŐV�����J�[�h������
    public void ResetHandCards(int cards , Deck deck , Vector3 pos , Vector3 scale) {
        if (cards == _hands_card.Count) {
            return;
        }

        if (cards < _hands_card.Count) {
            int diff = (_hands_card.Count) - cards;

            for (int i = 0; i < diff; i++) {
                Card card = _hands_card[Random.Range(0, _hands_card.Count)];

                TrashCard(card);
            }
        }

        if (cards > _hands_card.Count) {
            int diff = cards - (_hands_card.Count);

            for (int i = 0; i < diff; i++) {
                if (deck.GetDeckCount() <= 0) {
                    return;
                }

                CreateCard(deck.DrawDeck(), pos , scale);
            }
        }
    }

    public bool IsDraggingCard()
    {
        foreach (Card card in _hands_card)
        {
            if (card.GetDragging())
            {
                return true;
            }
        }
        return false;
    }
}
