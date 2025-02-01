using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class CardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text cardTypeText;
    [SerializeField] private TMP_Text cardEffectText;
    [SerializeField] private TMP_Text cardCostText;

    public void SetCardData(CardData card)
    {
        cardTypeText.text = "";//"Type: " + card.type;
        cardEffectText.text = ""; ;//Effect: " + (string.IsNullOrEmpty(card.normal_effect) ? card.hope_effect + " / " + card.despair_effect : card.normal_effect);
        cardCostText.text = "";//"Cost: " + card.normal_cost;

        string file_name = card.card_name + ".png";
        string image_path = Path.Combine(Application.persistentDataPath, "Image", file_name);
        cardNameText.text = "loading_now";

        if (File.Exists(image_path))
        {
            PersonalDataController personal_data = new PersonalDataController();

            Debug.Log(file_name + ":" + personal_data.Load().RESOLUTION);
            byte[] image_data = File.ReadAllBytes(image_path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(image_data);
            ApplyTextureWithPixelsPerUnit(texture, personal_data.Load().RESOLUTION, this.gameObject);

        }
        else
        {
            cardNameText.text = "ÉfÅ[É^Ç™å©Ç¬Ç©ÇËÇ‹ÇπÇÒ";
            Debug.Log(file_name + "failed");
        }

        cardNameText.text = "";
    }

    private void ApplyTextureWithPixelsPerUnit(Texture2D texture, float pixels_per_unit, GameObject obj)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixels_per_unit);

        obj.GetComponent<Image>().sprite = sprite;
    }
}
