using System.Collections.Generic;
using UnityEngine;

public class CardLoader : MonoBehaviour
{
    private List<CardData> cardList;

    // 任意のJSONファイル名を指定してカードリストを読み込む
    public void LoadCardList(string jsonFileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName); // 拡張子なしで指定
        if (jsonFile != null)
        {
            Debug.Log("JSONファイル読み込み成功: " + jsonFileName);
            Debug.Log(jsonFile.text); // JSON内容を確認
            CardListWrapper cardListWrapper = JsonUtility.FromJson<CardListWrapper>(jsonFile.text);
            if (cardListWrapper != null && cardListWrapper.cards != null)
            {
                cardList = cardListWrapper.cards;
                Debug.Log("カードリストのデシリアライズ成功");
            }
            else
            {
                Debug.LogError("カードリストのデシリアライズに失敗しました");
            }
        }
        else
        {
            Debug.LogError("JSONファイルが見つかりません: " + jsonFileName);
        }
    }

    // カードリストを返す
    public List<CardData> GetCardList()
    {
        return cardList;
    }
}

[System.Serializable]
public class CardData
{
    public string cardName;
    public string type;
    public string effect;
    public int amount;
    public int cost;
    public int costHope;
    public int costDespair;
    public string effectHope;
    public string effectDespair;
    public int despairAmount;
}

[System.Serializable]
public class CardListWrapper
{
    public List<CardData> cards;
}
