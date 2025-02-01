using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
    public List<CardData> LoadCardDeck(string jsonFileName)
    {
        string filePath = Application.persistentDataPath + "/" + jsonFileName + ".json";
        List<CardData> index = null;

        if (File.Exists(filePath))
        {
            // ファイルからJSONを読み込み
            string json = File.ReadAllText(filePath);

            // JSONをデッキデータに変換
            DeckData deckData = JsonUtility.FromJson<DeckData>(json);

            if (deckData != null && deckData.cards != null)
            {
                index = deckData.cards; // デッキリストに復元
                Debug.Log("デッキが読み込まれました: " + filePath);
            }
            else
            {
                Debug.LogError("デッキデータの読み込みに失敗しました");
            }
        }
        else
        {
            Debug.Log("保存されたデッキが見つかりません: " + filePath);
        }
        return index;
    }

    public void Get(string json_file_name) {
        cardList = GetNetworkCardData(json_file_name);
    }

    public List<CardData> GetNetworkCardData(string json_file_name)
    {
        string file_path = Path.Combine(Application.persistentDataPath, json_file_name + ".json");

        if (File.Exists(file_path))
        {
            string json = File.ReadAllText(file_path);

            CardListWrapper card_list_wrapper = JsonUtility.FromJson<CardListWrapper>(json);
            //Debug.Log(card_list_wrapper.cards);
            //Debug.Log(file_path);
            return card_list_wrapper.cards;
        }
        else
        {
            Debug.LogError("ファイルが存在しません");
            return null;
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
    public string card_name;
    public string type;
    public string normal_effect;
    public int normal_amount;
    public int normal_cost;
    public string hope_effect;
    public int hope_amount;
    public int hope_cost;
    public int hope_bonus_amount;
    public string despair_effect;
    public int despair_amount;
    public int despair_cost;
    public int despair_bonus_amount;
    public string image;
}

[System.Serializable]
public class CardListWrapper
{
    public List<CardData> cards;
}
