using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DeckEditor : MonoBehaviour
{
    [SerializeField] private GameObject cardUIPrefab; // カードUIプレハブ
    [SerializeField] private Transform cardListParent; // カードリストを配置する親オブジェクト
    [SerializeField] private Transform deckListParent; // デッキ内のカードを配置する親オブジェクト
    [SerializeField] private RectTransform cardListContentRectTransform; // カードリストのContent
    [SerializeField] private RectTransform deckListContentRectTransform; // デッキリストのContent

    [SerializeField] private int cardPerRow = 5; // 一列に配置されるカードの枚数
    private List<CardData> cardList = new List<CardData>();
    private List<CardData> deckList = new List<CardData>(); // デッキに追加されたカード

    private CardLoader cardLoader;
    private GridLayoutGroup cardGridLayout;
    private GridLayoutGroup deckGridLayout;

    void Start()
    {
        cardLoader = GetComponent<CardLoader>(); // CardLoader スクリプトを取得
        cardGridLayout = cardListParent.GetComponent<GridLayoutGroup>();
        deckGridLayout = deckListParent.GetComponent<GridLayoutGroup>();

        LoadCardList("cardList"); // 任意のカードリストを読み込む
        LoadDeck();
        DisplayCards();
    }

    void LoadCardList(string jsonFileName)
    {
        // JSONファイルからカードリストを読み込む
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);
        if (jsonFile != null)
        {
            CardListWrapper cardListWrapper = JsonUtility.FromJson<CardListWrapper>(jsonFile.text);
            if (cardListWrapper != null && cardListWrapper.cards != null)
            {
                cardList = cardListWrapper.cards;
                Debug.Log("カードリストの読み込み成功");
            }
            else
            {
                Debug.LogError("カードリストの読み込みに失敗しました");
            }
        }
        else
        {
            Debug.LogError("カードリストのJSONファイルが見つかりません");
        }
    }

    void LoadDeck()
    {
        // ファイルパスを設定
        string filePath = Application.persistentDataPath + "/deck.json";

        if (File.Exists(filePath))
        {
            // ファイルからJSONを読み込み
            string json = File.ReadAllText(filePath);

            // JSONをデッキデータに変換
            DeckData deckData = JsonUtility.FromJson<DeckData>(json);

            if (deckData != null && deckData.cards != null)
            {
                deckList = deckData.cards; // デッキリストに復元
                Debug.Log("デッキが読み込まれました: " + filePath);

                UpdateDeckUI();
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
    }

    void DisplayCards()
    {
        foreach (var card in cardList)
        {
            GameObject cardUI = Instantiate(cardUIPrefab, cardListParent);
            CardUI cardUIScript = cardUI.GetComponent<CardUI>();
            cardUIScript.SetCardData(card);

            // カードにデッキに追加するボタンを追加
            Button addButton = cardUI.GetComponentInChildren<Button>();
            addButton.onClick.AddListener(() => AddCardToDeck(card));
        }
        AdjustContentHeight();
    }

    void AddCardToDeck(CardData card)
    {
        // デッキに同名のカードが4枚未満であれば追加
        int cardCount = deckList.FindAll(c => c.cardName == card.cardName).Count;
        if (cardCount < 4) // 同名カードが4枚未満であれば追加
        {
            deckList.Add(card);
            UpdateDeckUI();
        }
        else
        {
            Debug.Log("このカードは4枚までしか追加できません");
        }
        AdjustDeckContentHeight();
    }

    void RemoveCardFromDeck(CardData card)
    {
        deckList.Remove(card);
        UpdateDeckUI();
    }

    void UpdateDeckUI()
    {
        // デッキ内のカードを更新
        foreach (Transform child in deckListParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var card in deckList)
        {
            GameObject cardUI = Instantiate(cardUIPrefab, deckListParent);
            CardUI cardUIScript = cardUI.GetComponent<CardUI>();
            cardUIScript.SetCardData(card);

            // デッキからカードを削除するボタンを追加
            Button removeButton = cardUI.GetComponentInChildren<Button>();
            removeButton.onClick.AddListener(() => RemoveCardFromDeck(card));
        }
        AdjustDeckContentHeight();
    }

    void AdjustContentHeight()
    {
        // カードリストのContent高さを調整
        int cardCount = cardListParent.childCount;
        float contentHeight = Mathf.CeilToInt((float)cardCount / cardPerRow) * 350f; // 高さを計算

        // Contentの高さを設定
        cardListContentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentHeight);
    }

    void AdjustDeckContentHeight()
    {
        // デッキリストのContent高さを調整
        int deckCount = deckListParent.childCount;
        float contentHeight = Mathf.CeilToInt((float)deckCount / cardPerRow) * 350f; // 高さを計算

        // Contentの高さを設定
        deckListContentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentHeight);
    }

    // デッキの保存処理
    public void SaveDeck()
    {
        // デッキのデータをJSON形式に変換
        DeckData deckData = new DeckData();
        deckData.cards = deckList;

        string json = JsonUtility.ToJson(deckData, true);

        // ファイルパスを設定
        string filePath = Application.persistentDataPath + "/deck.json";

        // JSONファイルを保存
        File.WriteAllText(filePath, json);
        Debug.Log("デッキが保存されました: " + filePath);
    }
}

[System.Serializable]
public class DeckData
{
    public List<CardData> cards;
}
