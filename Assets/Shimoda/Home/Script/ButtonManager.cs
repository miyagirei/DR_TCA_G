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

    [SerializeField] GameObject _setting_panel;
    [SerializeField] Button _button_open_setting_panel;
    [SerializeField] Button _button_close_setting_panel;
    [SerializeField] Dropdown _dropdown_button;
    [SerializeField] Button _button_setting_save;
    [SerializeField] PersonalDataController _personal_data_controller;
    PersonalData _personal_data = new PersonalData();

    void Start()
    {
        buttonCard.onClick.AddListener(OnCardButtonClicked);
        buttonHome.onClick.AddListener(OnHomeButtonClicked);
        buttonBattle.onClick.AddListener(OnBattleButtonClicked);
        buttonOption.onClick.AddListener(OnOptionButtonClicked);
        buttonCardList.onClick.AddListener(OnCardListButtonClicked);
        buttonDeck.onClick.AddListener(OnCardDeckButtonClicked);
        buttonBattleToBattle.onClick.AddListener(OnBattleToBattleButtonClicked);

        _button_exit.onClick.AddListener(() => OnPanelButton(_check_the_end_panel, true));
        _button_yes.onClick.AddListener(OnExitButtonClicked);
        _button_no.onClick.AddListener(() => OnPanelButton(_check_the_end_panel, false));

        //_button_open_setting_panel.onClick.AddListener(() => OnPanelButton(_setting_panel , true));//セッティング要素を考え直してください
        _button_close_setting_panel.onClick.AddListener(() => OnPanelButton(_setting_panel , false));
        _dropdown_button.onValueChanged.AddListener((value) => ChangeSolution(value));
        _button_setting_save.onClick.AddListener(() => SaveSetting());

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

    void OnPanelButton(GameObject panel ,bool view) {
        panel.gameObject.SetActive(view);
    }

    void ChangeSolution(int value) {
        Debug.Log("Solution :" + value);
        switch (value) {
            case 0:
                _personal_data.RESOLUTION = 2160;
                break;
            case 1:
                _personal_data.RESOLUTION = 1440;
                break;
            case 2:
                _personal_data.RESOLUTION = 1080;
                break;
            case 3:
                _personal_data.RESOLUTION = 720;
                break;
            case 4:
                _personal_data.RESOLUTION = 480;
                break;
            case 5:
                _personal_data.RESOLUTION = 360;
                break;
            case 6:
                _personal_data.RESOLUTION = 240;
                break;
            case 7:
                _personal_data.RESOLUTION = 144;
                break;
        };
    }

    void SaveSetting() {
        _personal_data_controller.Save(_personal_data);
    }
}
