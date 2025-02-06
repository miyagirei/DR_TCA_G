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

    [SerializeField]private CardLoader cardLoader;
    private GridLayoutGroup cardGridLayout;
    private GridLayoutGroup deckGridLayout;
    private string[] deckNames = { "Deck1", "Deck2", "Deck3", "Deck4", "Deck5", "Deck6" };
    private int currentDeckIndex;

    // デッキ選択画面と編集画面のUIを格納するパネル
    [SerializeField] private GameObject deckSelectPanel;
    [SerializeField] private GameObject deckEditorPanel;

    List<CardData> _prefab_deck_obj = new List<CardData>();
    Dictionary<CardData, GameObject> _deck_data = new Dictionary<CardData, GameObject>();

    List<CardData> _prefab_card_obj = new List<CardData>();
    Dictionary<CardData, GameObject> _card_data = new Dictionary<CardData, GameObject>();

    int _loading_count = 0;
    int _deck_count = 0;

    void Start()
    {
        cardLoader = GetComponent<CardLoader>(); // CardLoader スクリプトを取得
        cardGridLayout = cardListParent.GetComponent<GridLayoutGroup>();
        deckGridLayout = deckListParent.GetComponent<GridLayoutGroup>();

        cardList = cardLoader.GetNetworkCardData("card_data");
        LoadDeck(); // 初期デッキを読み込む
        DisplayCards(); // カードリストを表示
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
                DisplayDeck();
            }
            else
            {
                Debug.LogError("デッキデータの読み込みに失敗しました");
                deckList.Clear(); // デッキリストを空にする
                DisplayDeck();
            }
        }
        else
        {
            Debug.Log("保存されたデッキが見つかりません: " + filePath);
            deckList.Clear(); // デッキリストを空にする
            //UpdateDeckUI(); // 空のデッキUIを更新
            DisplayDeck();
        }
    }

    private void Update()
    {
        SetImageData();
        SetDeckImage();
    }

    void SetImageData()
    {
        if (_loading_count >= _card_data.Count)
        {
            return;
        }

        CardUI card_ui = _card_data[_prefab_card_obj[_loading_count]].GetComponent<CardUI>();
        card_ui.SetCardData(_prefab_card_obj[_loading_count]);
        _loading_count++;
    }    
    void DisplayCards()
    {
        cardLoader.Get("card_data");
        List<CardData> cardList = cardLoader.GetCardList(); // CardLoader からカードリストを取得

        if (cardList != null)
        {
            foreach (var card in cardList)
            {
                GameObject cardUI = Instantiate(cardUIPrefab, cardListParent);
                _prefab_card_obj.Add(card);
                _card_data.Add(card, cardUI);

                Button addButton = cardUI.GetComponentInChildren<Button>();
                addButton.onClick.AddListener(() => AddCardToDeck(card));
            }
            AdjustContentHeight();
        }
        else
        {
            Debug.LogError("カードリストが取得できません");
        }
    }
    
    void SetDeckImage()
    {
        if (_deck_count >= _deck_data.Count)
        {
            return;
        }

        Debug.Log(_deck_count + "count : " + _deck_data.Count );
        CardUI card_ui = _deck_data[_prefab_deck_obj[_deck_count]].GetComponent<CardUI>();
        card_ui.SetCardData(_prefab_deck_obj[_deck_count]);
        _deck_count++;
    }
    void DisplayDeck()
    {
        _deck_data.Clear();
        _prefab_deck_obj.Clear();
        foreach (Transform child in deckListParent)
        {
            Destroy(child.gameObject);
        }

        _deck_count = 0;
        List<CardData> cardList = deckList; 

        if (cardList != null)
        {
            foreach (var card in cardList)
            {
                GameObject cardUI = Instantiate(cardUIPrefab, deckListParent);
                _deck_data.Add(card, cardUI);
                _prefab_deck_obj.Add(card);
                // デッキからカードを削除するボタンを追加
                Button removeButton = cardUI.GetComponentInChildren<Button>();
                removeButton.onClick.AddListener(() => RemoveCardFromDeck(card));

            }
            AdjustContentHeight();
        }
        else
        {
            Debug.LogError("カードリストが取得できません");
        }
    }

    void AddCardToDeck(CardData card)
    {
            // デッキに同名のカードが4枚未満であれば追加
            int cardCount = deckList.FindAll(c => c.card_name == card.card_name).Count;
            if (cardCount < 4) // 同名カードが4枚未満であれば追加
            {
                deckList.Add(card);
                CreateCardData(card);
            }
            else
            {
                SoundManager.PlaySoundStatic(SoundType.AlertsSound);
                Debug.Log("このカードは4枚までしか追加できません");
            }
        AdjustDeckContentHeight();
    }

    void RemoveCardFromDeck(CardData card)
    {
        SoundManager.PlaySoundStatic(SoundType.TrashSound);
        deckList.Remove(card);
        Destroy(_deck_data[card]);
        AdjustDeckContentHeight();

    }

    void CreateCardData(CardData card) {
        SoundManager.PlaySoundStatic(SoundType.DrawSound);
        GameObject cardUI = Instantiate(cardUIPrefab, deckListParent);
        _deck_data.Add(card, cardUI);
        _prefab_deck_obj.Add(card);
        CardUI card_ui = cardUI.GetComponent<CardUI>();
        card_ui.SetCardData(card);
        // デッキからカードを削除するボタンを追加
        Button removeButton = cardUI.GetComponentInChildren<Button>();
        removeButton.onClick.AddListener(() => RemoveCardFromDeck(card));
        _deck_count++;
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
        SoundManager.PlaySoundStatic(SoundType.HopeSound);

        // 現在選択されているデッキのデータをJSON形式に変換こ
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
        SoundManager.PlaySoundStatic(SoundType.HopeSound);

        PlayerPrefs.SetInt("SelectedDeck", (currentDeckIndex + 1));
        PlayerPrefs.Save();
        Debug.Log($"デッキ "+(currentDeckIndex + 1)+" が選択されました");
    }

    // デッキ選択ボタンの処理
    public void OnDeckButtonClicked(int deckIndex)
    {
        SoundManager.PlaySoundStatic(SoundType.DecisionSound);
        currentDeckIndex = deckIndex;
        LoadDeck(); // 選択したデッキを読み込む
        deckSelectPanel.SetActive(false); // デッキ選択画面を非表示
        deckEditorPanel.SetActive(true); // デッキ編集画面を表示
    }

    // 戻るボタン
    public void OnBackButtonClicked()
    {
        SoundManager.PlaySoundStatic(SoundType.ReturnSound);
        deckSelectPanel.SetActive(true); // デッキ選択画面を表示
        deckEditorPanel.SetActive(false); // デッキ編集画面を非表示
    }
}

[System.Serializable]
public class DeckData
{
    public List<CardData> cards;
}

