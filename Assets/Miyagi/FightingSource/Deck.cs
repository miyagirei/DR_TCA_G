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
    public Vector3 GetPos() => this.transform.position;

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

    public int GetDeckCount() => _deck_card.Count;
    
    //デバック用カード補充
    void DebugAddNewCard() {
        for (int i = 0; i < 10; i++)
        {
            GameObject card_obj = new GameObject(i + "test");
            card_obj.transform.SetParent(this.transform);
            Card card = card_obj.AddComponent<Card>();
            int amount = Random.Range(1, 5);
            int effect_choice = Random.Range(0, 5);
            string effect = card.GetEffectNumber(effect_choice);
            card.Init("normal", amount, amount, effect , CardType.Normal);
            _deck_card.Add(card);
        }        
        
        for (int i = 0; i < 5; i++)
        {
            GameObject card_obj = new GameObject(i + "hope");
            card_obj.transform.SetParent(this.transform);
            Card card = card_obj.AddComponent<Card>();
            int amount = Random.Range(1, 5);
            int effect_choice = Random.Range(0, 5);
            string effect = card.GetEffectNumber(effect_choice);
            card.Init("hope", amount, amount, effect , CardType.OnlyHope , amount * 2);
            _deck_card.Add(card);
        }        
        
        for (int i = 0; i < 5; i++)
        {
            GameObject card_obj = new GameObject(i + "despair");
            card_obj.transform.SetParent(this.transform);
            Card card = card_obj.AddComponent<Card>();
            int amount = Random.Range(1, 5);
            int effect_choice = Random.Range(0, 5);
            string effect = card.GetEffectNumber(effect_choice);
            card.Init("despair", amount, amount, effect , CardType.OnlyDespair , amount * 2);
            _deck_card.Add(card);
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject card_obj = new GameObject(i + "hope_despair");
            card_obj.transform.SetParent(this.transform);
            Card card = card_obj.AddComponent<Card>();
            int h_amount = Random.Range(1, 5);
            int d_amount = Random.Range(1, 5);
            int hope_choice = Random.Range(0, 5);
            int despair_choice;
            do
            {
                despair_choice = Random.Range(0, 5);
            } while (hope_choice == despair_choice);

            string hope_effect = card.GetEffectNumber(hope_choice);
            string despair_effect = card.GetEffectNumber(despair_choice);
            card.Init("hope_despair", hope_effect, h_amount,h_amount * 2, h_amount, despair_effect , d_amount,d_amount * 2, d_amount);
            _deck_card.Add(card);
        }
    }
}

