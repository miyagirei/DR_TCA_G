using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DeckEditor : MonoBehaviour
{
    [SerializeField] private GameObject cardUIPrefab; // �J�[�hUI�v���n�u
    [SerializeField] private Transform cardListParent; // �J�[�h���X�g��z�u����e�I�u�W�F�N�g
    [SerializeField] private Transform deckListParent; // �f�b�L���̃J�[�h��z�u����e�I�u�W�F�N�g
    [SerializeField] private RectTransform cardListContentRectTransform; // �J�[�h���X�g��Content
    [SerializeField] private RectTransform deckListContentRectTransform; // �f�b�L���X�g��Content

    [SerializeField] private int cardPerRow = 5; // ���ɔz�u�����J�[�h�̖���
    private List<CardData> cardList = new List<CardData>();
    private List<CardData> deckList = new List<CardData>(); // �f�b�L�ɒǉ����ꂽ�J�[�h

    [SerializeField]private CardLoader cardLoader;
    private GridLayoutGroup cardGridLayout;
    private GridLayoutGroup deckGridLayout;
    private string[] deckNames = { "Deck1", "Deck2", "Deck3", "Deck4", "Deck5", "Deck6" };
    private int currentDeckIndex;

    // �f�b�L�I����ʂƕҏW��ʂ�UI���i�[����p�l��
    [SerializeField] private GameObject deckSelectPanel;
    [SerializeField] private GameObject deckEditorPanel;

    List<CardData> _prefab_deck_obj = new List<CardData>();
    Dictionary<CardData, GameObject> _deck_data = new Dictionary<CardData, GameObject>();

    List<CardData> _prefab_card_obj = new List<CardData>();
    Dictionary<CardData, GameObject> _card_data = new Dictionary<CardData, GameObject>();

    int _loading_count = 0;
    int _deck_count = 0;

    void Start()
    {
        cardLoader = GetComponent<CardLoader>(); // CardLoader �X�N���v�g���擾
        cardGridLayout = cardListParent.GetComponent<GridLayoutGroup>();
        deckGridLayout = deckListParent.GetComponent<GridLayoutGroup>();

        cardList = cardLoader.GetNetworkCardData("card_data");
        LoadDeck(); // �����f�b�L��ǂݍ���
        DisplayCards(); // �J�[�h���X�g��\��
    }

    void LoadDeck()
    {
        // ���ݑI������Ă���f�b�L��ǂݍ���
        string filePath = Application.persistentDataPath + "/deck" + (currentDeckIndex + 1) + ".json";

        if (File.Exists(filePath))
        {
            // �t�@�C������JSON��ǂݍ���
            string json = File.ReadAllText(filePath);

            // JSON���f�b�L�f�[�^�ɕϊ�
            DeckData deckData = JsonUtility.FromJson<DeckData>(json);

            if (deckData != null && deckData.cards != null)
            {
                deckList = deckData.cards; // �f�b�L���X�g�ɕ���
                Debug.Log("�f�b�L���ǂݍ��܂�܂���: " + filePath);
                DisplayDeck();
            }
            else
            {
                Debug.LogError("�f�b�L�f�[�^�̓ǂݍ��݂Ɏ��s���܂���");
                deckList.Clear(); // �f�b�L���X�g����ɂ���
                DisplayDeck();
            }
        }
        else
        {
            Debug.Log("�ۑ����ꂽ�f�b�L��������܂���: " + filePath);
            deckList.Clear(); // �f�b�L���X�g����ɂ���
            //UpdateDeckUI(); // ��̃f�b�LUI���X�V
            DisplayDeck();
        }
    }

    private void Update()
    {
        SetImageData();
        SetDeckImage();
    }

    void SetImageData()
    {
        if (_loading_count >= _card_data.Count)
        {
            return;
        }

        CardUI card_ui = _card_data[_prefab_card_obj[_loading_count]].GetComponent<CardUI>();
        card_ui.SetCardData(_prefab_card_obj[_loading_count]);
        _loading_count++;
    }    
    void DisplayCards()
    {
        cardLoader.Get("card_data");
        List<CardData> cardList = cardLoader.GetCardList(); // CardLoader ����J�[�h���X�g���擾

        if (cardList != null)
        {
            foreach (var card in cardList)
            {
                GameObject cardUI = Instantiate(cardUIPrefab, cardListParent);
                _prefab_card_obj.Add(card);
                _card_data.Add(card, cardUI);

                Button addButton = cardUI.GetComponentInChildren<Button>();
                addButton.onClick.AddListener(() => AddCardToDeck(card));
            }
            AdjustContentHeight();
        }
        else
        {
            Debug.LogError("�J�[�h���X�g���擾�ł��܂���");
        }
    }
    
    void SetDeckImage()
    {
        if (_deck_count >= _deck_data.Count)
        {
            return;
        }

        Debug.Log(_deck_count + "count : " + _deck_data.Count );
        CardUI card_ui = _deck_data[_prefab_deck_obj[_deck_count]].GetComponent<CardUI>();
        card_ui.SetCardData(_prefab_deck_obj[_deck_count]);
        _deck_count++;
    }
    void DisplayDeck()
    {
        _deck_data.Clear();
        _prefab_deck_obj.Clear();
        foreach (Transform child in deckListParent)
        {
            Destroy(child.gameObject);
        }

        _deck_count = 0;
        List<CardData> cardList = deckList; 

        if (cardList != null)
        {
            foreach (var card in cardList)
            {
                GameObject cardUI = Instantiate(cardUIPrefab, deckListParent);
                _deck_data.Add(card, cardUI);
                _prefab_deck_obj.Add(card);
                // �f�b�L����J�[�h���폜����{�^����ǉ�
                Button removeButton = cardUI.GetComponentInChildren<Button>();
                removeButton.onClick.AddListener(() => RemoveCardFromDeck(card));

            }
            AdjustContentHeight();
        }
        else
        {
            Debug.LogError("�J�[�h���X�g���擾�ł��܂���");
        }
    }

    void AddCardToDeck(CardData card)
    {
            // �f�b�L�ɓ����̃J�[�h��4�������ł���Βǉ�
            int cardCount = deckList.FindAll(c => c.card_name == card.card_name).Count;
            if (cardCount < 4) // �����J�[�h��4�������ł���Βǉ�
            {
                deckList.Add(card);
                CreateCardData(card);
            }
            else
            {
                SoundManager.PlaySoundStatic(SoundType.AlertsSound);
                Debug.Log("���̃J�[�h��4���܂ł����ǉ��ł��܂���");
            }
        AdjustDeckContentHeight();
    }

    void RemoveCardFromDeck(CardData card)
    {
        SoundManager.PlaySoundStatic(SoundType.TrashSound);
        deckList.Remove(card);
        Destroy(_deck_data[card]);
        AdjustDeckContentHeight();

    }

    void CreateCardData(CardData card) {
        SoundManager.PlaySoundStatic(SoundType.DrawSound);
        GameObject cardUI = Instantiate(cardUIPrefab, deckListParent);
        _deck_data.Add(card, cardUI);
        _prefab_deck_obj.Add(card);
        CardUI card_ui = cardUI.GetComponent<CardUI>();
        card_ui.SetCardData(card);
        // �f�b�L����J�[�h���폜����{�^����ǉ�
        Button removeButton = cardUI.GetComponentInChildren<Button>();
        removeButton.onClick.AddListener(() => RemoveCardFromDeck(card));
        _deck_count++;
    }

    void AdjustContentHeight()
    {
        // �J�[�h���X�g��Content�����𒲐�
        int cardCount = cardListParent.childCount;
        float contentHeight = Mathf.CeilToInt((float)cardCount / cardPerRow) * 350f; // �������v�Z

        // Content�̍�����ݒ�
        cardListContentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentHeight);
    }

    void AdjustDeckContentHeight()
    {
        // �f�b�L���X�g��Content�����𒲐�
        int deckCount = deckListParent.childCount;
        float contentHeight = Mathf.CeilToInt((float)deckCount / cardPerRow) * 350f; // �������v�Z

        // Content�̍�����ݒ�
        deckListContentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentHeight);
    }

    // �f�b�L�̕ۑ�����
    public void SaveDeck()
    {
        SoundManager.PlaySoundStatic(SoundType.HopeSound);

        // ���ݑI������Ă���f�b�L�̃f�[�^��JSON�`���ɕϊ���
        DeckData deckData = new DeckData();
        deckData.cards = deckList;

        string json = JsonUtility.ToJson(deckData, true);

        // �t�@�C���p�X��ݒ�
        string filePath = Application.persistentDataPath + "/deck" + (currentDeckIndex + 1) + ".json";

        // JSON�t�@�C����ۑ�
        File.WriteAllText(filePath, json);
        Debug.Log("�f�b�L���ۑ�����܂���: " + filePath);
    }

    public void SetMainDeck()
    {
        SoundManager.PlaySoundStatic(SoundType.HopeSound);

        PlayerPrefs.SetInt("SelectedDeck", (currentDeckIndex + 1));
        PlayerPrefs.Save();
        Debug.Log($"�f�b�L "+(currentDeckIndex + 1)+" ���I������܂���");
    }

    // �f�b�L�I���{�^���̏���
    public void OnDeckButtonClicked(int deckIndex)
    {
        SoundManager.PlaySoundStatic(SoundType.DecisionSound);
        currentDeckIndex = deckIndex;
        LoadDeck(); // �I�������f�b�L��ǂݍ���
        deckSelectPanel.SetActive(false); // �f�b�L�I����ʂ��\��
        deckEditorPanel.SetActive(true); // �f�b�L�ҏW��ʂ�\��
    }

    // �߂�{�^��
    public void OnBackButtonClicked()
    {
        SoundManager.PlaySoundStatic(SoundType.ReturnSound);
        deckSelectPanel.SetActive(true); // �f�b�L�I����ʂ�\��
        deckEditorPanel.SetActive(false); // �f�b�L�ҏW��ʂ��\��
    }
}

[System.Serializable]
public class DeckData
{
    public List<CardData> cards;
}

