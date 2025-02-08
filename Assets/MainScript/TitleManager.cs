using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Audio;

public class TitleManager : MonoBehaviour
{
    const float MOVE_PANEL_SPEED = 3000f;
    const float ROTATE_PANEL_SPEED = 180f;

    [SerializeField] Button _start_button;
    [SerializeField] Button _setting_button;
    [SerializeField] GameObject _setting_panel;
    [SerializeField] Button _setting_back_button;

    [SerializeField , Header("audio")] Button _audio_button;
    [SerializeField] GameObject _audio_panel;
    [SerializeField] Button _audio_back_button;
    [SerializeField] AudioMixer _mixer;
    [SerializeField] Slider _master_volume;
    [SerializeField] Slider _bgm_volume;
    [SerializeField] Slider _se_volume;

    float _waiting_rotate = 90f;
    float _rotate_x = 0f;

    Vector3 _waitng_pos = new Vector3(2000 , 0 , 0);


    bool _move_setting_panel = false;
    bool _move_audio_panel = false;
    void ClearImageFolder()
    {
        string image_folder_path = Path.Combine(Application.persistentDataPath, "Image");

        if (Directory.Exists(image_folder_path))
        {
            string[] files = Directory.GetFiles(image_folder_path);

            foreach (string file in files)
            {
                File.Delete(file);
            }
            Debug.Log("消去が完了しました");
        }
    }


    private void Start()
    {
        PersonalDataController personal_controller = new PersonalDataController();
        SetSliderValueToMixer("GeneralMaster", _master_volume);
        SetSliderValueToMixer("GeneralBGM", _bgm_volume);
        SetSliderValueToMixer("GeneralSE", _se_volume);

        _master_volume.value =  personal_controller.Load().AUDIO_MASTER;
        _bgm_volume.value =  personal_controller.Load().AUDIO_BGM;
        _se_volume.value =  personal_controller.Load().AUDIO_SE;

        _rotate_x = 0;

        _start_button.onClick.AddListener(() => LoadNextScene());

        _setting_button.onClick.AddListener(() => SwitchSettingPanel(true));
        _setting_back_button.onClick.AddListener(() => SwitchSettingPanel(false));

        _audio_button.onClick.AddListener(() => SwitchAudioPanel(true));
        _audio_back_button.onClick.AddListener(() => SwitchAudioPanel(false));
    }
    void Update()
    {
        MoveSettingPanel();
        MoveAudioPanel();

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Debug.Log("Imageファイルをクリアしています");
            ClearImageFolder();
            return;
        }
    }

    public void LoadNextScene()
    {
        SoundManager.PlaySoundStatic(SoundType.DecisionSound);
        SceneManager.LoadScene("CardDataLoadScene");
    }

    void SwitchSettingPanel(bool display) {
        _move_setting_panel = display;

        if (display)
        {
            SoundManager.PlaySoundStatic(SoundType.DecisionSound);
        }
        else if (!display) {
            SoundManager.PlaySoundStatic(SoundType.ReturnSound);
        }
    }    
    void SwitchAudioPanel(bool display) {
        _move_audio_panel = display;

        if (display)
        {
            SoundManager.PlaySoundStatic(SoundType.DecisionSound);
        } else if (!display) {
            SoundManager.PlaySoundStatic(SoundType.ReturnSound);
            PersonalDataController controller = new PersonalDataController();
            PersonalData personal_data = new PersonalData();

            personal_data.AUDIO_MASTER = _master_volume.value;
            personal_data.AUDIO_BGM = _bgm_volume.value;
            personal_data.AUDIO_SE = _se_volume.value;
            personal_data.RESOLUTION = controller.Load().RESOLUTION;//現在帰れないのでバグらないように置いてるだけ

            controller.Save(personal_data);
        }
    }


    void MoveSettingPanel() {
        if (_move_setting_panel)
        {
            _setting_panel.gameObject.transform.localPosition = Vector3.MoveTowards(_setting_panel.gameObject.transform.localPosition, new Vector3(0, 0, 0), MOVE_PANEL_SPEED * Time.deltaTime);
        }
        else if (!_move_setting_panel)
        {
            _setting_panel.gameObject.transform.localPosition = Vector3.MoveTowards(_setting_panel.gameObject.transform.localPosition, _waitng_pos, MOVE_PANEL_SPEED * Time.deltaTime);
        }
    }

    void MoveAudioPanel() {
        if (_move_audio_panel)
        {
            _setting_panel.gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(_rotate_x , 0 , 0);
            
            if (_rotate_x >= _waiting_rotate) {
                _rotate_x = _waiting_rotate;
                return;
            }

            _rotate_x += ROTATE_PANEL_SPEED * Time.deltaTime;
        }
        else if (!_move_audio_panel)
        {
            _setting_panel.gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(_rotate_x, 0, 0);
            if (_rotate_x <= 0f)
            {
                _rotate_x = 0f;
                return;
            }

            _rotate_x -= ROTATE_PANEL_SPEED * Time.deltaTime;
        }
    }

    private float ConvertVolume2dB(float volume) =>
    Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)), -80f, 0f);

    private void SetSliderValueToMixer(string group_name, Slider slider)
    {
        slider.onValueChanged.AddListener(value => _mixer.SetFloat(group_name, ConvertVolume2dB(value)));
    }
}
