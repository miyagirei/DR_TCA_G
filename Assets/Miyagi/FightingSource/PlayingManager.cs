using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayingManager : MonoBehaviour
{
    [SerializeField] ParameterData _parameter_data_controller;
    [HideInInspector] float CPU_THINGKING_TIME = 0;
    [HideInInspector] float SCENE_CHANGE_TIME = 0;
    [HideInInspector] float TURN_CHANGE_TIME = 0;
    [HideInInspector] float CARD_EFFECT_DISTANCE = 0;
    [HideInInspector] float TIME_UNTIL_TIME_RUNS_OUT = 0;
    [HideInInspector] int RESET_CARDS_COUNT = 0;
    [HideInInspector] int PLAYER_MAX_HP = 0;

    const int MAX_HP = 10;
    [SerializeField] Text _result_Text;

    [SerializeField] Player _player1;
    [SerializeField] Deck _deck1;
    [SerializeField] Hands _hands1;
    [SerializeField] PlayerUIManager _player1_UI;

    [SerializeField] Player _player2;
    [SerializeField] Deck _deck2;
    [SerializeField] Hands _hands2;
    [SerializeField] EnemyUIManager _player2_UI;

    float _progress_time = 0;
    bool _conclusion = false;

    bool _cpu_draw = false;
    float _cpu_incapacity_time = 0;

    bool _turn_change_animation = false;
    Player _turn_player;

    bool _display_damage = false;
    float _display_damage_time = 0;

    Situation _turn_situation = new Situation();

    CharacterType _player_character_type = CharacterType.Monokuma;
    CharacterType _enemy_character_type = CharacterType.Monokuma;

    bool _enemy_previous_hope = false;
    bool _enemy_previous_despair = false;

    bool _online_trash_box = false;
    bool _reset_card = false;
    string _start_fight_bgm = "BGM_BOX15";
    string _final_fight_bgm = "BGM_DANGANRONPA";

    void SwitchOnlineTrashBox()
    {
        _online_trash_box = !_online_trash_box;

        if (!_online_trash_box) {
            _reset_card = true;
        }
    }
    void ResetProgressTime() => _progress_time = 0;

    public void SetCharacterType(CharacterType player, CharacterType enemy)
    {
        _player_character_type = player;
        _enemy_character_type = enemy;
        _player1_UI.ChangePlayerImage(player);
        _player2_UI.ChangePlayerImage(enemy);
    }
    void Start()
    {
        Parameter parameter_data = _parameter_data_controller.GetParameterData("parameter_data");
        CPU_THINGKING_TIME = parameter_data.CPU_THINGKING_TIME;
        SCENE_CHANGE_TIME = parameter_data.SCENE_CHANGE_TIME;
        TURN_CHANGE_TIME = parameter_data.TURN_CHANGE_TIME;
        CARD_EFFECT_DISTANCE = parameter_data.CARD_EFFECT_DISTANCE;
        TIME_UNTIL_TIME_RUNS_OUT = parameter_data.TIME_UNTIL_TIME_RUNS_OUT;
        RESET_CARDS_COUNT = parameter_data.RESET_CARD_COUNT;
        PLAYER_MAX_HP = parameter_data.PLAYER_MAX_HP;

        ResetProgressTime();
        _conclusion = false;
        _player1 = new Player("player", PLAYER_MAX_HP, true, _deck1, _hands1, new Vector3(0, -4), new Vector3(2.8f, 4f, 1), _player_character_type);

        _player2 = new Player("enemy", PLAYER_MAX_HP, false, _deck2, _hands2, new Vector3(3.0f, -0.5f), new Vector3(0.7f, 1, 1), _enemy_character_type);

        _turn_player = _player1;
        _player1_UI.AssingTurnChangeButton(() => TurnChange(_player2));
        _player1_UI.AssingTrashBoxButton(() => SwitchOnlineTrashBox());
        _player1_UI.ChangeImageTrashBox(_online_trash_box);
        RandomolyChooseTurnPlayer();

        _enemy_previous_hope = false;
        _enemy_previous_despair = false;

        if (GameObject.Find("BGMManager") != null)
        {
            GameObject.Find("BGMManager").GetComponent<BGMManager>().ChangeFightBGM(_start_fight_bgm);
        }

        Debug.Log(RESET_CARDS_COUNT);
    }

    public float GetTimer() => TIME_UNTIL_TIME_RUNS_OUT - _progress_time;
    void Update()
    {
        if (_parameter_data_controller.GetParameterData("parameter_data") == null)
        {
            SceneManager.LoadScene("ResultScene");
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.LogWarning("parameter_dataが存在しません");
        }

        _progress_time += Time.deltaTime;

        if (_conclusion)
        {
            if (_progress_time >= SCENE_CHANGE_TIME)
            {
                SceneManager.LoadScene("ResultScene");
                SceneManager.sceneLoaded += OnSceneLoaded;
                return;
            }
            return;
        }


        _player1_UI.Display(_player1);
        _player2_UI.Display(_player2);
        ChangeBGM();

        if (_display_damage && _display_damage_time < 2.0)
        {
            _display_damage_time += Time.deltaTime;
            return;
        }
        else if (_display_damage && _display_damage_time > 2.0)
        {
            _player1_UI.ShowDamage(0, false);
            _display_damage_time = 0;
            _display_damage = false;
        }

        if (_enemy_previous_hope != _player2.GetHopeCondition() || _enemy_previous_despair != _player2.GetDespairCondition())
        {
            _player1_UI.ChangePlayerImage(_player_character_type, _player1.GetHopeCondition(), _player1.GetDespairCondition());
            _player2_UI.ChangePlayerImage(_enemy_character_type, _player2.GetHopeCondition(), _player2.GetDespairCondition());

            _enemy_previous_hope = _player2.GetHopeCondition();
            _enemy_previous_despair = _player2.GetDespairCondition();
        }

        if (_turn_change_animation && _progress_time < TURN_CHANGE_TIME)
        {
            _player1_UI.DisplayTurnChangePanel(_turn_player, _turn_situation.GetSituation(), true);
            _player1_UI.DisplayTurnChangeButton(false);
            _player1_UI.DisplayTimer(GetTimer(), false);
            return;
        }
        else if (_turn_change_animation && _progress_time > TURN_CHANGE_TIME)
        {
            _player1_UI.DisplayTurnChangePanel(_turn_player, _turn_situation.GetSituation(), false);
            ResetProgressTime();

            _turn_change_animation = false;
        }

        _player1_UI.DisplayTimer(GetTimer(), true);
        if (_player1.IsCurrentPlayer())
        {
            PlayerMoveing(_player1, _player2);
            DebugWin();
            _player1_UI.DisplayTurnChangeButton(true);

            if (_player1.GetHands().IsDraggingCard())
            {
                _player1_UI.DisplayCardEffectDistance(CARD_EFFECT_DISTANCE, true);
            }
            else
            {
                _player1_UI.DisplayCardEffectDistance(CARD_EFFECT_DISTANCE, false);
            }
        }


        if (_player2.IsCurrentPlayer())
        {
            CPUMoving(_player2, _player1);
            DebugLose();
            _player1_UI.DisplayTurnChangeButton(false);
        }

        TimeRunsAndTheTurnEnd();
    }

    //プレイヤーが操作
    void PlayerMoveing(Player player, Player enemy)
    {

        if (player.GetHands().IsPlayedCard(player) != null)
        {

            if (player.GetHands().GetCardCount() <= player.GetHands().IsPlayedCard(player).GetCostByCondition(player))
            {
                player.GetHands().IsPlayedCard(player).ReturnCard();
                return;
            }

            if (!player.GetHands().IsPlayedCard(player).MetRestrictions(player))
            {
                player.GetHands().IsPlayedCard(player).ReturnCard();
                return;
            }

            _online_trash_box = false;
            _player1_UI.TogglePressableStateTrashBox(false);

            if (!player.GetHands().isCardSelectionValid(player.GetHands().IsPlayedCard(player).GetCostByCondition(player)))
            {
                player.GetHands().ShowAvailableCards(player.GetHands().IsPlayedCard(player));
                return;
            }

            int previous_hp = enemy.GetHP();
            Card played_card = player.GetHands().IsPlayedCard(player);
            played_card.Effect(player, enemy, player.GetCardPos(), player.GetCardScale(), _turn_situation);


            int damage = previous_hp - enemy.GetHP(); // ダメージ計算
            if (damage > 0)
            {
                _display_damage = true;
                _player1_UI.ShowDamage(damage, true); // ダメージ表示
            }


            PlayingLogger.LogStatic(player.GetName() + "が効果を発動 : " + played_card.GetEffectByCondition(player) + "(" + played_card.GetAmountByCondition(player, _turn_situation.GetSituation()) + ")", Color.green);
            player.GetHands().TrashCard(played_card);
            Debug.Log(previous_hp + " > " + enemy.GetHP());

            player.GetHands().discardSelectedCards();
            player.GetHands().arrangeCards(player.GetCardPos(), player.GetCardScale());
            player.GetHands().SelectedPlayable(true);
            _player1_UI.TogglePressableStateTrashBox(true);

            checkResult();
        }

        if (_reset_card) {
            player.GetHands().discardSelectedCards();
            player.GetHands().arrangeCards(player.GetCardPos(), player.GetCardScale());
            player.GetHands().SelectedPlayable(true);
            _player1_UI.ChangeImageTrashBox(_online_trash_box);
            _reset_card = false;
        }

        if (_online_trash_box)
        {
            player.GetHands().ShowAvailableCards(null);
            _player1_UI.ChangeImageTrashBox(_online_trash_box);

            if (player.GetHands().IsCanDiscardCard())
            {
                player.GetHands().discardSelectedCards();
                player.GetHands().arrangeCards(player.GetCardPos(), player.GetCardScale());
                player.GetHands().SelectedPlayable(true);
                _online_trash_box = false;
                _player1_UI.ChangeImageTrashBox(_online_trash_box);
            }

            return;
        }
    }

    //CPUが操作
    void CPUMoving(Player player, Player enemy)
    {
        _cpu_incapacity_time += Time.deltaTime;
        if (_cpu_incapacity_time < CPU_THINGKING_TIME)
        {
            return;
        }
        if (!_cpu_draw)
        {

            player.GetHands().ResetHandCards(RESET_CARDS_COUNT, player.GetDeck(), player.GetCardPos(), player.GetCardScale());
            _cpu_draw = true;
            ResetCPUIncapacityTime();
        }

        if (_cpu_incapacity_time < CPU_THINGKING_TIME)
        {
            return;
        }
        if (player.GetHands().CheckMostExpensiveCardYouCanPay(player) != null)
        {

            int previous_hp = enemy.GetHP();
            Card played_card = player.GetHands().CheckMostExpensiveCardYouCanPay(player);
            played_card.Effect(player, enemy, player.GetCardPos(), player.GetCardScale(), _turn_situation);
            PlayingLogger.LogStatic(player.GetName() + "が効果を発動 : " + played_card.GetEffectByCondition(player) + "(" + played_card.GetAmountByCondition(player, _turn_situation.GetSituation()) + ")", Color.red);
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

        if (player.GetHands().CheckMostExpensiveCardYouCanPay(player) == null)
        {
            Debug.Log("Non-Card");
            checkResult();
            TurnChange(enemy);
            ResetCPUIncapacityTime();
            return;//カードの枚数がコストを下回っていたら使えない
        }

        Debug.LogError("CPUが想定していない動きをしています");
    }

    //CPUの稼働時間をリセットする
    void ResetCPUIncapacityTime() => _cpu_incapacity_time = 0;

    //ターンを終了して相手に行動を移す
    void TurnChange(Player turn)
    {
        turn.SetCurrentPlayer(true);
        turn.GetHands().SelectedPlayable(true);
        turn.GetHands().arrangeCards(turn.GetCardPos(), turn.GetCardScale());

        Hostile(turn).GetHands().ReleaseDragState();
        Hostile(turn).GetHands().discardSelectedCards();
        Hostile(turn).SetCurrentPlayer(false);
        Hostile(turn).GetHands().SelectedPlayable(false);
        Hostile(turn).GetHands().arrangeCards(Hostile(turn).GetCardPos(), Hostile(turn).GetCardScale());

        ResetProgressTime();
        _cpu_draw = false;
        _turn_change_animation = true;
        _turn_player = turn;

        if (turn.GetDeck().GetDeckCount() >= 0)
        {
            turn.GetHands().ResetHandCards(RESET_CARDS_COUNT, turn.GetDeck(), turn.GetCardPos(), turn.GetCardScale());
            turn.GetHands().SelectedPlayable(true);
        }
    }


    //プレイヤー1ならプレイヤー2に、プレイヤー2ならプレイヤー1を返す
    Player Hostile(Player my)
    {
        if (my == _player1)
        {
            return _player2;
        }

        return _player1;
    }

    //引き分けかどちらかが勝っているかを判定する
    void checkResult()
    {
        if (_player1.GetHP() <= 0 && _player2.GetHP() <= 0)
        {
            Debug.Log("引き分け");
            _result_Text.text = "引き分け";
            _conclusion = true;
            _player1.SetWinningState();
            _player2.SetWinningState();
            return;
        }

        if (_player1.GetHP() <= 0)
        {
            Debug.Log(_player1.GetName() + "の負け");
            _result_Text.text = _player1.GetName() + "の負け";
            _conclusion = true;
            _player2.SetWinningState();
            return;
        }

        if (_player2.GetHP() <= 0)
        {
            Debug.Log(_player2.GetName() + "の負け");
            _result_Text.text = _player2.GetName() + "の負け";
            _conclusion = true;
            _player1.SetWinningState();
            return;
        }
    }

    //ResultSceneに勝利状況を渡す
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject result_data = GameObject.Find("ResultDataController");
        if (result_data != null)
        {
            result_data.GetComponent<ResultDataController>().SetResultData(_player1.GetWinning(), _player2.GetWinning());
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //デバック用//強制的に勝利する
    void DebugWin()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.Space))
        {
            _player2.SetHP(0);
            checkResult();
        }
    }

    //デバック用//強制的に敗北する
    void DebugLose()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.Space))
        {
            _player1.SetHP(0);
            checkResult();
        }
    }

    //ランダムでターンプレイヤーを決める
    void RandomolyChooseTurnPlayer()
    {
        int random_player = Random.Range(0, 2);

        if (random_player == 0)
        {
            TurnChange(_player1);
        }
        else
        {
            TurnChange(_player2);
        }
    }

    //時間切れの際に相手にターンを回す
    void TimeRunsAndTheTurnEnd()
    {
        if (TIME_UNTIL_TIME_RUNS_OUT <= TURN_CHANGE_TIME || TIME_UNTIL_TIME_RUNS_OUT <= SCENE_CHANGE_TIME)
        {
            Debug.LogWarning("時間切れまでの時間が短すぎます");
            return;
        }

        if (_progress_time >= TIME_UNTIL_TIME_RUNS_OUT)
        {
            TurnChange(Hostile(_turn_player));
        }
    }

    //どちらかのHPが半分になった時BGMをかける
    void ChangeBGM()
    {
        if (GameObject.Find("BGMManager") != null)
        {
            if (_player1.GetHP() <= _player1.GetMaxHP() / 2 || _player2.GetHP() <= _player2.GetMaxHP() / 2)
            {
                GameObject.Find("BGMManager").GetComponent<BGMManager>().ChangeFightBGM(_final_fight_bgm);
            }
        }
    }
}
