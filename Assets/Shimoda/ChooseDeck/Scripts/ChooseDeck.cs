using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseDeck : MonoBehaviour
{
    [SerializeField] private List<Button> deckButtons; // �f�b�L�ɑΉ�����{�^��
    [SerializeField] private List<GameObject> highlights; // �n�C���C�g�p�̃I�u�W�F�N�g���X�g
    private int selectedDeckIndex = 1;
    // Start is called before the first frame update
    void Start()
    {
        selectedDeckIndex = PlayerPrefs.GetInt("SelectedDeck", 0) - 1;
        UpdateButtonHighlight();
    }

    public void OnSelectButtonClicked(int deckIndex)
    {
        selectedDeckIndex = deckIndex;
        PlayerPrefs.SetInt("SelectedDeck",(deckIndex + 1));
        PlayerPrefs.Save();
        Debug.Log($"�f�b�L " + (deckIndex + 1) + " ���I������܂���");

        UpdateButtonHighlight();
    }
    private void UpdateButtonHighlight()
    {
        int index = 0;
        // �S�n�C���C�g���\��
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
    }
    public void OnBackButton()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
