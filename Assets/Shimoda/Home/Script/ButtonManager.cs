using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public Button buttonCard;
    public Button buttonHome;
    public Button buttonBattle;
    public Button buttonOption;
    public Button buttonDeck;
    public Button buttonCardList;
    public GameObject buttonDeckAndList;
    public GameObject buttonStoryAndBattle;
    public GameObject buttonPlayerAndSetting;

    void Start()
    {
        buttonCard.onClick.AddListener(OnCardButtonClicked);
        buttonHome.onClick.AddListener(OnHomeButtonClicked);
        buttonBattle.onClick.AddListener(OnBattleButtonClicked);
        buttonOption.onClick.AddListener(OnOptionButtonClicked);
        buttonCardList.onClick.AddListener(OnCardListButtonClicked);
        buttonDeck.onClick.AddListener(OnCardDeckButtonClicked);

        buttonDeckAndList.SetActive(false);
        buttonStoryAndBattle.SetActive(false);
        buttonPlayerAndSetting.SetActive(false);
    }

    void OnCardButtonClicked()
    {
        buttonCard.gameObject.SetActive(false);
        buttonBattle.gameObject.SetActive(true);
        buttonOption.gameObject.SetActive(true);
        buttonDeckAndList.SetActive(true);
        buttonStoryAndBattle.SetActive(false);
        buttonPlayerAndSetting.SetActive(false);
    }
    void OnHomeButtonClicked()
    {
        buttonCard.gameObject.SetActive(true);
        buttonBattle.gameObject.SetActive(true);
        buttonOption.gameObject.SetActive(true);
        buttonDeckAndList.SetActive(false);
        buttonStoryAndBattle.SetActive(false);
        buttonPlayerAndSetting.SetActive(false);
    }
    void OnBattleButtonClicked()
    {
        buttonCard.gameObject.SetActive(true);
        buttonBattle.gameObject.SetActive(false);
        buttonOption.gameObject.SetActive(true);
        buttonDeckAndList.SetActive(false);
        buttonStoryAndBattle.SetActive(true);
        buttonPlayerAndSetting.SetActive(false);
    }
    void OnOptionButtonClicked()
    {
        buttonCard.gameObject.SetActive(true);
        buttonBattle.gameObject.SetActive(true);
        buttonOption.gameObject.SetActive(false);
        buttonDeckAndList.SetActive(false);
        buttonStoryAndBattle.SetActive(false);
        buttonPlayerAndSetting.SetActive(true);
    }
    void OnCardListButtonClicked()
    {
        SceneManager.LoadScene("CardListScene");
    }
    void OnCardDeckButtonClicked()
    {
        SceneManager.LoadScene("CardDeckScene");
    }
}
