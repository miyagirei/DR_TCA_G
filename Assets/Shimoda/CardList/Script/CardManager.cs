using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData
{
    public string cardName;
    public int damageDealt;
}

public class CardManager : MonoBehaviour
{
    public List<CardData> cardList = new List<CardData>();

    void Start()
    {
        // 仮のカードデータを追加
        cardList.Add(new CardData { cardName = "Fireball", damageDealt = 10 });
        cardList.Add(new CardData { cardName = "Ice Lance", damageDealt = 8 });
        cardList.Add(new CardData { cardName = "Thunder Strike", damageDealt = 12 });
    }

    public List<CardData> GetAllCards()
    {
        return cardList;
    }
}
