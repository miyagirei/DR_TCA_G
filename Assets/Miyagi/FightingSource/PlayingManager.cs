using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayingManager : MonoBehaviour
{
    [SerializeField] DataController _data_controller;
    const int MAX_HP = 10;
    [SerializeField] Text _turn_Text;
    [SerializeField] Text _result_Text;

    [SerializeField]Player _player1;
    [SerializeField]Deck _deck1;
    [SerializeField]Hands _hands1;
    [SerializeField]PlayerUIManager _player1_UI;
    
    [SerializeField]Player _player2;
    [SerializeField]Deck _deck2;
    [SerializeField]Hands _hands2;
    [SerializeField]PlayerUIManager _player2_UI;

    float _progress_time = 0;
    bool _conclusion = false;
    [HideInInspector]float CPU_THINGKING_TIME = 0;
    [HideInInspector] float SCENE_CHANGE_TIME = 0;

    bool _cpu_draw = false;
    async void Start()
    {
        CPU_THINGKING_TIME = await _data_controller.GetParamValueFloat("CPU_THINGKING_TIME");
        SCENE_CHANGE_TIME = await _data_controller.GetParamValueInt("SCENE_CHANGE_TIME");

        _progress_time = 0;
        _conclusion = false;
        _player1 = new Player("player" , MAX_HP , true , _deck1 , _hands1);

        _player2 = new Player("enemy" , MAX_HP , false , _deck2 , _hands2);
    }

    void Update()
    {
        if (!_data_controller.isWaiting()) {
            return;
        }

        _progress_time += Time.deltaTime;

        if (_conclusion) {
            if (_progress_time >= SCENE_CHANGE_TIME) {
                SceneManager.LoadScene("ResultScene");
                SceneManager.sceneLoaded += OnSceneLoaded;
                return;
            }
            return;
        }
        _player1_UI.Display(_player1);
        _player2_UI.Display(_player2);
        if (_player1.IsCurrentPlayer()) {
            PlayerMoveing(_player1 , _player2) ;
            DebugWin();
        }
        if (_player2.IsCurrentPlayer()) {
            CPUMoving(_player2, _player1);
            DebugLose();
        }
    }

    //�v���C���[������
    void PlayerMoveing(Player player , Player enemy) {
        if (Input.GetMouseButtonDown(0))
        {
            if (player.GetHands().PickDrawDeck(player.GetDeck()) == null || player.GetDeck().GetDeckCount() == 0)
            {
                return;
            }
            player.GetHands().ResetHandCards(4, player.GetHands().PickDrawDeck(player.GetDeck()), new Vector3(0, -4));
            player.GetHands().SelectedPlayable(true);
        }
        if (player.GetHands().IsPlayedCard() != null)
        {
            if (player.GetHands().GetCardCount() <= player.GetHands().IsPlayedCard().GetCost()) {
                player.GetHands().IsPlayedCard().ReturnCard();
                return;
            }

            if (!player.GetHands().isCardSelectionValid(player.GetHands().IsPlayedCard().GetCost())) {
                player.GetHands().ShowAvailableCards();
                return;
            }

            int previous_hp = enemy.GetHP();
            player.GetHands().IsPlayedCard().Effect(player , enemy , new Vector3(0,-4));
            player.GetHands().TrashCard(player.GetHands().IsPlayedCard());
            Debug.Log(previous_hp + " > " + enemy.GetHP());

            player.GetHands().discardSelectedCards();
            player.GetHands().arrangeCards(-4);
            TurnChange(enemy);
        }
    }

    //CPU������
    void CPUMoving(Player player , Player enemy) {
        if (_progress_time < CPU_THINGKING_TIME)
        {
            return;
        }
        if (!_cpu_draw) {

            player.GetHands().ResetHandCards(4, player.GetDeck() , new Vector3(0, 4));
            _cpu_draw = true;
            _progress_time = 0;
        }

        if (_progress_time < CPU_THINGKING_TIME) {
            return;
        }
        if (player.GetHands().checkHighDamageCard() != null) {

            int previous_hp = enemy.GetHP();
            enemy.AddHP(-player.GetHands().checkHighDamageCard().GetDamage());
            player.GetHands().TrashCard(player.GetHands().checkHighDamageCard());
            Debug.Log(previous_hp + " > " + enemy.GetHP());
            TurnChange(enemy);
            return;
        }

        if (player.GetHands().checkHighDamageCard() == null)
        {
            Debug.Log("Non-Card");
            TurnChange(enemy);
            return;//�J�[�h�̖������R�X�g��������Ă�����g���Ȃ�
        }
    }

    //�^�[�����I�����đ���ɍs�����ڂ�
    void TurnChange(Player turn) {
        turn.SetCurrentPlayer(true);
        turn.GetHands().SelectedPlayable(true);

        Hostile(turn).SetCurrentPlayer(false);
        Hostile(turn).GetHands().SelectedPlayable(false);

        _turn_Text.text = turn.GetName() + " Turn";
        _progress_time = 0;
        _cpu_draw = false;
        checkResult();
    }

    //�v���C���[1�Ȃ�v���C���[2�ɁA�v���C���[2�Ȃ�v���C���[1��Ԃ�
    Player Hostile(Player my) {
        if (my == _player1) {
            return _player2;
        }

        return _player1;
    }

    //�����������ǂ��炩�������Ă��邩�𔻒肷��
    void checkResult() {
        if (_player1.GetHP() <= 0 && _player2.GetHP() <= 0) {
            Debug.Log("��������");
            _result_Text.text = "��������";
            _conclusion = true;
            _player1.SetWinningState();
            _player2.SetWinningState();
            return;
        }

        if (_player1.GetHP() <= 0) {
            Debug.Log(_player1.GetName() + "�̕���");
            _result_Text.text = _player1.GetName() + "�̕���";
            _conclusion = true;
            _player2.SetWinningState();
            return;
        }        
        
        if (_player2.GetHP() <= 0) {
            Debug.Log(_player2.GetName() + "�̕���");
            _result_Text.text = _player2.GetName() + "�̕���";
            _conclusion = true;
            _player1.SetWinningState();
            return;
        }
    }

    //ResultScene�ɏ����󋵂�n��
    void OnSceneLoaded(Scene scene , LoadSceneMode mode) {
        GameObject result_data = GameObject.Find("ResultDataController");
        if (result_data != null) {
            result_data.GetComponent<ResultDataController>().SetResultData(_player1.GetWinning() , _player2.GetWinning());
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //�f�o�b�N�p//�����I�ɏ�������
    void DebugWin() {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.Space)) {
            _player2.SetHP(0);
            checkResult();
        }
    }

    //�f�o�b�N�p//�����I�ɔs�k����
    void DebugLose() {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.Space)) {
            _player1.SetHP(0);
            checkResult();
        }
    }
}