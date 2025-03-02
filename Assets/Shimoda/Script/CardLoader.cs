using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Fusion;
using System;

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
            List<CardData> deck_data = JsonUtility.FromJson<List<CardData>>(json);
            DeckData deckData = JsonUtility.FromJson<DeckData>(json);

            //if (deckData != null && deckData.cards != null)
            //{
            if (deck_data != null)
            {
                index = deckData.cards; // デッキリストに復元
                //index = deck_data;
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

    public void Get(string json_file_name)
    {
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

    public List<NetworkCardData> ConvertCardList(List<CardData> data)
    {
        List<NetworkCardData> list = new List<NetworkCardData>();
        foreach (CardData card in data)
        {
            NetworkCardData network = new NetworkCardData();
            network = network.ConvertCardData(card);
            list.Add(network);
        }

        return list;
    }    
    
    public List<CardData> ConvertNetworkList(List<NetworkCardData> data)
    {
        List<CardData> list = new List<CardData>();
        foreach (NetworkCardData card in data)
        {
            CardData local = new CardData();
            local = local.ConvertNetworkCardData(card);
            list.Add(local);
        }

        return list;
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
    public string restrictions;
    public int restrictions_amount;
    public int card_id;

    public CardData ConvertNetworkCardData(NetworkCardData data)
    {
        CardData card_data = new CardData();
        CardLoader card_loader = new CardLoader();
        card_loader.Get("card_data");
        List<CardData> card_list = card_loader.GetCardList();

        if (GetSameId(card_list, data.card_id) != null)
        {
            CardData same_data = GetSameId(card_list, data.card_id);
            card_data.card_name = same_data.card_name;
            card_data.type = same_data.type.ToString();
            card_data.normal_effect = same_data.normal_effect.ToString();
            card_data.normal_amount = same_data.normal_amount;
            card_data.normal_cost = same_data.normal_cost;
            card_data.hope_effect = same_data.hope_effect.ToString();
            card_data.hope_amount = same_data.hope_amount;
            card_data.hope_cost = same_data.hope_cost;
            card_data.hope_bonus_amount = same_data.hope_bonus_amount;
            card_data.despair_effect = same_data.despair_effect.ToString();
            card_data.despair_amount = same_data.despair_amount;
            card_data.despair_cost = same_data.despair_cost;
            card_data.despair_bonus_amount = same_data.despair_bonus_amount;
            card_data.image = same_data.image.ToString();
            card_data.restrictions = same_data.restrictions.ToString();
            card_data.restrictions_amount = same_data.restrictions_amount;
        }
        else
        {
            card_data.card_name = "not_found";
        }

        //card_data.type = data.type.ToString();
        //card_data.normal_effect = data.normal_effect.ToString();
        //card_data.normal_amount = data.normal_amount;
        //card_data.normal_cost = data.normal_cost;
        //card_data.hope_effect = data.hope_effect.ToString();
        //card_data.hope_amount = data.hope_amount;
        //card_data.hope_cost = data.hope_cost;
        //card_data.hope_bonus_amount = data.hope_bonus_amount;
        //card_data.despair_effect = data.despair_effect.ToString();
        //card_data.despair_amount = data.despair_amount;
        //card_data.despair_cost = data.despair_cost;
        //card_data.despair_bonus_amount = data.despair_bonus_amount;
        //card_data.image = data.image.ToString();
        //card_data.restrictions = data.restrictions.ToString();
        //card_data.restrictions_amount = data.restrictions_amount;
        card_data.card_id = data.card_id;

        return card_data;
    }

    CardData GetSameId(List<CardData> card_list, int search_id)
    {
        foreach (CardData card in card_list)
        {
            if (card.card_id == search_id)
            {
                return card;
            }
        }
        return null;
    }
}

[System.Serializable]
public class CardListWrapper
{
    public List<CardData> cards;
}

//card_name;
//type;
//normal_effect;
//normal_amount;
//normal_cost;
//hope_effect;
//hope_amount;
//hope_cost;
//hope_bonus_amount;
//despair_effect;
//despair_amount;
//despair_cost;
//despair_bonus_amount;
//image;
//restrictions;
//restrictions_amount;

public struct NetworkCardData : INetworkStruct
{
    //public NetworkString<_32> card_name;
    //public NetworkString<_32> type;
    //public NetworkString<_32> normal_effect;
    //public int normal_amount;
    //public int normal_cost;
    //public NetworkString<_32> hope_effect;
    //public int hope_amount;
    //public int hope_cost;
    //public int hope_bonus_amount;
    //public NetworkString<_32> despair_effect;
    //public int despair_amount;
    //public int despair_cost;
    //public int despair_bonus_amount;
    //public NetworkString<_32> image;
    //public NetworkString<_32> restrictions;
    //public int restrictions_amount;
    public int card_id;

    public NetworkCardData ConvertCardData(CardData data)
    {
        NetworkCardData network_data = new NetworkCardData();
        //network_data.card_name = data.card_name;
        //network_data.type = data.type;
        //network_data.normal_effect = data.normal_effect;
        //network_data.normal_amount = data.normal_amount;
        //network_data.normal_cost = data.normal_cost;
        //network_data.hope_effect = data.hope_effect;
        //network_data.hope_amount = data.hope_amount;
        //network_data.hope_cost = data.hope_cost;
        //network_data.hope_bonus_amount = data.hope_bonus_amount;
        //network_data.despair_effect = data.despair_effect;
        //network_data.despair_amount = data.despair_amount;
        //network_data.despair_cost = data.despair_cost;
        //network_data.despair_bonus_amount = data.despair_bonus_amount;
        //network_data.image = data.image;
        //network_data.restrictions = data.restrictions;
        //network_data.restrictions_amount = data.restrictions_amount;
        network_data.card_id = data.card_id;

        return network_data;
    }
}

//public class NetworkCardSync : NetworkBehaviour
//{
//    [Networked] private NetworkString<_32> card_name { get; set; }
//    [Networked] private NetworkString<_32> type { get; set; }
//    [Networked] private NetworkString<_32> normal_effect { get; set; }
//    [Networked] private int normal_amount { get; set; }
//    [Networked] private int normal_cost { get; set; }
//    [Networked] private string hope_effect { get; set; }
//    [Networked] private int hope_amount { get; set; }
//    [Networked] private int hope_cost { get; set; }
//    [Networked] private int hope_bonus_amount { get; set; }
//    [Networked] private string despair_effect { get; set; }
//    [Networked] private int despair_amount { get; set; }
//    [Networked] private int despair_cost { get; set; }
//    [Networked] private int despair_bonus_amount { get; set; }
//    [Networked] private string image { get; set; }
//    [Networked] private string restrictions { get; set; }
//    [Networked] private int restrictions_amount { get; set; }

//    private CardData _cardData;

//    public void SetCardData(CardData data)
//    {
//        _cardData = data;

//        if (HasStateAuthority) // ネットワーク上のホストのみが変更できる
//        {
//            card_name = data.card_name;
//            type = data.type;
//            normal_effect = data.normal_effect;
//            normal_amount = data.normal_amount;
//            normal_cost = data.normal_cost;
//            hope_effect = data.hope_effect;
//            hope_amount = data.hope_amount;
//            hope_cost = data.hope_cost;
//            hope_bonus_amount = data.hope_bonus_amount;
//            despair_effect = data.despair_effect;
//            despair_amount = data.despair_amount;
//            despair_cost = data.despair_cost;
//            despair_bonus_amount = data.despair_bonus_amount;
//            image = data.image;
//            restrictions = data.restrictions;
//            restrictions_amount = data.restrictions_amount;
//        }
//    }

//    public CardData GetCardData()
//    {
//        if (_cardData == null)
//        {
//            _cardData = new CardData
//            {
//                card_name = card_name.ToString(),
//                type = type.ToString(),
//                normal_effect = normal_effect.ToString(),
//                normal_amount = normal_amount,
//                normal_cost = normal_cost,
//                hope_effect = hope_effect,
//                hope_amount = hope_amount,
//                hope_cost = hope_cost,
//                hope_bonus_amount = hope_bonus_amount,
//                despair_effect = despair_effect,
//                despair_amount = despair_amount,
//                despair_cost = despair_cost,
//                despair_bonus_amount = despair_bonus_amount,
//                image = image,
//                restrictions = restrictions,
//                restrictions_amount = restrictions_amount
//            };
//        }
//        return _cardData;
//    }
//}