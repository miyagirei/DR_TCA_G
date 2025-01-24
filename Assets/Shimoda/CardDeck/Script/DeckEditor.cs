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

    private CardLoader cardLoader;
    private GridLayoutGroup cardGridLayout;
    private GridLayoutGroup deckGridLayout;
    private string[] deckNames = { "Deck1", "Deck2", "Deck3", "Deck4", "Deck5", "Deck6" };
    private int currentDeckIndex;

    // �f�b�L�I����ʂƕҏW��ʂ�UI���i�[����p�l��
    [SerializeField] private GameObject deckSelectPanel;
    [SerializeField] private GameObject deckEditorPanel;

    void Start()
    {
        cardLoader = GetComponent<CardLoader>(); // CardLoader �X�N���v�g���擾
        cardGridLayout = cardListParent.GetComponent<GridLayoutGroup>();
        deckGridLayout = deckListParent.GetComponent<GridLayoutGroup>();

        LoadCardList("cardList"); // �C�ӂ̃J�[�h���X�g��ǂݍ���
        LoadDeck(); // �����f�b�L��ǂݍ���
        DisplayCards(); // �J�[�h���X�g��\��
    }

    void LoadCardList(string jsonFileName)
    {
        // JSON�t�@�C������J�[�h���X�g��ǂݍ���
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);
        if (jsonFile != null)
        {
            CardListWrapper cardListWrapper = JsonUtility.FromJson<CardListWrapper>(jsonFile.text);
            if (cardListWrapper != null && cardListWrapper.cards != null)
            {
                cardList = cardListWrapper.cards;
                Debug.Log("�J�[�h���X�g�̓ǂݍ��ݐ���");
            }
            else
            {
                Debug.LogError("�J�[�h���X�g�̓ǂݍ��݂Ɏ��s���܂���");
            }
        }
        else
        {
            Debug.LogError("�J�[�h���X�g��JSON�t�@�C����������܂���");
        }
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
                UpdateDeckUI();
            }
            else
            {
                Debug.LogError("�f�b�L�f�[�^�̓ǂݍ��݂Ɏ��s���܂���");
                deckList.Clear(); // �f�b�L���X�g����ɂ���
                UpdateDeckUI(); // ��̃f�b�LUI���X�V
            }
        }
        else
        {
            Debug.Log("�ۑ����ꂽ�f�b�L��������܂���: " + filePath);
            deckList.Clear(); // �f�b�L���X�g����ɂ���
            UpdateDeckUI(); // ��̃f�b�LUI���X�V
        }
    }

    void DisplayCards()
    {
        foreach (var card in cardList)
        {
            GameObject cardUI = Instantiate(cardUIPrefab, cardListParent);
            CardUI cardUIScript = cardUI.GetComponent<CardUI>();
            cardUIScript.SetCardData(card);

            // �J�[�h�Ƀf�b�L�ɒǉ�����{�^����ǉ�
            Button addButton = cardUI.GetComponentInChildren<Button>();
            addButton.onClick.AddListener(() => AddCardToDeck(card));
        }
        AdjustContentHeight();
    }

    void AddCardToDeck(CardData card)
    {
            // �f�b�L�ɓ����̃J�[�h��4�������ł���Βǉ�
            int cardCount = deckList.FindAll(c => c.cardName == card.cardName).Count;
            if (cardCount < 4) // �����J�[�h��4�������ł���Βǉ�
            {
                deckList.Add(card);
                UpdateDeckUI();
            }
            else
            {
                Debug.Log("���̃J�[�h��4���܂ł����ǉ��ł��܂���");
            }
        AdjustDeckContentHeight();
    }

    void RemoveCardFromDeck(CardData card)
    {
        deckList.Remove(card);
        UpdateDeckUI();
    }

    void UpdateDeckUI()
    {
        // �f�b�L���̃J�[�h���X�V
        foreach (Transform child in deckListParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var card in deckList)
        {
            GameObject cardUI = Instantiate(cardUIPrefab, deckListParent);
            CardUI cardUIScript = cardUI.GetComponent<CardUI>();
            cardUIScript.SetCardData(card);

            // �f�b�L����J�[�h���폜����{�^����ǉ�
            Button removeButton = cardUI.GetComponentInChildren<Button>();
            removeButton.onClick.AddListener(() => RemoveCardFromDeck(card));
        }
        AdjustDeckContentHeight();
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
        // ���ݑI������Ă���f�b�L�̃f�[�^��JSON�`���ɕϊ�
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
        PlayerPrefs.SetInt("SelectedDeck", (currentDeckIndex + 1));
        PlayerPrefs.Save();
        Debug.Log($"�f�b�L "+(currentDeckIndex + 1)+" ���I������܂���");
    }

    // �f�b�L�I���{�^���̏���
    public void OnDeckButtonClicked(int deckIndex)
    {
        currentDeckIndex = deckIndex;
        LoadDeck(); // �I�������f�b�L��ǂݍ���
        deckSelectPanel.SetActive(false); // �f�b�L�I����ʂ��\��
        deckEditorPanel.SetActive(true); // �f�b�L�ҏW��ʂ�\��
    }

    // �߂�{�^��
    public void OnBackButtonClicked()
    {
        deckSelectPanel.SetActive(true); // �f�b�L�I����ʂ�\��
        deckEditorPanel.SetActive(false); // �f�b�L�ҏW��ʂ��\��
    }

    // �f�b�L�I����ʂ�UI�{�^����ݒ�
    void SetDeckButtons()
    {
        for (int i = 0; i < deckNames.Length; i++)
        {
            GameObject deckButton = new GameObject("DeckButton" + i);
            Button button = deckButton.AddComponent<Button>();
            button.GetComponentInChildren<Text>().text = deckNames[i];

            int deckIndex = i;
            button.onClick.AddListener(() => OnDeckButtonClicked(deckIndex));

            deckButton.transform.SetParent(deckSelectPanel.transform);
        }
    }
}

[System.Serializable]
public class DeckData
{
    public List<CardData> cards;
}

