using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Net;

public class ImageDownloader : MonoBehaviour
{
    [SerializeField]string _file_id = "";
    [SerializeField]string _folder_path = Path.Combine(Application.streamingAssetsPath, "Image");
    bool _is_finish = false;
    bool _exist_data = false;
    bool _error = false;
    int _files_length = 0;
    string _download_info = "";

    public bool IsFinish() => _is_finish;
    public void resetFinish() => _is_finish = false;
    public bool IsImageData() => _exist_data;
    public bool getError() => _error;
    public int getFilesLength() => _files_length;
    public string getDownloadInfo() => _download_info;

    private void Start()
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        _file_id = null;
        _error = false;
        _exist_data = false;
        if (!Directory.Exists(_folder_path)) {
            Directory.CreateDirectory(_folder_path);
        }

        string[] files = Directory.GetFiles(_folder_path);

        int data_card_num = 0;
        string file_path = Path.Combine(Application.streamingAssetsPath, "card_data" + ".json");
        if (File.Exists(file_path))
        {
            string json = File.ReadAllText(file_path);

            CardListWrapper card_list_wrapper = JsonUtility.FromJson<CardListWrapper>(json);
            data_card_num = card_list_wrapper.cards.Count;
        }


        if (files.Length >= data_card_num)
        {
            _exist_data = true;
            _files_length = files.Length;
            Debug.Log("FilesLength : "+ files.Length);
        }
        else {
            _exist_data = false;
            _files_length = files.Length;
            Debug.Log("FilesLength : " + files.Length);
        }
    }
    private void Update()
    {
        string[] files = Directory.GetFiles(_folder_path);
        _files_length = files.Length;
    }

    public void DownloadAndSave(string png_name , string file_id)
    {
        string file_name = png_name + ".png";
        string image_path = Path.Combine(Application.streamingAssetsPath, "Image", file_name);
        if (File.Exists(image_path)) {
            _is_finish = true;
            Debug.Log("ë∂ç›ÇµÇƒÇ¢ÇÈÉtÉ@ÉCÉãÇ≈Ç∑");
            return;
        }

        string save_path = Path.Combine(_folder_path, file_name);

        if (png_name == null || file_id == null || png_name == "" || file_id == "") {
            Debug.LogError("èåèÇñûÇΩÇµÇƒÇ¢Ç»Ç¢");
            _error = true;
        }

        StartCoroutine(HandleRedirectAndDownload(file_id, save_path));
    }
    IEnumerator HandleRedirectAndDownload(string file_id, string save_path) {
        string url = "https://drive.google.com/uc?id=" + file_id + "&export=download";
        using (UnityWebRequest request = UnityWebRequest.Get(url)) {
            yield return request.SendWebRequest();
            if (request.responseCode == 200) {
                File.WriteAllBytes(save_path, request.downloadHandler.data);
                Debug.Log("response 200");
                _file_id = file_id;
                _is_finish = true;
                _download_info = "200 success " + file_id;
            }
            else if (request.responseCode == 302)
            {
                string confirm_url = request.GetResponseHeader("Location");

                if (confirm_url.Contains("&confirm=")) {
                    string token = confirm_url.Split(new string[] { "&confirm=" }, System.StringSplitOptions.None)[1].Split('&')[0];
                    string download_url = $"https://drive.google.com/uc?export=download&confirm={token}&id={file_id}";
                    Debug.Log("response 302");
                    yield return StartCoroutine(DownloadFile(download_url, save_path));
                }

            }
            else if (request.result == UnityWebRequest.Result.Success)
            {
                File.WriteAllBytes(save_path, request.downloadHandler.data);
                Debug.Log("Success: " + save_path);
                _file_id = file_id;
                _is_finish = true;
                _download_info = "normal_success" + file_id;
            }
            else {
                Debug.LogError($"Download failed: {request.error}");
                yield return StartCoroutine(DownloadFile(url, save_path));
            }

        }
    }

    IEnumerator DownloadFile(string url, string path) {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                File.WriteAllBytes(path, request.downloadHandler.data);
                Debug.Log("success : "+ path);
                _download_info = "late_normal_success";
                _is_finish = true;
            }
            else {
                Debug.LogError($"Download failed: {request.error}");
            }
        }
    }
}
