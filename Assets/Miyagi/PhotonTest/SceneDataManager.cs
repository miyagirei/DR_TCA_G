using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDataManager : MonoBehaviour
{
    public static SceneDataManager Instance { get; private set; }

    public Dictionary<string, object> Data { get; private set; } = new Dictionary<string, object>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //else {
        //    Destroy(gameObject);
        //}
    }

    public void SetData(string key, object value) {
        Data[key] = value;
    }

    public object GetData(string key) {
        return Data.ContainsKey(key) ? Data[key] : null;
    }
}
