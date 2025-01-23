using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CardLoader : MonoBehaviour
{
    private List<CardData> cardList;

    // �C�ӂ�JSON�t�@�C�������w�肵�ăJ�[�h���X�g��ǂݍ���
    public void LoadCardList(string jsonFileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName); // �g���q�Ȃ��Ŏw��
        if (jsonFile != null)
        {
            Debug.Log("JSON�t�@�C���ǂݍ��ݐ���: " + jsonFileName);
            Debug.Log(jsonFile.text); // JSON���e���m�F
            CardListWrapper cardListWrapper = JsonUtility.FromJson<CardListWrapper>(jsonFile.text);
            if (cardListWrapper != null && cardListWrapper.cards != null)
            {
                cardList = cardListWrapper.cards;
                Debug.Log("�J�[�h���X�g�̃f�V���A���C�Y����");
            }
            else
            {
                Debug.LogError("�J�[�h���X�g�̃f�V���A���C�Y�Ɏ��s���܂���");
            }
        }
        else
        {
            Debug.LogError("JSON�t�@�C����������܂���: " + jsonFileName);
        }
    }
    public List<CardData> LoadCardDeck(string jsonFileName)
    {
        string filePath = Application.persistentDataPath + "/" + jsonFileName + ".json";
        List<CardData> index = null;

        if (File.Exists(filePath))
        {
            // �t�@�C������JSON��ǂݍ���
            string json = File.ReadAllText(filePath);

            // JSON���f�b�L�f�[�^�ɕϊ�
            DeckData deckData = JsonUtility.FromJson<DeckData>(json);

            if (deckData != null && deckData.cards != null)
            {
                index = deckData.cards; // �f�b�L���X�g�ɕ���
                Debug.Log("�f�b�L���ǂݍ��܂�܂���: " + filePath);
            }
            else
            {
                Debug.LogError("�f�b�L�f�[�^�̓ǂݍ��݂Ɏ��s���܂���");
            }
        }
        else
        {
            Debug.Log("�ۑ����ꂽ�f�b�L��������܂���: " + filePath);
        }
        return index;
    }

    // �J�[�h���X�g��Ԃ�
    public List<CardData> GetCardList()
    {
        return cardList;
    }
}

[System.Serializable]
public class CardData
{
    public string cardName;
    public string type;
    public string effect;
    public int amount;
    public int amountHope;
    public int amountDespair;
    public int cost;
    public int costHope;
    public int costDespair;
    public string effectHope;
    public string effectDespair;
    public int despairAmount;
    public int effectType;
    public int effectTypeHope;
    public int effectTypeDespair;
}

[System.Serializable]
public class CardListWrapper
{
    public List<CardData> cards;
}
