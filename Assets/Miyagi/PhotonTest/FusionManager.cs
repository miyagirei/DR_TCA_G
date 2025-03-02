using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class FusionManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkRunner _runner_prefab;
     NetworkRunner _runner;

    [SerializeField] NetworkPrefabRef _player_prefab;

    //private List<NetworkCardData> _spawn_deck = new List<NetworkCardData>();
    //[Networked, Capacity(40)] public NetworkLinkedList<NetworkCardData> _deck_list => default;
    //[Networked, Capacity(40)] public NetworkLinkedList<NetworkCardData> _opponent_list => default;

    //テスト用
    //List<NetworkCardData> _deck_list = new List<NetworkCardData>();
    //List<NetworkCardData> _opponent_list = new List<NetworkCardData>();

    //bool _player_1 = false;
    //bool _player_2 = false;

    //bool _set_deck = false;
    async void Start()
    {
        _runner = Instantiate(_runner_prefab);
        _runner.AddCallbacks(this);

        var result = await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = "TestSession",
            SceneManager = _runner.GetComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok) {
            Debug.Log("Photon Host Start");

            var random_value = UnityEngine.Random.insideUnitCircle * 5f;
            var spawn_position = new Vector3(random_value.x, random_value.y, 0f);

            _runner.Spawn(_player_prefab, spawn_position, Quaternion.identity, _runner.LocalPlayer);

            //CardLoader loader = new CardLoader();
            //_spawn_deck = loader.ConvertCardList(loader.LoadCardDeck("deck" + PlayerPrefs.GetInt("SelectedDeck", 0)));

            //if (_runner.SessionInfo.PlayerCount == 1)
            //{
            //    Debug.Log("SendPlayer1");
            //    SetPlayer1(_spawn_deck.ToArray());
            //}
            //else if (_runner.SessionInfo.PlayerCount == 2)
            //{
            //    Debug.Log("SendPlayer2");
            //    SetPlayer2(_spawn_deck.ToArray());
            //}
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.LogError("Photon Host Error");
        }
        
    }


    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("PlayerJoined : " + player.PlayerId);

        //if (!runner.IsServer) { return; }

        //var random_value = UnityEngine.Random.insideUnitCircle * 5f;
        //var spawn_position = new Vector3(random_value.x, random_value.y, 0f);

        //var avater = runner.Spawn(_player_prefab, spawn_position, Quaternion.identity, player);
        //runner.SetPlayerObject(player, avater);
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        //if (!runner.IsServer) { return; }

        //if (runner.TryGetPlayerObject(player, out var avatar))
        //{
        //    runner.Despawn(avatar);
        //}
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        data.Direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);

        input.Set(data);
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj , PlayerRef player) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player , ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player , ReliableKey key, float data) { }

    //[Rpc(RpcSources.InputAuthority, RpcTargets.All)]
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

    //[Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    //void RPC_SendPlayer2(NetworkCardData[] received_cards)
    //{
    //    if (_player_2)
    //    {
    //        Debug.Log("Updated_Player2");
    //        return;
    //    }
    //    Debug.Log("Update_Player2");
    //    _opponent_list.Clear();
    //    foreach (var network_card in received_cards)
    //    {
    //        _opponent_list.Add(network_card);
    //    }
    //    _player_2 = true;

    //}


    //void SetPlayer1(NetworkCardData[] received_cards)
    //{
    //    if (_player_1)
    //    {
    //        Debug.Log("Updated_Player1");
    //        return;
    //    }
    //    //_deck_list.Clear();

    //    Debug.Log("Update_Player1");
    //    foreach (var network_card in received_cards)
    //    {
    //        _deck_list.Add(network_card);

    //    }
    //    _player_1 = true;
    //}

    //void SetPlayer2(NetworkCardData[] received_cards)
    //{
    //    if (_player_2)
    //    {
    //        Debug.Log("Updated_Player2");
    //        return;
    //    }
    //    //_opponent_list.Clear();
    //    Debug.Log("Update_Player2");
    //    foreach (var network_card in received_cards)
    //    {
    //        _opponent_list.Add(network_card);
    //    }
    //    _player_2 = true;

    //}

    //public void SetMyDeck() {

    //    CardLoader loader = new CardLoader();
    //    _spawn_deck = loader.ConvertCardList(loader.LoadCardDeck("deck" + PlayerPrefs.GetInt("SelectedDeck", 0)));
        
    //    if (_runner.SessionInfo.PlayerCount == 1)
    //    {
    //        Debug.Log("SendPlayer1");
    //        SetPlayer1(_spawn_deck.ToArray());
    //    }
    //    else if (_runner.SessionInfo.PlayerCount == 2)
    //    {
    //        Debug.Log("SendPlayer2");
    //        SetPlayer2(_spawn_deck.ToArray());
    //    }

    //    _set_deck = true;
    //}
}

public struct NetworkInputData : INetworkInput
{
    public Vector3 Direction;   
}
