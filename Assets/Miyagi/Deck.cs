using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField]List<Card> _deck_card = new List<Card>();
    private void Start()
    {
        DebugAddNewCard();

    }

    public Card DrawDeck( ) {
        if (_deck_card.Count == 0) {
            Debug.Log("Not Card in Deck");
            return null;
        }

        Card card = _deck_card[Random.Range(0, _deck_card.Count)];


        int choiceNum = _deck_card.IndexOf(card);
        _deck_card.RemoveAt(choiceNum);
        Destroy(card.gameObject);

        return card;
    }

    
    void DebugAddNewCard() {
        for (int i = 0; i < 10; i++) {
            GameObject card_obj = new GameObject(i + "test");
            card_obj.transform.SetParent(this.transform);
            Card card = card_obj.AddComponent<Card>();
            card.Init("test",i , i);
            _deck_card.Add(card);
            //Debug.Log(_cards[i].name);
        }
    }
}

