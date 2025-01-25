using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class ImageDownloader : MonoBehaviour
{
    [SerializeField]string _file_id = "";
    [SerializeField]string _folder_path = Path.Combine(Application.streamingAssetsPath, "Image");
    bool _is_finish = false;

    public bool IsFinish() => _is_finish;

    private void Start()
    {
        _file_id = null;
        if (!Directory.Exists(_folder_path)) {
            Directory.CreateDirectory(_folder_path);
        }

    }
    public void DownloadAndSave(string png_name , string file_id)
    {
        string file_name = png_name + ".png";
        string save_path = Path.Combine(_folder_path, file_name);

        if (png_name == null || file_id == null) {
            Debug.LogError("èåèÇñûÇΩÇµÇƒÇ¢Ç»Ç¢");
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
                _is_finish = true;
            }
            else {
                Debug.LogError($"Download failed: {request.error}");
            }
        }
    }
}
