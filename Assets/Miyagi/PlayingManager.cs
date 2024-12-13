using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingManager : MonoBehaviour
{
    const int MAX_HP = 10;
    bool _is_playing = false;
    [SerializeField]Player _player1;
    [SerializeField]Deck _deck1;
    [SerializeField]Hands _hands1;
    
    [SerializeField]Player _player2;
    [SerializeField] Deck _deck2;
    [SerializeField] Hands _hands2;
    void Start()
    {
        _player1 = new Player("player" , MAX_HP , true , _deck1 , _hands1);
        _player2 = new Player("enemy" , MAX_HP , false , _deck2 , _hands2);
        Debug.Log(_player1.IsCurrentPlayer());
    }

    void Update()
    {
        if (_player1.IsCurrentPlayer()) {
            PlayerMoveing(_player1 , _player2) ;
        }
    }

    void PlayerMoveing(Player player , Player enemy) {
        if (player.GetHands().PickDrawDeck(player.GetDeck()) != null)
        {
            player.GetHands().addHands(player.GetHands().PickDrawDeck(player.GetDeck()));
        }

        if (player.GetHands().IsPlayedCard() != null)
        {
            int previous_hp = enemy.GetHP();
            enemy.AddHP(-player.GetHands().IsPlayedCard().GetDamage());
            player.GetHands().TrashCard(player.GetHands().IsPlayedCard());
            Debug.Log(previous_hp + " > " + enemy.GetHP());
            TurnChange(enemy);
        }
    }

    void TurnChange(Player turn) {
        turn.SetCurrentPlayer(true);
        Hostile(turn).SetCurrentPlayer(false);
    }

    Player Hostile(Player my) {
        if (my == _player1) {
            return _player2;
        }

        return _player1;
    }
}
