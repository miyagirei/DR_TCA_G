using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class DataController : MonoBehaviour
{
    [SerializeField]
    bool DebugWaitingText = true;

    bool waiting = false;

    [SerializeField]
    private string sheet_name = "testSub";

    private string apiUrl = "https://script.google.com/macros/s/AKfycbw_56ss0h8ivlSZ79cCv-tUsdesk10E0Oxsh2fbRWRjDjclytSG0yyJHoJRmVW8mSH-/exec";

    private Dictionary<string, float> paramDataF = new Dictionary<string, float>();
    private Dictionary<string, int> paramDataI = new Dictionary<string, int>();
    private Dictionary<string, string> paramDataS = new Dictionary<string, string>();

    private void Start()
    {
        waiting = false;
        StartCoroutine(GetDataFromSheet());
    }


    public IEnumerator GetSheetData(string sheetName, System.Action<List<Dictionary<string, string>>> onSuccess, System.Action<string> onError)
    {
        string url = $"{apiUrl}?sheetName={sheetName}";

        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            // 取得したデータを処理
            if (sheetName == "CardDataList")
            {
                ProcessCardData(www.downloadHandler.text);
            }
            else {
                ProcessSheetData(www.downloadHandler.text); 
            }
        }
    }

    void ProcessSheetData(string jsonData)
    {
        // JSONデータを解析
        JArray data = JArray.Parse(jsonData);

        foreach (JObject row in data)
        {
            string paramName = row["param"].ToString();
            string value = row["value"].ToString();
            //Debug.Log($"Param: {paramName}, Value:{value}");

            if (!paramDataF.ContainsKey(paramName) && float.TryParse(value, out float floatValue))
            {
                paramDataF.Add(paramName, floatValue);
            }

            if (!paramDataI.ContainsKey(paramName) && int.TryParse(value, out int intValue))
            {
                paramDataI.Add(paramName, intValue);
            }

            if (!paramDataS.ContainsKey(paramName))
            {
                paramDataS.Add(paramName, value);
                Debug.Log(paramName + ":" + value);
            }
        }
    }

    void ProcessCardData(string json_data) {
        // JSONデータを解析
        JArray data = JArray.Parse(json_data);

        foreach (JObject row in data)
        {
            string card = row["card"].ToString();
            string card_type = row["cardType"].ToString();
            string normal_damage = row["normal_damage"].ToString();
            string normal_heal_amount = row["normal_heal_amount"].ToString();
            string normal_draw_amount = row["normal_draw_amount"].ToString();
            string normal_effect = row["normal_effect"].ToString();
            string normal_cost = row["normal_cost"].ToString();
            string hope_effect = row["hope_effect"].ToString();
            string hope_cost = row["hope_cost"].ToString();
            string despair_effect = row["despair_effect"].ToString();
            string despair_cost = row["despair_cost"].ToString();
            string bonus_damage = row["bonus_damage"].ToString();
            string bonus_heal_amount = row["bonus_heal_amount"].ToString();
            string bonus_draw_amount = row["bonus_draw_amount"].ToString();
            string image = row["image"].ToString();
            //Debug.Log($"Param: {paramName}, Value:{value}");

            /*
            if (!paramDataF.ContainsKey(paramName) && float.TryParse(value, out float floatValue))
            {
                paramDataF.Add(paramName, floatValue);
            }

            if (!paramDataI.ContainsKey(paramName) && int.TryParse(value, out int intValue))
            {
                paramDataI.Add(paramName, intValue);
            }

            if (!paramDataS.ContainsKey(paramName))
            {
                paramDataS.Add(paramName, value);
                Debug.Log(paramName + ":" + value);
            }
            */
        }
    }

    IEnumerator GetDataFromSheet()
    {

        yield return GetSheetData(
            sheet_name,
            (data) => {
                foreach (var row in data)
                {
                    Debug.Log($"Param: {row["param"]}, Value: {row["value"]}");
                }
            },
            (error) => {
                Debug.LogError("Failed to " + error);
            }
       );
    }

    public async Task<float> GetParamValueFloat(string paramName)
    {
        float wait_time = 1;
        int wait_count = 0;
        while (paramDataF.Count == 0)
        {
            wait_time += Time.deltaTime;
            if (wait_time >= 1)
            {
                if (DebugWaitingText) Debug.Log("loading " + paramName + " : waiting..." + wait_count + "float");
                wait_time = 0;
                wait_count++;
            }
            await Task.Yield();
        }

        waiting = true;
        if (paramDataF.TryGetValue(paramName, out float value))
        {
            return value;
        }
        else
        {
            Debug.LogWarning("not found : " + paramName);
            return 0f;
        }
    }

    public async Task<int> GetParamValueInt(string paramName)
    {
        float wait_time = 1;
        int wait_count = 0;
        while (paramDataI.Count == 0)
        {
            wait_time += Time.deltaTime;
            if (wait_time >= 1)
            {
                if (DebugWaitingText) Debug.Log("loading " + paramName + " : waiting..." + wait_count + "int");
                wait_time = 0;
                wait_count++;
            }
            await Task.Yield();
        }

        if (paramDataI.TryGetValue(paramName, out int value))
        {
            return value;
        }
        else
        {
            Debug.LogWarning("not found : " + paramName);
            return 0;
        }
    }

    public async Task<string> GetParamValueString(string paramName)
    {
        float wait_time = 1;
        int wait_count = 0;
        while (paramDataS.Count == 0)
        {
            wait_time += Time.deltaTime;
            if (wait_time >= 1)
            {
                if (DebugWaitingText) Debug.Log("loading " + paramName + " : waiting..." + wait_count + "string");
                wait_time = 0;
                wait_count++;
            }
            await Task.Yield();
        }

        if (paramDataS.TryGetValue(paramName, out string value))
        {
            return value;
        }
        else
        {
            Debug.LogWarning("not found : " + paramName);
            return null;
        }
    }

    public bool isWaiting() => waiting;
}
