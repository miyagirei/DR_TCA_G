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
    private string[] deckNames = { "Deck1", "Deck2", "Deck3", "Deck4", "Deck5", "Deck6" };
    private int currentDeckIndex;

    // デッキ選択画面と編集画面のUIを格納するパネル
    [SerializeField] private GameObject deckSelectPanel;
    [SerializeField] private GameObject deckEditorPanel;

    void Start()
    {
        cardLoader = GetComponent<CardLoader>(); // CardLoader スクリプトを取得
        cardGridLayout = cardListParent.GetComponent<GridLayoutGroup>();
        deckGridLayout = deckListParent.GetComponent<GridLayoutGroup>();

        LoadCardList("cardList"); // 任意のカードリストを読み込む
        LoadDeck(); // 初期デッキを読み込む
        DisplayCards(); // カードリストを表示
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
        // 現在選択されているデッキを読み込む
        string filePath = Application.persistentDataPath + "/deck" + (currentDeckIndex + 1) + ".json";

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
                deckList.Clear(); // デッキリストを空にする
                UpdateDeckUI(); // 空のデッキUIを更新
            }
        }
        else
        {
            Debug.Log("保存されたデッキが見つかりません: " + filePath);
            deckList.Clear(); // デッキリストを空にする
            UpdateDeckUI(); // 空のデッキUIを更新
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
        // 現在選択されているデッキのデータをJSON形式に変換
        DeckData deckData = new DeckData();
        deckData.cards = deckList;

        string json = JsonUtility.ToJson(deckData, true);

        // ファイルパスを設定
        string filePath = Application.persistentDataPath + "/deck" + (currentDeckIndex + 1) + ".json";

        // JSONファイルを保存
        File.WriteAllText(filePath, json);
        Debug.Log("デッキが保存されました: " + filePath);
    }

    public void SetMainDeck()
    {
        PlayerPrefs.SetInt("SelectedDeck", (currentDeckIndex + 1));
        PlayerPrefs.Save();
        Debug.Log($"デッキ "+(currentDeckIndex + 1)+" が選択されました");
    }

    // デッキ選択ボタンの処理
    public void OnDeckButtonClicked(int deckIndex)
    {
        currentDeckIndex = deckIndex;
        LoadDeck(); // 選択したデッキを読み込む
        deckSelectPanel.SetActive(false); // デッキ選択画面を非表示
        deckEditorPanel.SetActive(true); // デッキ編集画面を表示
    }

    // 戻るボタン
    public void OnBackButtonClicked()
    {
        deckSelectPanel.SetActive(true); // デッキ選択画面を表示
        deckEditorPanel.SetActive(false); // デッキ編集画面を非表示
    }

    // デッキ選択画面のUIボタンを設定
    void SetDeckButtons()
    {
        for (int i = 0; i < deckNames.Length; i++)
        {
            GameObject deckButton = new GameObject("DeckButton" + i);
            Button button = deckButton.AddComponent<Button>();
            button.GetComponentInChildren<Text>().text = deckNames[i];

            int deckIndex = i;
            button.onClick.AddListener(() => OnDeckButtonClicked(deckIndex));

            deckButton.transform.SetParent(deckSelectPanel.transform);
        }
    }
}

[System.Serializable]
public class DeckData
{
    public List<CardData> cards;
}

