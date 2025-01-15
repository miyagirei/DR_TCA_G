using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardListUI : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform contentArea;
    private CardManager cardManager;

    void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
        DisplayCardList();
    }

    void DisplayCardList()
    {
        List<CardData> cards = cardManager.GetAllCards();

        foreach (var card in cards)
        {
            GameObject cardObj = Instantiate(cardPrefab, contentArea);
            cardObj.GetComponentInChildren<Text>().text = $"{card.cardName}\nDamage: {card.damageDealt}";
        }
    }
}
