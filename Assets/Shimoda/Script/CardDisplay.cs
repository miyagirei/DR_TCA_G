using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private GameObject cardUIPrefab; // カードUIプレハブ
    [SerializeField] private Transform cardListParent; // カードリストを配置する親オブジェクト
    [SerializeField] private RectTransform contentRectTransform; // スクロールビューのContentのRectTransform
    [SerializeField] private float cardHeight = 100f; // 1枚あたりのカードの高さ（適宜調整）
    [SerializeField] private int cardPerRow = 5; //　一列に配置されるカードの枚数

    Dictionary<CardData,GameObject> _card_data = new Dictionary<CardData, GameObject>();
    List<CardData> _card_obj = new List<CardData>();
    int _loading_count = 0;

    private CardLoader cardLoader;

    void Start()
    {
        _loading_count = 0;
        cardLoader = GetComponent<CardLoader>(); // CardLoader スクリプトを取得
        if (cardLoader != null)
        {
            // 任意のシーンや設定に応じて、異なるJSONファイルを指定
            cardLoader.Get("card_data");
            DisplayCards();
        }
        else
        {
            Debug.LogError("CardLoader スクリプトが見つかりません");
        }
    }

    private void Update()
    {
        SetImageData();
    }

    void SetImageData() {
        if (_loading_count >= _card_data.Count) {
            return;
        }

        CardUI card_ui = _card_data[_card_obj[_loading_count]].GetComponent<CardUI>();
        card_ui.SetCardData(_card_obj[_loading_count]);
        _loading_count++;
    }

    void DisplayCards()
    {
        List<CardData> cardList = cardLoader.GetCardList(); // CardLoader からカードリストを取得

        if (cardList != null)
        {
            foreach (var card in cardList)
            {
                GameObject cardUI = Instantiate(cardUIPrefab, cardListParent);
                _card_obj.Add(card);
                _card_data.Add(card , cardUI);
            }
            AdjustContentHeight();
        }
        else
        {
            Debug.LogError("カードリストが取得できません");
        }
    }

    void AdjustContentHeight()
    {
        // 配置されたカードの数に基づいてContentの高さを設定
        int cardCount = cardListParent.childCount;
        float contentHeight = cardCount /cardPerRow * cardHeight;

        // Contentの高さを設定
        contentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentHeight);
    }
}
