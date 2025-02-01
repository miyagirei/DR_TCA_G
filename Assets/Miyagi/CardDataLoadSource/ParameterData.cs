using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ParameterData : MonoBehaviour
{
    DataController _data_controller;

    [HideInInspector] float CPU_THINGKING_TIME = 0;
    [HideInInspector] float SCENE_CHANGE_TIME = 0;
    [HideInInspector] float TURN_CHANGE_TIME = 0;
    [HideInInspector] float CARD_EFFECT_DISTANCE = 0;
    [HideInInspector] float TRASH_DECIDED_HEIGHT = 0;
    [HideInInspector] float TRASH_UNDECIDED_HEIGHT = 0;
    [HideInInspector] float TIME_UNTIL_TIME_RUNS_OUT = 0;
    [HideInInspector] float CARD_MOVING_SPEED = 2f;
    [HideInInspector] int RESET_CARD_COUNT = 0;
    [HideInInspector] float VERSION = 0.00f;
    [HideInInspector] int PLAYER_MAX_HP = 10;

    bool _same_version = false;
    bool _loaded = false;
    bool _determine = false;
    public bool isSameVersion() => _same_version;
    public bool isLoaded() => _loaded;
    public bool isDetermine() => _determine;
    public bool isError() => _data_controller.isDataError();

    void Start()
    {
        _same_version = false;
        _loaded = false;
        _determine = false;
    }

    public async void LoadParameterData() {
        _data_controller = GameObject.Find("DataController").GetComponent<DataController>();
        CPU_THINGKING_TIME = await _data_controller.GetParamValueFloat("CPU_THINGKING_TIME");
        SCENE_CHANGE_TIME = await _data_controller.GetParamValueFloat("SCENE_CHANGE_TIME");
        TURN_CHANGE_TIME = await _data_controller.GetParamValueFloat("TURN_CHANGE_TIME");
        CARD_EFFECT_DISTANCE = await _data_controller.GetParamValueFloat("CARD_EFFECT_DISTANCE");
        TRASH_DECIDED_HEIGHT = await _data_controller.GetParamValueFloat("TRASH_DECIDED_HEIGHT");
        TRASH_UNDECIDED_HEIGHT = await _data_controller.GetParamValueFloat("TRASH_UNDECIDED_HEIGHT");
        TIME_UNTIL_TIME_RUNS_OUT = await _data_controller.GetParamValueFloat("TIME_UNTIL_TIME_RUNS_OUT");
        CARD_MOVING_SPEED = await _data_controller.GetParamValueFloat("CARD_MOVING_SPEED");
        RESET_CARD_COUNT = await _data_controller.GetParamValueInt("RESET_CARDS_COUNT");
        VERSION = await _data_controller.GetParamValueFloat("VERSION");
        PLAYER_MAX_HP = await _data_controller.GetParamValueInt("PLAYER_MAX_HP");
        _loaded = true;
    }

    void Update()
    {
        if (!isLoaded()) {
            return;
        }

        if (!_data_controller.isWaiting()) {
            return;
        }

        if (GetParameterData("parameter_data") != null)
        {
            if (!isLoaded()) {
                return;
            }

            if (GetParameterData("parameter_data").VERSION != VERSION) {
                _same_version = false;
            } else if (GetParameterData("parameter_data").VERSION == VERSION) {
                _same_version = true;
            }
            _determine = true;
        }
        else {
            Saving();
        }
    }

    public Parameter GetParameterData(string json_file_name) {
        string file_path = Path.Combine(Application.persistentDataPath, json_file_name + ".json");

        if (File.Exists(file_path))
        {
            string json = File.ReadAllText(file_path);

            Parameter parameter = JsonUtility.FromJson<Parameter>(json);
            return parameter;
        }
        else
        {
            Debug.LogError("ÉtÉ@ÉCÉãÇ™ë∂ç›ÇµÇ‹ÇπÇÒ");
            return null;
        }
    }

    public void Saving() {
        SaveParameterData("parameter_data");
    }

    void SaveParameterData(string json_file_name) {
        Parameter parameter = new Parameter();
        parameter.CPU_THINGKING_TIME = CPU_THINGKING_TIME;
        parameter.SCENE_CHANGE_TIME = SCENE_CHANGE_TIME;
        parameter.TURN_CHANGE_TIME = TURN_CHANGE_TIME;
        parameter.CARD_EFFECT_DISTANCE = CARD_EFFECT_DISTANCE;
        parameter.TRASH_DECIDED_HEIGHT = TRASH_DECIDED_HEIGHT;
        parameter.TRASH_UNDECIDED_HEIGHT = TRASH_UNDECIDED_HEIGHT;
        parameter.TIME_UNTIL_TIME_RUNS_OUT = TIME_UNTIL_TIME_RUNS_OUT;
        parameter.CARD_MOVING_SPEED = CARD_MOVING_SPEED;
        parameter.RESET_CARD_COUNT = RESET_CARD_COUNT;
        parameter.VERSION = VERSION;
        parameter.PLAYER_MAX_HP = PLAYER_MAX_HP;


        string json = JsonUtility.ToJson(parameter, true);
        string file_path = Path.Combine(Application.persistentDataPath, json_file_name + ".json");
        File.WriteAllText(file_path, json);
    }
}
[System.Serializable]
public class Parameter
{
    public float CPU_THINGKING_TIME = 0;
    public float SCENE_CHANGE_TIME = 0;
    public float TURN_CHANGE_TIME = 0;
    public float CARD_EFFECT_DISTANCE = 0;
    public float TRASH_DECIDED_HEIGHT = 0;
    public float TRASH_UNDECIDED_HEIGHT = 0;
    public float TIME_UNTIL_TIME_RUNS_OUT = 0;
    public float CARD_MOVING_SPEED = 2f;
    public int RESET_CARD_COUNT = 0;
    public float VERSION = 0.01f;
    public int PLAYER_MAX_HP = 10;
}
