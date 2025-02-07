using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class HomeSceneManager : MonoBehaviour
{
    const float CANVAS_MOVE_SPEED = 500f;
    const float INVISIBLE_MOVE_SPEED = 1800f;
    const float SPEED_SUPPRESSION_VALUE = 300f;
    const float PANEL_MOVE_SPEED = 3000f;
    const float ROTATE_PANEL_SPEED = 180f;

    [Header("MainButton")]

    [SerializeField] Vector3 _back_button_invisible_pos = new Vector3(-1160, -640, 0);
    [SerializeField] Vector3 _back_button_visible_pos = new Vector3(-760, -440, 0);
    public Button _button_back;

    [SerializeField] Vector3 _main_button_invisible_pos = new Vector3(0, -1000, 0);
    Vector3 _main_panel_invisible_pos = new Vector3(0, 1000, 0);

    public Button _button_card;
    [SerializeField] Vector3 _card_button_visible_pos = new Vector3(0, -240, 0);
    public GameObject _panel_deck_or_list;

    public Button _button_battle;
    [SerializeField] Vector3 _battle_button_visible_pos = new Vector3(-450, -290, 0);
    public GameObject _panel_story_or_battle;

    public Button _button_option;
    [SerializeField] Vector3 _option_button_visible_pos = new Vector3(450, -290, 0);
    public GameObject _panel_player_or_setting;

    [SerializeField] bool _move_card_panel = false;
    [SerializeField] bool _move_main_panel = false;
    [SerializeField] bool _move_battle_panel = false;
    [SerializeField] bool _move_option_panel = false;

    [Header("CardInside")]
    public Button buttonDeck;
    public Button buttonCardList;
    public Button buttonBattleToBattle;

    [SerializeField] Button _button_exit;
    [SerializeField] GameObject _check_the_end_panel;
    [SerializeField] Button _button_yes;
    [SerializeField] Button _button_no;

    [SerializeField, Header("Setting")] GameObject _setting_panel;
    [SerializeField] Button _button_open_setting_panel;
    [SerializeField] Button _button_close_setting_panel;
    [SerializeField] Dropdown _dropdown_button;
    [SerializeField] PersonalDataController _personal_data_controller;
    [SerializeField] Vector3 _invisible_setting_pos = new Vector3(2000, 0, 0);
    PersonalData _personal_data = new PersonalData();
    [SerializeField] Button _button_open_audio_panel;
    [SerializeField] Button _button_close_audio_panel;
    [SerializeField] AudioMixer _mixer;
    [SerializeField] Slider _master_volume;
    [SerializeField] Slider _bgm_volume;
    [SerializeField] Slider _se_volume;

    [SerializeField] float _rotate_x_setting = 0;
    [SerializeField] float _waiting_rotate_x_setting = 90f;
    [SerializeField] bool _move_setting_panel = false;
    [SerializeField] bool _move_audio_panel = false;

    [SerializeField] Image _character_sprite;

    [SerializeField] Button _player_setting_button;
    [SerializeField] Button _player_setting_button_back;
    [SerializeField] GameObject _player_setting_panel;
    [SerializeField] bool _move_player_setting_panel;
    [SerializeField] Dropdown _player_setting_dropdown;

    void Start()
    {
        SetSliderValueToMixer("GeneralMaster", _master_volume);
        SetSliderValueToMixer("GeneralBGM", _bgm_volume);
        SetSliderValueToMixer("GeneralSE", _se_volume);
        SetImageCharacter();

        _master_volume.value = _personal_data_controller.Load().AUDIO_MASTER;
        _bgm_volume.value = _personal_data_controller.Load().AUDIO_BGM;
        _se_volume.value = _personal_data_controller.Load().AUDIO_SE;

        _button_card.onClick.AddListener(OnCardButtonClicked);
        _button_back.onClick.AddListener(OnHomeButtonClicked);
        _button_battle.onClick.AddListener(OnBattleButtonClicked);
        _button_option.onClick.AddListener(OnOptionButtonClicked);
        buttonCardList.onClick.AddListener(OnCardListButtonClicked);
        buttonDeck.onClick.AddListener(OnCardDeckButtonClicked);
        buttonBattleToBattle.onClick.AddListener(OnBattleToBattleButtonClicked);

        _button_exit.onClick.AddListener(() => OnPanelButton(_check_the_end_panel, true));
        _button_yes.onClick.AddListener(OnExitButtonClicked);
        _button_no.onClick.AddListener(() => OnPanelButton(_check_the_end_panel, false));

        _button_open_setting_panel.onClick.AddListener(() => OnMovePanel(ref _move_setting_panel , true));//セッティング要素を考え直してください
        _button_close_setting_panel.onClick.AddListener(() => OnMovePanel(ref _move_setting_panel ,false));
        _dropdown_button.onValueChanged.AddListener((value) => ChangeSolution(value));

        _button_open_audio_panel.onClick.AddListener(() => OnMovePanel(ref _move_audio_panel, true));
        _button_close_audio_panel.onClick.AddListener(() => OnMovePanel(ref _move_audio_panel, false));

        _player_setting_button.onClick.AddListener(() => OnMovePanel(ref _move_player_setting_panel , true));
        _player_setting_button_back.onClick.AddListener(() => OnMovePanel(ref _move_player_setting_panel, false));
        _player_setting_dropdown.onValueChanged.AddListener((value) => ChangePlayerCharacter(value));
    }

    private void Update()
    {
        if (_move_main_panel)
        {
            MoveMainPanel();
        }

        if (_move_card_panel)
        {
            MoveMainPanel(_button_card, _button_battle, _button_option, _move_card_panel, _panel_deck_or_list, _panel_story_or_battle, _panel_player_or_setting);
        }

        if (_move_battle_panel)
        {
            MoveMainPanel(_button_battle, _button_card, _button_option, _move_battle_panel, _panel_story_or_battle, _panel_deck_or_list, _panel_player_or_setting);
        }

        if (_move_option_panel)
        {
            MoveMainPanel(_button_option, _button_battle, _button_card, _move_option_panel, _panel_player_or_setting, _panel_story_or_battle, _panel_deck_or_list);
        }

        MoveSettingPanel();
        MoveAudioPanel();
        MovePlayerSettingPanel();
    }

    void MoveMainPanel()
    {
        _button_back.gameObject.transform.localPosition = Vector3.MoveTowards(_button_back.gameObject.transform.localPosition, _back_button_invisible_pos, INVISIBLE_MOVE_SPEED * Time.deltaTime);

        _button_card.gameObject.transform.localPosition = Vector3.MoveTowards(_button_card.gameObject.transform.localPosition, _card_button_visible_pos, INVISIBLE_MOVE_SPEED * Time.deltaTime);
        _button_battle.gameObject.transform.localPosition = Vector3.MoveTowards(_button_battle.gameObject.transform.localPosition, _battle_button_visible_pos, INVISIBLE_MOVE_SPEED * Time.deltaTime);
        _button_option.gameObject.transform.localPosition = Vector3.MoveTowards(_button_option.gameObject.transform.localPosition, _option_button_visible_pos, INVISIBLE_MOVE_SPEED * Time.deltaTime);

        _panel_deck_or_list.transform.localPosition = Vector3.MoveTowards(_panel_deck_or_list.transform.localPosition, _main_panel_invisible_pos, INVISIBLE_MOVE_SPEED * Time.deltaTime);
        _panel_story_or_battle.transform.localPosition = Vector3.MoveTowards(_panel_story_or_battle.transform.localPosition, _main_panel_invisible_pos, INVISIBLE_MOVE_SPEED * Time.deltaTime);
        _panel_player_or_setting.transform.localPosition = Vector3.MoveTowards(_panel_player_or_setting.transform.localPosition, _main_panel_invisible_pos, INVISIBLE_MOVE_SPEED * Time.deltaTime);

        if (Vector3.Distance(_button_card.gameObject.transform.localPosition, _card_button_visible_pos) < 10 &&
            Vector3.Distance(_button_battle.gameObject.transform.localPosition, _battle_button_visible_pos) < 10 &&
            Vector3.Distance(_button_option.gameObject.transform.localPosition, _option_button_visible_pos) < 10)
        {
            _button_back.gameObject.transform.localPosition = _back_button_invisible_pos;

            _button_card.gameObject.transform.localPosition = _card_button_visible_pos;
            _button_battle.gameObject.transform.localPosition = _battle_button_visible_pos;
            _button_option.gameObject.transform.localPosition = _option_button_visible_pos;

            _panel_deck_or_list.transform.localPosition = _main_panel_invisible_pos;
            _panel_story_or_battle.transform.localPosition = _main_panel_invisible_pos;
            _panel_player_or_setting.transform.localPosition = _main_panel_invisible_pos;
            _move_main_panel = false;
        }
    }
    void MoveMainPanel(Button main, Button sub_0, Button sub_1, bool move_panel, GameObject panel, GameObject other_0, GameObject other_1)
    {
        _button_back.gameObject.transform.localPosition = Vector3.MoveTowards(_button_back.gameObject.transform.localPosition, _back_button_visible_pos, INVISIBLE_MOVE_SPEED * Time.deltaTime);

        Vector3 target = new Vector3(0, 0, 0);
        float distance = Vector3.Distance(main.gameObject.transform.localPosition, target);
        main.gameObject.transform.localPosition = Vector3.MoveTowards(main.gameObject.transform.localPosition, target, CANVAS_MOVE_SPEED * Time.deltaTime + (distance / SPEED_SUPPRESSION_VALUE));
        sub_0.gameObject.transform.localPosition = Vector3.MoveTowards(sub_0.gameObject.transform.localPosition, _main_button_invisible_pos, INVISIBLE_MOVE_SPEED * Time.deltaTime);
        sub_1.gameObject.transform.localPosition = Vector3.MoveTowards(sub_1.gameObject.transform.localPosition, _main_button_invisible_pos, INVISIBLE_MOVE_SPEED * Time.deltaTime);

        panel.transform.localPosition = Vector3.MoveTowards(panel.transform.localPosition, target, INVISIBLE_MOVE_SPEED * Time.deltaTime * 2);
        other_0.transform.localPosition = Vector3.MoveTowards(other_0.transform.localPosition, _main_panel_invisible_pos, INVISIBLE_MOVE_SPEED * Time.deltaTime);
        other_1.transform.localPosition = Vector3.MoveTowards(other_1.transform.localPosition, _main_panel_invisible_pos, INVISIBLE_MOVE_SPEED * Time.deltaTime);

        if (distance < 10)
        {
            _button_back.gameObject.transform.localPosition = _back_button_visible_pos;

            main.gameObject.transform.localPosition = target;
            panel.transform.localPosition = target;

            sub_0.gameObject.transform.localPosition = _main_button_invisible_pos;
            sub_1.gameObject.transform.localPosition = _main_button_invisible_pos;

            other_0.transform.localPosition = _main_panel_invisible_pos;
            other_1.transform.localPosition = _main_panel_invisible_pos;
            move_panel = false;
        }
    }

    void OnCardButtonClicked()
    {
        SoundManager.PlaySoundStatic(SoundType.DecisionSound);
        _move_main_panel = false;
        _move_card_panel = true;
        _move_battle_panel = false;
        _move_option_panel = false;
        _move_player_setting_panel = false;
    }
    void OnHomeButtonClicked()
    {
        SoundManager.PlaySoundStatic(SoundType.ReturnSound);
        _move_main_panel = true;
        _move_card_panel = false;
        _move_battle_panel = false;
        _move_option_panel = false;
        _move_setting_panel = false;
        _move_audio_panel = false;
        _move_player_setting_panel = false;
    }
    void OnBattleButtonClicked()
    {
        SoundManager.PlaySoundStatic(SoundType.DecisionSound);
        _move_main_panel = false;
        _move_card_panel = false;
        _move_battle_panel = true;
        _move_option_panel = false;
        _move_player_setting_panel = false;
    }
    void OnOptionButtonClicked()
    {
        SoundManager.PlaySoundStatic(SoundType.DecisionSound);
        _move_main_panel = false;
        _move_card_panel = false;
        _move_battle_panel = false;
        _move_option_panel = true;
        _move_player_setting_panel = false;
    }
    void OnCardListButtonClicked()
    {
        SoundManager.PlaySoundStatic(SoundType.DecisionSound);
        SceneManager.LoadScene("CardListScene");
    }
    void OnCardDeckButtonClicked()
    {
        SoundManager.PlaySoundStatic(SoundType.DecisionSound);
        SceneManager.LoadScene("CardDeckScene");
    }
    void OnBattleToBattleButtonClicked()
    {
        SoundManager.PlaySoundStatic(SoundType.DecisionSound);
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

    void OnPanelButton(GameObject panel, bool view)
    {
        if (view)
        {
            SoundManager.PlaySoundStatic(SoundType.DecisionSound);
        }
        else if (!view) {
            SoundManager.PlaySoundStatic(SoundType.ReturnSound);
        }
        panel.gameObject.SetActive(view);

    }

    void OnMovePanel(ref bool panel , bool view )
    {
        panel = view;

        if (view)
        {
            SoundManager.PlaySoundStatic(SoundType.DecisionSound);
        }
        else if (!view)
        {
            SoundManager.PlaySoundStatic(SoundType.ReturnSound);
            _personal_data_controller.Save(_personal_data);
        }
    }


    void MoveSettingPanel()
    {
        if (_move_setting_panel)
        {
            if (Vector3.Distance(_setting_panel.transform.localPosition, new Vector3(0, 0, 0)) <= 10) {
                _setting_panel.transform.localPosition = new Vector3(0, 0, 0);
                return;
            }

            _setting_panel.transform.localPosition = Vector3.MoveTowards(_setting_panel.transform.localPosition, new Vector3(0, 0, 0), PANEL_MOVE_SPEED * Time.deltaTime);
        }
        else if (!_move_setting_panel)
        {
            if (Vector3.Distance(_setting_panel.transform.localPosition, _invisible_setting_pos) <= 10)
            {
                _setting_panel.transform.localPosition = _invisible_setting_pos;
                return;
            }

            _setting_panel.transform.localPosition = Vector3.MoveTowards(_setting_panel.transform.localPosition, _invisible_setting_pos, PANEL_MOVE_SPEED * Time.deltaTime);
        }
    }

    void MoveAudioPanel()
    {
        if (_move_audio_panel)
        {
            _setting_panel.gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(_rotate_x_setting, 0, 0);

            if (_rotate_x_setting >= _waiting_rotate_x_setting)
            {
                _rotate_x_setting = _waiting_rotate_x_setting;
                return;
            }

            _rotate_x_setting += ROTATE_PANEL_SPEED * Time.deltaTime;
        }
        else if (!_move_audio_panel)
        {
            _setting_panel.gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(_rotate_x_setting, 0, 0);
            if (_rotate_x_setting <= 0f)
            {
                _rotate_x_setting = 0f;
                return;
            }

            _rotate_x_setting -= ROTATE_PANEL_SPEED * Time.deltaTime;
        }
    }

    void ChangeSolution(int value)
    {
        Debug.Log("Solution :" + value);
        switch (value)
        {
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

    private float ConvertVolume2dB(float volume) =>
    Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)), -80f, 0f);

    private void SetSliderValueToMixer(string group_name, Slider slider)
    {
        slider.onValueChanged.AddListener(value => _mixer.SetFloat(group_name, ConvertVolume2dB(value)));
    }

    void SetImageCharacter() {
        CharacterTypeInfomation character_info = new CharacterTypeInfomation();
        _character_sprite.sprite = TextureToSprite(Resources.Load<Texture2D>(character_info.GetCharacterImage(_personal_data_controller.Load().CHARACTER_TYPE)));
    }
    Sprite TextureToSprite(Texture2D texture)
    {
        return Sprite.Create(
            texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0f)
        );
    }

    void MovePlayerSettingPanel()
    {
        if (_move_player_setting_panel)
        {
            if (Vector3.Distance(_player_setting_panel.transform.localPosition, new Vector3(0, 0, 0)) <= 10)
            {
                _player_setting_panel.transform.localPosition = new Vector3(0, 0, 0);
                return;
            }

            _player_setting_panel.transform.localPosition = Vector3.MoveTowards(_player_setting_panel.transform.localPosition, new Vector3(0, 0, 0), PANEL_MOVE_SPEED * Time.deltaTime);
        }
        else if (!_move_player_setting_panel)
        {
            if (Vector3.Distance(_player_setting_panel.transform.localPosition, _invisible_setting_pos) <= 10)
            {
                _player_setting_panel.transform.localPosition = _invisible_setting_pos;
                return;
            }

            _player_setting_panel.transform.localPosition = Vector3.MoveTowards(_player_setting_panel.transform.localPosition, _invisible_setting_pos, PANEL_MOVE_SPEED * Time.deltaTime);
        }
    }

    void ChangePlayerCharacter(int value)
    {
        Debug.Log("Solution :" + value);
        switch (value)
        {
            case 0:
                _personal_data.CHARACTER_TYPE = CharacterType.Monokuma;
                break;
            case 1:
                _personal_data.CHARACTER_TYPE = CharacterType.NaegiMakoto;
                break;
            case 2:
                _personal_data.CHARACTER_TYPE = CharacterType.CelestiaLudenberg;
                break;
            case 3:
                _personal_data.CHARACTER_TYPE = CharacterType.YamadaHifumi;
                break;
            case 4:
                _personal_data.CHARACTER_TYPE = CharacterType.EnoshimaJunko;
                break;
        };

        _personal_data_controller.Save(_personal_data);
        SetImageCharacter();
    }

}
