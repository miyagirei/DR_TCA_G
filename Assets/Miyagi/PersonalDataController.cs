using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PersonalDataController : MonoBehaviour
{
    [SerializeField]int RESOLUTION = 0;
    private void Start()
    {
        string file_path = Application.persistentDataPath + "/personal_data" + ".json";

        if (!File.Exists(file_path)) {
            PersonalData personal_data = new PersonalData();

            personal_data.AUDIO_MASTER = 1f;
            personal_data.AUDIO_BGM = 1f;
            personal_data.AUDIO_SE = 1f;
            personal_data.RESOLUTION = 1080;
            Save(personal_data);
        }
    }

    public PersonalData Load()
    {
        string file_path = Application.persistentDataPath + "/personal_data" + ".json";

        if (File.Exists(file_path))
        {
            string json = File.ReadAllText(file_path);

            PersonalData personal_data = JsonUtility.FromJson<PersonalData>(json);
            return personal_data;
        }
        return null;
    }

    public void Save(PersonalData data) {

        string json = JsonUtility.ToJson(data, true);

        string filePath = Application.persistentDataPath + "/personal_data" + ".json";

        File.WriteAllText(filePath, json);
        Debug.Log("personaldata_test");
    }
}

[System.Serializable]
public class PersonalData 
{
    public float AUDIO_MASTER = 0f;
    public float AUDIO_BGM = 0f;
    public float AUDIO_SE = 0f;
    public int RESOLUTION = 0;
    public CharacterType CHARACTER_TYPE = CharacterType.Monokuma; 
}