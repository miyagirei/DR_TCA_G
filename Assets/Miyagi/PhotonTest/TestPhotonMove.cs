using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class TestPhotonMove : NetworkTransform
{

    void Start()
    {

    }
    public override void FixedUpdateNetwork()
    {
        Debug.Log("Fixed Update");
        if (GetInput(out NetworkInputData data))
        {

            this.transform.position += data.Direction;

        }
    }
}
