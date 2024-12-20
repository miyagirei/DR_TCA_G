using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [HideInInspector]float CPU_THINGKING_TIME = 0;

    bool _cpu_draw = false;
    async void Start()
    {
        CPU_THINGKING_TIME = await _data_controller.GetParamValueFloat("CPU_THINGKING_TIME");
        _progress_time = 0;
        _player1 = new Player("player" , MAX_HP , true , _deck1 , _hands1);

        _player2 = new Player("enemy" , MAX_HP , false , _deck2 , _hands2);
    }

    void Update()
    {
        if (!_data_controller.isWaiting()) {
            return;
        }

        _progress_time += Time.deltaTime;
        _player1_UI.Display(_player1);
        _player2_UI.Display(_player2);
        if (_player1.IsCurrentPlayer()) {
            PlayerMoveing(_player1 , _player2) ;
        }
        if (_player2.IsCurrentPlayer()) {
            CPUMoving(_player2, _player1);
        }
    }

    void PlayerMoveing(Player player , Player enemy) {
        if (Input.GetMouseButtonDown(0))
        {
            if (player.GetHands().PickDrawDeck(player.GetDeck()) == null)
            {
                return;
            }
            player.GetHands().addHands(player , player.GetHands().PickDrawDeck(player.GetDeck()) , new Vector2(0,-4));
            player.GetHands().SelectedPlayable(true);
        }
        if (player.GetHands().IsPlayedCard() != null)//プレイできるカードがあるとき
        {
            if (player.GetHands().GetCardCount() <= player.GetHands().IsPlayedCard().GetCost()) {
                player.GetHands().IsPlayedCard().ReturnCard();
                return;//カードの枚数がコストを下回っていたら使えない
            }

            if (!player.GetHands().isCardSelectionValid(player.GetHands().IsPlayedCard().GetCost())) {
                player.GetHands().ShowAvailableCards();
                return;
            }

            int previous_hp = enemy.GetHP();
            enemy.AddHP(-player.GetHands().IsPlayedCard().GetDamage());
            player.GetHands().TrashCard(player.GetHands().IsPlayedCard());
            Debug.Log(previous_hp + " > " + enemy.GetHP());
            player.GetHands().discardSelectedCards();
            player.GetHands().arrangeCards(-4);
            TurnChange(enemy);
        }
    }

    void CPUMoving(Player player , Player enemy) {
        if (_progress_time < CPU_THINGKING_TIME)
        {
            return;
        }
        if (!_cpu_draw) {
            for(int i = 0; i < 4; i++) {
                player.GetHands().addHands(player, player.GetDeck(), new Vector2(0, 4));
            }
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
            return;//カードの枚数がコストを下回っていたら使えない
        }
    }

    void TurnChange(Player turn) {
        turn.SetCurrentPlayer(true);
        turn.GetHands().SelectedPlayable(true);//次のターンのプレイヤー

        Hostile(turn).SetCurrentPlayer(false);
        Hostile(turn).GetHands().SelectedPlayable(false);//終了したプレイヤー

        _turn_Text.text = turn.GetName() + " Turn";
        _progress_time = 0;
        checkResult();
    }

    Player Hostile(Player my) {
        if (my == _player1) {
            return _player2;
        }

        return _player1;
    }

    void checkResult() {
        if (_player1.GetHP() <= 0 && _player2.GetHP() <= 0) {
            Debug.Log("引き分け");
            _result_Text.text = "引き分け";
            return;
        }

        if (_player1.GetHP() <= 0) {
            Debug.Log(_player1.GetName() + "の負け");
            _result_Text.text = _player1.GetName() + "の負け";
            return;
        }        
        
        if (_player2.GetHP() <= 0) {
            Debug.Log(_player2.GetName() + "の負け");
            _result_Text.text = _player2.GetName() + "の負け";
            return;
        }
    }
}
