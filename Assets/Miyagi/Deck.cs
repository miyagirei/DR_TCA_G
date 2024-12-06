using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    List<Card> _cards = new List<Card>();

    private void Start()
    {
        DebugAddNewCard();

    }

    public void drawDeck( List<Card> hands ) {
        if (_cards.Count == 0) {
            Debug.Log("Not Card in Deck");
            return;
        }

        Card card = _cards[Random.Range(0, _cards.Count)];

        hands.Add(card);

        int choiceNum = _cards.IndexOf(card);
        _cards.RemoveAt(choiceNum);
        Destroy(card.gameObject);
        
        Debug.Log(_cards.Count);
    }

    
    void DebugAddNewCard() {
        for (int i = 0; i < 10; i++) {
            GameObject card_obj = new GameObject(i + "test");
            card_obj.transform.SetParent(this.transform);
            Card card = card_obj.AddComponent<Card>();
            card.init("test",i);
            _cards.Add(card);
            Debug.Log(_cards[i].name);
        }
    }
}

