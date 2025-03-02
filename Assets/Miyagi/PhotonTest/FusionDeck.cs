using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FusionDeck : MonoBehaviour
{
    string _name = "";
    public void SetName(string name) => _name = name;
    public string GetName() => _name;

    [SerializeField] bool playerDeck;
    [SerializeField] List<Card> _deck_card = new List<Card>();

    private CardLoader cardLoader;
    [SerializeField]private List<CardData> _deckList = new List<CardData>();
    private void Start()
    {
        //if (playerDeck)
        //{
        //    cardLoader = GetComponent<CardLoader>();
        //    if (cardLoader != null)
        //    {
        //        _deckList = cardLoader.LoadCardDeck("deck" + PlayerPrefs.GetInt("SelectedDeck", 0)); // deck1.json を読み込む(一旦デバッグとして１つ目のデッキを読み込む)
        //    }
        //    else
        //    {
        //        Debug.LogError("CardLoader スクリプトが見つかりません");
        //    }
        //    AddDeckCard();
        //}
        //else
        //{
        //    DebugAddNewCard();
        //}
    }
    public void SetList(List<CardData> list) {
        _deckList = list;
        Debug.Log(_deckList[0].card_name + ":SetList");
        AddDeckCard();
    }

    public Vector3 GetPos() => this.transform.position;

    public Card DrawDeck()
    {
        if (_deck_card.Count == 0)
        {
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

    void AddDeckCard()
    {
        int index = 0;
        foreach (var data in _deckList)
        {
            GameObject card_obj = new GameObject(index + data.card_name.ToString());
            card_obj.transform.SetParent(this.transform);
            Card card = card_obj.AddComponent<Card>();
            string effect;
            Debug.Log(data.type + "data_type");
            switch (data.type.ToString())
            {
                case "Normal":
                    effect = data.normal_effect.ToString();
                    Debug.Log(effect + " : effectType");
                    card.Init(data.card_name.ToString(), CardType.Normal, effect, data.normal_amount, data.normal_cost, data.restrictions.ToString(), data.restrictions_amount);
                    _deck_card.Add(card);

                    break;
                case "OnlyDespair":
                    effect = data.despair_effect.ToString();
                    card.Init(data.card_name.ToString(), CardType.OnlyDespair, effect, data.despair_amount, data.despair_cost, data.restrictions.ToString(), data.restrictions_amount, data.despair_bonus_amount);
                    _deck_card.Add(card);

                    break;
                case "OnlyHope":
                    effect = data.hope_effect.ToString();
                    card.Init(data.card_name.ToString(), CardType.OnlyHope, effect, data.hope_amount, data.hope_cost, data.restrictions.ToString(), data.restrictions_amount, data.hope_bonus_amount);
                    _deck_card.Add(card);

                    break;
                case "HopeAndDespair":
                    string hope_effect = data.hope_effect.ToString();
                    string despair_effect = data.despair_effect.ToString();
                    card.Init(data.card_name.ToString(), hope_effect, data.hope_amount, data.hope_bonus_amount, data.hope_cost, despair_effect, data.despair_amount, data.despair_bonus_amount, data.despair_cost, data.restrictions.ToString(), data.restrictions_amount);
                    _deck_card.Add(card);

                    break;

            }
            index++;
        }
    }

    //デバック用カード補充
    void DebugAddNewCard()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject card_obj = new GameObject(i + "test");
            card_obj.transform.SetParent(this.transform);
            Card card = card_obj.AddComponent<Card>();
            int amount = 3;//Random.Range(1, 4);
            int effect_choice = Random.Range(0, 5);
            string effect = "Attack";//card.GetEffectNumber(effect_choice);
            card.Init("normal", CardType.Normal, effect, amount, amount / 3, "", 0);
            _deck_card.Add(card);
        }
    }
}

