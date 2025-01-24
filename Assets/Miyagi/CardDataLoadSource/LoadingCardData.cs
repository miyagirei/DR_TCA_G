using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingCardData : MonoBehaviour
{
    DataController _data_controller;
    void Start() {
        _data_controller = GameObject.Find("CardDataController").GetComponent<DataController>();
        _data_controller.LoadingCardData();
    }

    private void Update()
    {
        if (_data_controller.isCompleteSave()) {
            _data_controller.SaveCardDataList();
            Debug.Log("complete");
        }
    }
}
