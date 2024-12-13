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

    public void addHands(Deck deck) {
        //deck.DrawDeck(_hands);
        //_hands.Add(deck.DrawDeck());
        DisplayCard(deck.DrawDeck());
    }

    public Deck PickDrawDeck(Deck mine) {
        if (!Input.GetMouseButtonDown(0))
        {
            return null;
        }

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

    public void DisplayCard(Card origin) {
        int new_card_count = _hands_card.Count - 1;
        GameObject card_obj = Instantiate(_card_prefab, new Vector3(new_card_count * 2, -4) , Quaternion.identity);
        card_obj.transform.SetParent(this.transform);
        Card new_card = card_obj.GetComponent<Card>();
        new_card.Init(origin.GetName(), origin.GetDamage());

        new_card.SetPos(new Vector3(new_card_count * 2, -4));

        _hands_card.Add(new_card);
    }

    public Card IsPlayedCard() {
        foreach(Card card in _hands_card) {

            if (card.GetPlayed()) {
                return card;
            }
        }

        return null;
    }

    public void TrashCard(Card card) {
        int choiceNum = _hands_card.IndexOf(card);
        _hands_card.RemoveAt(choiceNum);
        card.Trash();
    }
}
