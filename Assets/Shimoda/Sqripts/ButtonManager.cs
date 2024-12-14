using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button buttonCard;
    public Button buttonHome;
    public Button buttonBattle;
    public Button buttonOption;
    public GameObject buttonDeckAndList;
    public GameObject buttonStoryAndBattle;

    void Start()
    {
        buttonCard.onClick.AddListener(OnCardButtonClicked);
        buttonHome.onClick.AddListener(OnHomeButtonClicked);
        buttonBattle.onClick.AddListener(OnBattleButtonClicked);
        buttonOption.onClick.AddListener(OnOptionButtonClicked);

        buttonDeckAndList.SetActive(false);
        buttonStoryAndBattle.SetActive(false);
    }

    void OnCardButtonClicked()
    {
        buttonCard.gameObject.SetActive(false);
        buttonBattle.gameObject.SetActive(true);
        buttonDeckAndList.SetActive(true);
        buttonStoryAndBattle.SetActive(false);
    }
    void OnHomeButtonClicked()
    {
        buttonCard.gameObject.SetActive(true);
        buttonBattle.gameObject.SetActive(true);
        buttonDeckAndList.SetActive(false);
        buttonStoryAndBattle.SetActive(false);
    }
    void OnBattleButtonClicked()
    {
        buttonCard.gameObject.SetActive(true);
        buttonBattle.gameObject.SetActive(false);
        buttonDeckAndList.SetActive(false);
        buttonStoryAndBattle.SetActive(true);
    }
    void OnOptionButtonClicked()
    {

    }
}
