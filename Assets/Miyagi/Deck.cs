using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    List<Card> _cards = new List<Card>();

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            _cards.Add( new Card("test", i) );
            Debug.Log(_cards[i]._card_name + ":" + _cards[i]._damage_dealt);
        }

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
        Debug.Log(_cards.Count);
    }

    
}
