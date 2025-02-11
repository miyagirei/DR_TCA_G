using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class TestPhotonMove : NetworkBehaviour
{

    void Start()
    {

    }
    public override void FixedUpdateNetwork()
    {
        Debug.Log("Fixed Update");
        if (GetInput(out NetworkInputData data))
        {

            data.Direction.Normalize();

            this.transform.position += data.Direction;

        }
    }
}
