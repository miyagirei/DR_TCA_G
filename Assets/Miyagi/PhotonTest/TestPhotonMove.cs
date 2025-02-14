using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class TestPhotonMove : NetworkBehaviour
{
    [Networked] private NetworkTransform _network_transfrom { get; set; }
    void Start()
    {
        _network_transfrom = GetComponent<NetworkTransform>();
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            Vector3 new_position = transform.position + data.Direction;

            new_position.x = Mathf.Clamp(new_position.x, -8f , 8f);
            new_position.y = Mathf.Clamp(new_position.y, -5f , 5f);

            _network_transfrom.Teleport(new_position);

        }

        if (Runner.SessionInfo.PlayerCount >= 2) {
            Debug.Log("Max_Player");
            if (Runner.IsServer || Runner.IsSharedModeMasterClient) {
                Runner.LoadScene("PhotonFightScene");
            }

        }
    }
}
