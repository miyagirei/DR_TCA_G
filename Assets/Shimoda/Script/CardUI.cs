using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour , IPointerUpHandler , IPointerDownHandler
{
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text cardTypeText;
    [SerializeField] private TMP_Text cardEffectText;
    [SerializeField] private TMP_Text cardCostText;
    [SerializeField] GameObject _image;
    [SerializeField] Canvas _my_canvas;
    [SerializeField] float _init_image_size_x;
    [SerializeField] float _init_image_size_y;
    int _init_sorting;

    void Start() {
        _init_image_size_x = _image.transform.localScale.x;
        _init_image_size_y = _image.transform.localScale.y;
        _my_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        _init_sorting = _my_canvas.sortingOrder;
    }

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
            if(personal_data.Load().RESOLUTION != 0)
            {
                ApplyTextureWithPixelsPerUnit(texture, personal_data.Load().RESOLUTION, _image);
            }
            else {
                ApplyTextureWithPixelsPerUnit(texture, 1000, _image);
            }

        }
        else
        {
            cardNameText.text = "ÉfÅ[É^Ç™å©Ç¬Ç©ÇËÇ‹ÇπÇÒ";
            Debug.Log(file_name + "failed");
        }

        cardNameText.text = "";
    }

    private void ApplyTextureWithPixelsPerUnit(Texture2D texture, float pixels_per_unit, GameObject image)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixels_per_unit);

        image.GetComponent<Image>().sprite = sprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _image.transform.localPosition = new Vector3(0 , 0);
        _image.transform.localScale = new Vector3(_init_image_size_x , _init_image_size_y);
        _my_canvas.sortingOrder = _init_sorting;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _image.transform.position = new Vector3(Screen.width / 2 , Screen.height / 2);
        _image.transform.localScale = new Vector3(_init_image_size_x * 3 , _init_image_size_y * 3);
        _my_canvas.sortingOrder = 9999;
        this.GetComponentInParent<ScrollRect>().transform.SetAsLastSibling();
    }
}
