using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class ChooseDeckSceneManager : MonoBehaviour
{
    [SerializeField] private List<Button> deckButtons; // デッキに対応するボタン
    [SerializeField] private List<GameObject> highlights; // ハイライト用のオブジェクトリスト
    [SerializeField] private CharacterType _player_character_type;
    [SerializeField] private CharacterType _enemy_character_type;
    [SerializeField] Button _character_change_left_button;
    [SerializeField] Button _character_change_right_button;
    [SerializeField] SpriteRenderer _player_sprite;
    private int selectedDeckIndex = 1;
    // Start is called before the first frame update
    void Start()
    {
        PersonalDataController personal_data_controller = new PersonalDataController();
        selectedDeckIndex = PlayerPrefs.GetInt("SelectedDeck", 0) - 1;
        UpdateButtonHighlight();
        _player_character_type = personal_data_controller.Load().CHARACTER_TYPE;
        CharacterChange(0);
        _character_change_right_button.onClick.AddListener(() => CharacterChange(1));
        _character_change_left_button.onClick.AddListener(() => CharacterChange(-1));
    }

    void CharacterChange(int change ) {
        _player_character_type += change;
        if (_player_character_type == CharacterType.MAX)
        {
            _player_character_type = CharacterType.Monokuma;
        }
        else if(_player_character_type == CharacterType.NULL){
            _player_character_type = CharacterType.MAX - 1;
        }
       
        CharacterTypeInfomation character_info = new CharacterTypeInfomation();
        Debug.Log(character_info.GetCharacterFile(_player_character_type) + " : GetCharacterFile");
        _player_sprite.sprite = TextureToSprite(Resources.Load<Texture2D>(character_info.GetCharacterFile(_player_character_type)) );
        
    }
    public void OnSelectButtonClicked(int deckIndex)
    {
        selectedDeckIndex = deckIndex;
        PlayerPrefs.SetInt("SelectedDeck",(deckIndex + 1));
        PlayerPrefs.Save();
        Debug.Log($"デッキ " + (deckIndex + 1) + " が選択されました");

        UpdateButtonHighlight();
    }
    private void UpdateButtonHighlight()
    {
        int index = 0;
        // 全ハイライトを非表示
        foreach (var highlight in highlights)
        {
            if (index == selectedDeckIndex)
            {
                highlight.SetActive(true);
            }
            else
            {
                highlight.SetActive(false);
            }
            index++;
        }
    }

    public void OnStartButton()
    {
        SceneManager.LoadScene("GameFightingScene");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void OnBackButton()
    {
        SceneManager.LoadScene("HomeScene");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _enemy_character_type = _player_character_type;
        GameObject play_manager = GameObject.Find("PlayingManager");
        if (play_manager != null)
        {
            Debug.Log(_player_character_type);
            play_manager.GetComponent<PlayingManager>().SetCharacterType(_player_character_type , _enemy_character_type);
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    Sprite TextureToSprite(Texture2D texture) {
        return Sprite.Create(
            texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f,0f)
        );
    }
}
