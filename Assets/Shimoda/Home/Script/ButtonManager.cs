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
    public Button buttonBattleToBattle;
    public GameObject buttonDeckAndList;
    public GameObject buttonStoryAndBattle;
    public GameObject buttonPlayerAndSetting;

    [SerializeField] Button _button_exit;
    [SerializeField] GameObject _check_the_end_panel;
    [SerializeField] Button _button_yes;
    [SerializeField] Button _button_no;

    void Start()
    {
        buttonCard.onClick.AddListener(OnCardButtonClicked);
        buttonHome.onClick.AddListener(OnHomeButtonClicked);
        buttonBattle.onClick.AddListener(OnBattleButtonClicked);
        buttonOption.onClick.AddListener(OnOptionButtonClicked);
        buttonCardList.onClick.AddListener(OnCardListButtonClicked);
        buttonDeck.onClick.AddListener(OnCardDeckButtonClicked);
        buttonBattleToBattle.onClick.AddListener(OnBattleToBattleButtonClicked);

        _button_exit.onClick.AddListener(() =>OnCheckTheEndButton(true));
        _button_yes.onClick.AddListener(OnExitButtonClicked);
        _button_no.onClick.AddListener(() =>OnCheckTheEndButton(false));

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
    void OnBattleToBattleButtonClicked()
    {
        SceneManager.LoadScene("ChooseDeckScene");
    }

    void OnExitButtonClicked()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();//ゲームプレイ終了
        #endif
    }

    void OnCheckTheEndButton(bool view) {
        _check_the_end_panel.gameObject.SetActive(view);
    }
}
