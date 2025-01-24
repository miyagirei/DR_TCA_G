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
        cardNameText.text = card.card_name;
        cardTypeText.text = "Type: " + card.type;
        cardEffectText.text = "Effect: " + (string.IsNullOrEmpty(card.normal_effect) ? card.hope_effect + " / " + card.despair_effect : card.normal_effect);
        cardCostText.text = "Cost: " + card.normal_cost;
    }
}
