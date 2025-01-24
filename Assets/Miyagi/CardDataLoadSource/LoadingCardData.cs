using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingCardData : MonoBehaviour
{
    DataController _card_data_controller;
    ParameterData _parameter_data;

    void Start() {
        _parameter_data = GameObject.Find("ParameterData").GetComponent<ParameterData>();
        _card_data_controller = GameObject.Find("CardDataController").GetComponent<DataController>();
        _parameter_data.LoadParameterData();
    }

    private void Update()
    {
        if (!_parameter_data.isLoaded()) {
            return;
        }
        if (!_parameter_data.isDetermine()) {
            return;
        }

        if (!_parameter_data.isSameVersion()) {
            _card_data_controller.LoadingCardData();
            _parameter_data.Saving();
        }

        if (_card_data_controller.isCompleteSave()) {
            _card_data_controller.SaveCardDataList();
            Debug.Log("complete");
            SceneManager.LoadScene("HomeScene");
        }

        if (_parameter_data.isSameVersion()) {
            Debug.Log("SameVersion");
            SceneManager.LoadScene("HomeScene");
        }
    }
}
