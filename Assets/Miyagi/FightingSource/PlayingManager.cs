using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayingManager : MonoBehaviour
{
    [SerializeField] DataController _data_controller;
    const int MAX_HP = 10;
    [SerializeField] Text _result_Text;

    [SerializeField]Player _player1;
    [SerializeField]Deck _deck1;
    [SerializeField]Hands _hands1;
    [SerializeField]PlayerUIManager _player1_UI;
    
    [SerializeField]Player _player2;
    [SerializeField]Deck _deck2;
    [SerializeField]Hands _hands2;
    [SerializeField]EnemyUIManager _player2_UI;

    float _progress_time = 0;
    bool _conclusion = false;
    [HideInInspector]float CPU_THINGKING_TIME = 0;
    [HideInInspector] float SCENE_CHANGE_TIME = 0;
    [HideInInspector] float TURN_CHANGE_TIME = 0;
    [HideInInspector] float CARD_EFFECT_DISTANCE = 0;
    [HideInInspector] float TIME_UNTIL_TIME_RUNS_OUT = 0;
    [HideInInspector] int RESET_CARDS_COUNT = 0;

    bool _cpu_draw = false;
    float _cpu_incapacity_time = 0;

    bool _turn_change_animation = false;
    Player _turn_player;

    PlayingSituation _turn_situation = PlayingSituation.Hopeful;

    void ResetProgressTime() => _progress_time = 0;
    async void Start()
    {
        CPU_THINGKING_TIME = await _data_controller.GetParamValueFloat("CPU_THINGKING_TIME");
        SCENE_CHANGE_TIME = await _data_controller.GetParamValueFloat("SCENE_CHANGE_TIME");
        TURN_CHANGE_TIME = await _data_controller.GetParamValueFloat("TURN_CHANGE_TIME");
        CARD_EFFECT_DISTANCE = await _data_controller.GetParamValueFloat("CARD_EFFECT_DISTANCE");
        TIME_UNTIL_TIME_RUNS_OUT = await _data_controller.GetParamValueFloat("TIME_UNTIL_TIME_RUNS_OUT");
        RESET_CARDS_COUNT = await _data_controller.GetParamValueInt("RESET_CARDS_COUNT");

        ResetProgressTime();
        _conclusion = false;
        _player1 = new Player("player" , MAX_HP , true , _deck1 , _hands1 ,new Vector3(0, -4) , new Vector3(2.8f , 4 , 1));
        _turn_player = _player1;

        _player2 = new Player("enemy" , MAX_HP , false , _deck2 , _hands2 , new Vector3(3, 0) , new Vector3(0.7f, 1, 1));

        _player1_UI.AssingTurnChangeButton(() => TurnChange(_player2));
        RandomolyChooseTurnPlayer();
    }

    public float GetTimer() => TIME_UNTIL_TIME_RUNS_OUT - _progress_time;
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

        if (_turn_change_animation && _progress_time < TURN_CHANGE_TIME) {
            _player1_UI.DisplayTurnChangePanel(_turn_player ,_turn_situation, true );
            _player1_UI.DisplayTurnChangeButton(false);
            _player1_UI.DisplayTimer(GetTimer(), false);
            return;
        }else if (_turn_change_animation && _progress_time > TURN_CHANGE_TIME) {
            _player1_UI.DisplayTurnChangePanel(_turn_player, _turn_situation, false);
            ResetProgressTime();
           
            _turn_change_animation = false;
        }

        _player1_UI.DisplayTimer(GetTimer(), true);
        if (_player1.IsCurrentPlayer()) {
            PlayerMoveing(_player1 , _player2) ;
            DebugWin();
            _player1_UI.DisplayTurnChangeButton(true);

            if (_player1.GetHands().IsDraggingCard())
            {
                _player1_UI.DisplayCardEffectDistance(CARD_EFFECT_DISTANCE, true);
            }
            else {
                _player1_UI.DisplayCardEffectDistance(CARD_EFFECT_DISTANCE, false);
            }
        }
        if (_player2.IsCurrentPlayer()) {
            CPUMoving(_player2, _player1);
            DebugLose();
            _player1_UI.DisplayTurnChangeButton(false);
        }

        TimeRunsAndTheTurnEnd();
    }

    //プレイヤーが操作
    void PlayerMoveing(Player player , Player enemy) {
        if (player.GetHands().IsPlayedCard(player) != null)
        {
            if (player.GetHands().GetCardCount() <= player.GetHands().IsPlayedCard(player).GetCostByCondition(player)) {
                player.GetHands().IsPlayedCard(player).ReturnCard();
                return;
            }

            if (!player.GetHands().isCardSelectionValid(player.GetHands().IsPlayedCard(player).GetCostByCondition(player))) {
                player.GetHands().ShowAvailableCards(player.GetHands().IsPlayedCard(player));
                return;
            }

            int previous_hp = enemy.GetHP();
            Card played_card = player.GetHands().IsPlayedCard(player);
            played_card.Effect(player , enemy , player.GetCardPos() , player.GetCardScale() , _turn_situation);
            PlayingLogger.LogStatic(player.GetName() + "が効果を発動 : " + played_card.GetEffectByCondition(player) + "(" + played_card.GetEffectAmount(played_card.GetEffectByCondition(player)) + ")", Color.green);
            player.GetHands().TrashCard(played_card);
            Debug.Log(previous_hp + " > " + enemy.GetHP());

            player.GetHands().discardSelectedCards();
            player.GetHands().arrangeCards(player.GetCardPos(), player.GetCardScale());
            player.GetHands().SelectedPlayable(true);
            checkResult();
        }
    }

    //CPUが操作
    void CPUMoving(Player player , Player enemy) {
        _cpu_incapacity_time+=Time.deltaTime;
        if (_cpu_incapacity_time < CPU_THINGKING_TIME)
        {
            return;
        }
        if (!_cpu_draw) {

            player.GetHands().ResetHandCards(RESET_CARDS_COUNT, player.GetDeck() , player.GetCardPos() , player.GetCardScale());
            _cpu_draw = true;
            ResetCPUIncapacityTime();
        }

        if (_cpu_incapacity_time < CPU_THINGKING_TIME) {
            return;
        }
        if (player.GetHands().CheckMostExpensiveCardYouCanPay(player) != null) {

            int previous_hp = enemy.GetHP();
            Card played_card = player.GetHands().CheckMostExpensiveCardYouCanPay(player);
            played_card.Effect(player, enemy, player.GetCardPos(), player.GetCardScale() , _turn_situation);
            PlayingLogger.LogStatic(player.GetName() + "が効果を発動 : " + played_card.GetEffectByCondition(player) + "(" + played_card.GetEffectAmount(played_card.GetEffectByCondition(player)) + ")", Color.red);
            player.GetHands().TrashCard(played_card);
            Debug.Log(previous_hp + " > " + enemy.GetHP());

            checkResult();

            player.GetHands().discardSelectedCards();
            player.GetHands().arrangeCards(player.GetCardPos(), player.GetCardScale());
            player.GetHands().SelectedPlayable(true);
            TurnChange(enemy);
            ResetCPUIncapacityTime();
            return;
        }

        if (player.GetHands().checkHighDamageCard() == null)
        {
            Debug.Log("Non-Card");
            checkResult();
            TurnChange(enemy);
            ResetCPUIncapacityTime();
            return;//カードの枚数がコストを下回っていたら使えない
        }
    }

    //CPUの稼働時間をリセットする
    void ResetCPUIncapacityTime() => _cpu_incapacity_time = 0;

    //ターンを終了して相手に行動を移す
    void TurnChange(Player turn) {
        turn.SetCurrentPlayer(true);
        turn.GetHands().SelectedPlayable(true);

        Hostile(turn).SetCurrentPlayer(false);
        Hostile(turn).GetHands().SelectedPlayable(false);

        _turn_player = turn;
        ResetProgressTime();
        _cpu_draw = false;

        if(turn.GetDeck().GetDeckCount() >= 0) {
            turn.GetHands().ResetHandCards(RESET_CARDS_COUNT, turn.GetDeck(), turn.GetCardPos() , turn.GetCardScale());
            turn.GetHands().SelectedPlayable(true);
        }
        _turn_change_animation = true;
    }

    //プレイヤー1ならプレイヤー2に、プレイヤー2ならプレイヤー1を返す
    Player Hostile(Player my) {
        if (my == _player1) {
            return _player2;
        }

        return _player1;
    }

    //引き分けかどちらかが勝っているかを判定する
    void checkResult() {
        if (_player1.GetHP() <= 0 && _player2.GetHP() <= 0) {
            Debug.Log("引き分け");
            _result_Text.text = "引き分け";
            _conclusion = true;
            _player1.SetWinningState();
            _player2.SetWinningState();
            return;
        }

        if (_player1.GetHP() <= 0) {
            Debug.Log(_player1.GetName() + "の負け");
            _result_Text.text = _player1.GetName() + "の負け";
            _conclusion = true;
            _player2.SetWinningState();
            return;
        }        
        
        if (_player2.GetHP() <= 0) {
            Debug.Log(_player2.GetName() + "の負け");
            _result_Text.text = _player2.GetName() + "の負け";
            _conclusion = true;
            _player1.SetWinningState();
            return;
        }
    }

    //ResultSceneに勝利状況を渡す
    void OnSceneLoaded(Scene scene , LoadSceneMode mode) {
        GameObject result_data = GameObject.Find("ResultDataController");
        if (result_data != null) {
            result_data.GetComponent<ResultDataController>().SetResultData(_player1.GetWinning() , _player2.GetWinning());
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //デバック用//強制的に勝利する
    void DebugWin() {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.Space)) {
            _player2.SetHP(0);
            checkResult();
        }
    }

    //デバック用//強制的に敗北する
    void DebugLose() {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.Space)) {
            _player1.SetHP(0);
            checkResult();
        }
    }

    //ランダムでターンプレイヤーを決める
    void RandomolyChooseTurnPlayer() {
        int random_player = Random.Range(0, 2);

        if (random_player == 0)
        {
            TurnChange(_player1);
        }
        else {
            TurnChange(_player2);
        }
    }

    //時間切れの際に相手にターンを回す
    void TimeRunsAndTheTurnEnd() {
        if (TIME_UNTIL_TIME_RUNS_OUT <= TURN_CHANGE_TIME || TIME_UNTIL_TIME_RUNS_OUT <= SCENE_CHANGE_TIME) {
            Debug.LogWarning("時間切れまでの時間が短すぎます");
            return;
        }

        if (_progress_time >= TIME_UNTIL_TIME_RUNS_OUT) {
            TurnChange(Hostile(_turn_player));
        }
    }

    void ChangeSituation() {
        if (_turn_situation == PlayingSituation.Hopeful)
        {
            _turn_situation = PlayingSituation.Desperate;
        }
        else 
        {
            _turn_situation = PlayingSituation.Hopeful;
        }
    }
}
