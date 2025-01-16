using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text cardTypeText;
    [SerializeField] private TMP_Text cardEffectText;
    [SerializeField] private TMP_Text cardCostText;

    public void SetCardData(CardData card)
    {
        cardNameText.text = card.cardName;
        cardTypeText.text = "Type: " + card.type;
        cardEffectText.text = "Effect: " + (string.IsNullOrEmpty(card.effect) ? card.effectHope + " / " + card.effectDespair : card.effect);
        cardCostText.text = "Cost: " + card.cost;
    }
}
