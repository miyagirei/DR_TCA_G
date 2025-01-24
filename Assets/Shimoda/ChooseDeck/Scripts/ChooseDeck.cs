using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseDeck : MonoBehaviour
{
    [SerializeField] private List<Button> deckButtons; // デッキに対応するボタン
    [SerializeField] private List<GameObject> highlights; // ハイライト用のオブジェクトリスト
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
    }
    public void OnBackButton()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
