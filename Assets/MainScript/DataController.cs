using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

public class DataController : MonoBehaviour
{
    [SerializeField]
    bool DebugWaitingText = true;

    bool _waiting = false;
    bool _complete_save_card_list = false;

    [SerializeField]
    private string sheet_name = "testSub";

    private string apiUrl = "https://script.google.com/macros/s/AKfycbwFK5K8om7HSiX8fQ_QXE9XOx2piojxZY4_9nn4CtfJIr3ezwx_OG1k9lvJlvR4NJb7/exec";

    private Dictionary<string, float> paramDataF = new Dictionary<string, float>();
    private Dictionary<string, int> paramDataI = new Dictionary<string, int>();
    private Dictionary<string, string> paramDataS = new Dictionary<string, string>();
    private Dictionary<string, CardData> paramDataCard = new Dictionary<string, CardData>();

    private void Start()
    {
        _waiting = false;
        _complete_save_card_list = false;
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
                Debug.Log(www.downloadHandler.text + ":card_data");
                ProcessCardData(www.downloadHandler.text);
            }
            else
            {
                Debug.Log(www.downloadHandler.text + ":other_data");
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

    void ProcessCardData(string json_data)
    {
        // JSONデータを解析
        JArray data = JArray.Parse(json_data);
        foreach (JObject row in data)
        {
            CardData card_data = new CardData();
            card_data.cardName = row["card"].ToString();
            card_data.type = row["card_type"].ToString();
            card_data.effect = row["normal_effect"].ToString();
            card_data.amount = int.Parse(row["normal_amount"].ToString());
            card_data.cost = int.Parse(row["normal_cost"].ToString());
            card_data.effectHope = row["hope_effect"].ToString();
            card_data.amountHope = int.Parse(row["hope_amount"].ToString());
            card_data.costHope = int.Parse(row["hope_cost"].ToString());
            card_data.amount_bonus_hope = int.Parse(row["hope_bonus_amount"].ToString());
            card_data.effectDespair = row["despair_effect"].ToString();
            card_data.amountDespair = int.Parse(row["despair_amount"].ToString());
            card_data.costHope = int.Parse(row["despair_cost"].ToString());
            card_data.amount_bonus_despair = int.Parse(row["despair_bonus_amount"].ToString());
            string image = row["image"].ToString();//画像データまだ読み込めません

            paramDataCard.Add(card_data.cardName, card_data);
        }
    }

    IEnumerator GetDataFromSheet()
    {

        yield return GetSheetData(
            sheet_name,
            (data) =>
            {
                foreach (var row in data)
                {
                    Debug.Log($"Param: {row["param"]}, Value: {row["value"]}");
                }
            },
            (error) =>
            {
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

        _waiting = true;
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

    public async void LoadingCardData()
    {
        float wait_time = 1;
        int wait_count = 0;
        while (paramDataCard.Count == 0)
        {
            wait_time += Time.deltaTime;
            if (wait_time >= 1)
            {
                if (DebugWaitingText) Debug.Log("loading " + "now " + wait_count);
                wait_time = 0;
                wait_count++;
            }
            await Task.Yield();
        }

        _complete_save_card_list = true;
    }

    public void SaveCardDataList()
    {
        CardListWrapper card_list_wrapper = ConvertDictionaryToWrapper(paramDataCard);
        string json = JsonUtility.ToJson(card_list_wrapper, true);
        string file_path = Path.Combine(Application.streamingAssetsPath, "card_data" + ".json");
        File.WriteAllText(file_path, json);
    }

    public bool isWaiting() => _waiting;
    public bool isCompleteSave() => _complete_save_card_list;

    CardListWrapper ConvertDictionaryToWrapper(Dictionary<string, CardData> dictionary) {
        var wrapper = new CardListWrapper
        {
            cards = new List<CardData>(dictionary.Values)
        };

        return wrapper;
    }
}