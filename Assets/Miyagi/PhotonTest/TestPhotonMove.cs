using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;

public class TestPhotonMove : NetworkBehaviour
{
    [Networked] private NetworkTransform _network_transfrom { get; set; }

    [Networked,OnChangedRender(nameof(ColorChanged))] Color _test_color { get; set; }

    [Networked, Capacity(40)] public NetworkLinkedList<NetworkCardData> _deck_list => default;
    //[Networked, Capacity(40)] private NetworkLinkedList<NetworkCardData> _opponent_list => default;

    public override void Spawned()
    {
        base.Spawned();
        _network_transfrom = GetComponent<NetworkTransform>();
        CardLoader loader = new CardLoader();
        
        SetDeck(loader.ConvertCardList(loader.LoadCardDeck("deck" + PlayerPrefs.GetInt("SelectedDeck", 0))).ToArray());

    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            Vector3 new_position = transform.position + data.Direction;

            new_position.x = Mathf.Clamp(new_position.x, -8.5f, 8.5f);
            new_position.y = Mathf.Clamp(new_position.y, -4.5f, 4.5f);

            _network_transfrom.transform.position = new_position;
        }

        if (HasStateAuthority && Input.GetKeyDown(KeyCode.E))
        {
            _test_color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            RPC_ChatDeck();
        }

        if (Runner.SessionInfo.PlayerCount >= 2)
        {
            CardLoader loader = new CardLoader();

            if (Runner.IsServer || Runner.IsSharedModeMasterClient)
            {
                //SceneDataManager.Instance.SetData("player_deck", ConvertListCardData(_deck_list));
                //SceneDataManager.Instance.SetData("opponent_deck", ConvertListCardData(_opponent_list));

                Runner.LoadScene("PhotonFightScene");
            }

        }

    }

    void ColorChanged()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = _test_color;
    }

    //[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    //void RPC_SendPlayer1(NetworkCardData[] received_cards)
    //{
    //    if (_player_1)
    //    {
    //        Debug.Log("Updated_Player1");
    //        return;
    //    }
    //    Debug.Log("Update_Player1");
    //    _deck_list.Clear();
    //    foreach (var network_card in received_cards)
    //    {
    //        _deck_list.Add(network_card);

    //    }
    //    _player_1 = true;
    //}    
   

    public PlayerRef GetOpponentPlayerRef()
    {
        foreach (PlayerRef player in Runner.ActivePlayers)
        {
            if (player != Object.InputAuthority)
            {
                return player;
            }
        }

        return PlayerRef.None;
    }

    List<CardData> ConvertListCardData(NetworkLinkedList<NetworkCardData> data)
    {
        List<CardData> list = new List<CardData>();
        foreach (NetworkCardData card in data)
        {
            CardData local = new CardData();
            local = local.ConvertNetworkCardData(card);
            list.Add(local);
        }

        return list;

    }

    public List<CardData> ConvertNetworkList(List<NetworkCardData> data)
    {
        List<CardData> list = new List<CardData>();
        foreach (NetworkCardData card in data)
        {
            CardData local = new CardData();
            local = local.ConvertNetworkCardData(card);
            list.Add(local);
        }

        return list;
    }

    void SetDeck(NetworkCardData[] received_cards)
    {
        _deck_list.Clear();
        foreach (var network_card in received_cards)
        {
            _deck_list.Add(network_card);

        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.Proxies)]
    void RPC_ChatDeck()
    {
        Debug.Log(_deck_list[0].card_id);
    }
}
