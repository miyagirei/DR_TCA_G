using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private GameObject cardUIPrefab; // カードUIプレハブ
    [SerializeField] private Transform cardListParent; // カードリストを配置する親オブジェクト
    [SerializeField] private RectTransform contentRectTransform; // スクロールビューのContentのRectTransform
    [SerializeField] private float cardHeight = 100f; // 1枚あたりのカードの高さ（適宜調整）
    [SerializeField] private int cardPerRow = 5; //　一列に配置されるカードの枚数

    private CardLoader cardLoader;

    void Start()
    {
        cardLoader = GetComponent<CardLoader>(); // CardLoader スクリプトを取得
        if (cardLoader != null)
        {
            // 任意のシーンや設定に応じて、異なるJSONファイルを指定
            cardLoader.LoadCardList("cardList"); // cardList.json を読み込む
            DisplayCards();
        }
        else
        {
            Debug.LogError("CardLoader スクリプトが見つかりません");
        }
    }

    void DisplayCards()
    {
        List<CardData> cardList = cardLoader.GetCardList(); // CardLoader からカードリストを取得

        if (cardList != null)
        {
            foreach (var card in cardList)
            {
                GameObject cardUI = Instantiate(cardUIPrefab, cardListParent);
                CardUI cardUIScript = cardUI.GetComponent<CardUI>();
                cardUIScript.SetCardData(card);
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
