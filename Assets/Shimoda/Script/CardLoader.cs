using System.Collections.Generic;
using UnityEngine;

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
    public int cost;
    public int costHope;
    public int costDespair;
    public string effectHope;
    public string effectDespair;
    public int despairAmount;
}

[System.Serializable]
public class CardListWrapper
{
    public List<CardData> cards;
}
