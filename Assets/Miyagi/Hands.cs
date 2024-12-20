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

    public void addHands(Player player,Deck deck , Vector2 pos) {
        //deck.DrawDeck(_hands);
        //_hands.Add(deck.DrawDeck());
        DisplayCard(player , deck.DrawDeck() , pos.y);
    }

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

    public void DisplayCard(Player player, Card origin , float y) {
        int new_card_count = _hands_card.Count - 1;
        GameObject card_obj = Instantiate(_card_prefab, new Vector3(new_card_count * 2, y) , Quaternion.identity);
        card_obj.transform.SetParent(this.transform);
        Card new_card = card_obj.GetComponent<Card>();
        new_card.Init(origin.GetName(), origin.GetDamage() , origin.GetCost());

        new_card.SetPos(new Vector3(new_card_count * 2, y));

        _hands_card.Add(new_card);

        arrangeCards(y);
    }

    public Card IsPlayedCard() {
        foreach(Card card in _hands_card) {

            if (card.GetPlayed()) {
                return card;
            }
        }

        return null;
    }

    //カードを捨てる時にリストから排除する
    public void TrashCard(Card card) {
        int choiceNum = _hands_card.IndexOf(card);
        _hands_card.RemoveAt(choiceNum);
        card.Trash();
    }

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

    public void SelectedPlayable(bool playable) {
        for (int i = 0; i < _hands_card.Count; i++)
        {
            _hands_card[i].SetDraggable(playable);
            
        }
    }

    public void ShowAvailableCards() {
        for (int i = 0; i < _hands_card.Count; i++) {
            if (IsPlayedCard() == _hands_card[i]) {
                continue;
            }

            _hands_card[i].temporarilySetPosition(new Vector3(_hands_card[i].GetPos().x , _hands_card[i].GetPos().y + 1));
            _hands_card[i].SetCanSelected(true);
        }
    }

    public bool isCardSelectionValid(int cost) {
        int count = 0;
        for (int i = 0; i < _hands_card.Count; i++) {
            if (_hands_card[i].GetDiscard()) {
                count++;
            }
        }
        return count >= cost;
    }

    public void discardSelectedCards() {
        for (int i = 0; i < _hands_card.Count; i++)
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

    public void arrangeCards(float y) {
        float correction = -(_hands_card.Count - 1) / 2;
        for (int i = 0; i < _hands_card.Count; i++) {
            _hands_card[i].SetPos( new Vector3(i * 2 + correction, y));
            _hands_card[i].ReturnPos();
        }
    }
}
