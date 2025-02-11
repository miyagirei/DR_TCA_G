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

    async void Start()
    {
        _runner = Instantiate(_runner_prefab);

        _runner.AddCallbacks(this);
        var result = await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.AutoHostOrClient,
            SessionName = "TestSession",
            SceneManager = _runner.GetComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok) {
            Debug.Log("Photon Host Start");
        }
        else
        {
            Debug.LogError("Photon Host Error");
        }
        
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {
        Debug.Log("PlayerJoined : " + player);

        if (!runner.IsServer) { return; }

        var random_value = UnityEngine.Random.insideUnitCircle * 5f;
        var spawn_position = new Vector3(random_value.x, random_value.y, 0f);

        var avater = runner.Spawn(_player_prefab, spawn_position, Quaternion.identity, player);
        runner.SetPlayerObject(player, avater);
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {
        if (!runner.IsServer) { return; }

        if (runner.TryGetPlayerObject(player, out var avatar)) {
            runner.Despawn(avatar);
        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input) {
        Debug.Log("Input : " + input);
        var data = new NetworkInputData();

        data.Direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0f );

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
}

public struct NetworkInputData : INetworkInput
{
    public Vector3 Direction;   
}
