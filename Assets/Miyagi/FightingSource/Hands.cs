using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    [SerializeField]List<Card> _hands_card = new List<Card>();
    [SerializeField]GameObject _card_prefab;


    void Start()
    {
        
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
    public void CreateCard(Card origin , float y) {
        int new_card_count = _hands_card.Count - 1;
        GameObject card_obj = Instantiate(_card_prefab, new Vector3(new_card_count * 2, y) , Quaternion.identity);
        card_obj.transform.SetParent(this.transform);
        Card new_card = card_obj.GetComponent<Card>();
        if (origin.GetIfNormalCard())
        {
            new_card.Init(origin.GetName(), origin.GetEffectAmount(origin.GetEffect()), origin.GetCost(), origin.GetEffect());
        }
        else if (!origin.GetIfNormalCard()) {
            new_card.Init(origin.GetName(), origin.GetHopeEffect(),origin.GetEffectAmount(origin.GetHopeEffect()), origin.GetCostOfHope(), 
                origin.GetDespairEffect() , origin.GetEffectAmount(origin.GetDespairEffect()) , origin.GetCostOfDespair());
        }
        new_card.CreatePopup();
        new_card.SetPos(new Vector3(new_card_count * 2, y));

        _hands_card.Add(new_card);

        arrangeCards(y);
    }
    
    //���ݎg��ꂽ�J�[�h�����邩�ǂ����𔻒肷��
    public Card IsPlayedCard(Player player) {
        foreach(Card card in _hands_card) {
            if (!card.GetPlayed()) {
                continue;
            }

            if (player.GetNormalCondition() && !card.GetIfNormalCard()) {
                card.ReturnCard();
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

            _hands_card[i].temporarilySetPosition(new Vector3(_hands_card[i].GetPos().x , _hands_card[i].GetPos().y + 1));
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
    public void arrangeCards(float y) {
        float correction = -(_hands_card.Count - 1) / 2;
        for (int i = 0; i < _hands_card.Count; i++) {
            _hands_card[i].SetPos( new Vector3(i * 2 + correction, y));
            _hands_card[i].ReturnPos();
        }
    }

    //�����̖������z���Ă����ꍇ�A�����_���Ɏ̂Ă邩�A�����̖�����������Ă����ꍇ�A���������܂ŐV�����J�[�h������
    public void ResetHandCards(int cards , Deck deck , Vector3 pos) {
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

                CreateCard(deck.DrawDeck(), pos.y);
            }
        }
    }
}
