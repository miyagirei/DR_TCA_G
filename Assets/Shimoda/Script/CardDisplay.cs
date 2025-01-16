using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private GameObject cardUIPrefab; // �J�[�hUI�v���n�u
    [SerializeField] private Transform cardListParent; // �J�[�h���X�g��z�u����e�I�u�W�F�N�g
    [SerializeField] private RectTransform contentRectTransform; // �X�N���[���r���[��Content��RectTransform
    [SerializeField] private float cardHeight = 100f; // 1��������̃J�[�h�̍����i�K�X�����j
    [SerializeField] private int cardPerRow = 5; //�@���ɔz�u�����J�[�h�̖���

    private CardLoader cardLoader;

    void Start()
    {
        cardLoader = GetComponent<CardLoader>(); // CardLoader �X�N���v�g���擾
        if (cardLoader != null)
        {
            // �C�ӂ̃V�[����ݒ�ɉ����āA�قȂ�JSON�t�@�C�����w��
            cardLoader.LoadCardList("cardList"); // cardList.json ��ǂݍ���
            DisplayCards();
        }
        else
        {
            Debug.LogError("CardLoader �X�N���v�g��������܂���");
        }
    }

    void DisplayCards()
    {
        List<CardData> cardList = cardLoader.GetCardList(); // CardLoader ����J�[�h���X�g���擾

        if (cardList != null)
        {
            foreach (var card in cardList)
            {
                GameObject cardUI = Instantiate(cardUIPrefab, cardListParent);
                CardUI cardUIScript = cardUI.GetComponent<CardUI>();
                cardUIScript.SetCardData(card);
            }
            AdjustContentHeight();
        }
        else
        {
            Debug.LogError("�J�[�h���X�g���擾�ł��܂���");
        }
    }

    void AdjustContentHeight()
    {
        // �z�u���ꂽ�J�[�h�̐��Ɋ�Â���Content�̍�����ݒ�
        int cardCount = cardListParent.childCount;
        float contentHeight = cardCount /cardPerRow * cardHeight;

        // Content�̍�����ݒ�
        contentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentHeight);
    }
}
