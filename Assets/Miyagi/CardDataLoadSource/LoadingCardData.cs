using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingCardData : MonoBehaviour
{
    DataController _card_data_controller;
    ParameterData _parameter_data;
    ImageDownloader _image_downloader;
    CardLoader _card_loader;
    bool _null_check = true;

    Queue<CardData> _download_queue = new Queue<CardData>();
    bool _is_image_downloading = false;
    int _download_count = 0;
    bool _is_download_start = false;

    [SerializeField] Text _download_count_text;

    void Start() {
        _parameter_data = GameObject.Find("ParameterData").GetComponent<ParameterData>();
        _card_data_controller = GameObject.Find("CardDataController").GetComponent<DataController>();
        _image_downloader = GameObject.Find("ImageDownloader").GetComponent<ImageDownloader>();
        _card_loader = this.gameObject.GetComponent<CardLoader>();
        
        _null_check = true;
        _is_image_downloading = false;
        _is_download_start = false;
        _parameter_data.LoadParameterData();
    }

    private void Update()
    {
        if (_parameter_data.isError()) {
            _download_count_text.text = "Error‚ª”­¶";
            SceneManager.LoadScene("TitleScene");
            return;
        }

        _download_count_text.text = "parameter : "+ _parameter_data.isLoaded() + 
            "\nversion_check : " + _parameter_data.isDetermine() +
            "\ndownload : " + _download_count + 
            "\nnow_import : " + _image_downloader.getFilesLength() + 
            "\nsame_version : " + _parameter_data.isSameVersion() + 
            "\nexist_full_image : " + _image_downloader.IsImageData() + 
            "\ndownload_now... : " + _is_image_downloading +
            "\nloading_info : " + _image_downloader.getDownloadInfo(); 

        if (!_parameter_data.isLoaded()) {
            return;
        }

        if (!_parameter_data.isDetermine())
        {
            return;
        }

        if (_image_downloader.getError()) {
            SceneManager.LoadScene("TitleScene");
            return;
        }

        if (!_parameter_data.isSameVersion() || !_image_downloader.IsImageData() || _is_image_downloading) {
            _card_data_controller.LoadingCardData();
            _parameter_data.Saving();
            _null_check = false;
        }

        if (_card_data_controller.isCompleteSave()) {
            _card_data_controller.SaveCardDataList();
            EnqueueDownloads();
        }

        if(!_is_image_downloading && _download_queue.Count > 0)
        {
            StartCoroutine(ProcessDownloadQueue());
            Debug.Log("coroutineTest");
        }

        if (_parameter_data.isSameVersion() && _null_check && _image_downloader.IsImageData() && !_is_image_downloading) {
            SceneManager.LoadScene("HomeScene");
        }

        if (_download_queue.Count == 0 && _download_count <= 0 && !_null_check) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void EnqueueDownloads() {
        if (!_is_download_start)
        {
            _download_count = _card_loader.GetNetworkCardData("card_data").Count;
            foreach (CardData card_data in _card_loader.GetNetworkCardData("card_data"))
            {
                _download_queue.Enqueue(card_data);
            }
            _is_download_start = true;
        }
        Debug.Log("download:" + _download_count);
    }

    public IEnumerator ProcessDownloadQueue() {
        _is_image_downloading = true;
        while (_download_queue.Count > 0) {
            CardData current_card = _download_queue.Peek();
            Debug.Log(current_card.card_name + ":"+ _download_count );
            _image_downloader.DownloadAndSave(current_card.card_name, current_card.image);
            while (!_image_downloader.IsFinish()) {
                Debug.Log("not_finish");
                yield return null;
            }
            _image_downloader.resetFinish();
            CardData load_card = _download_queue.Dequeue();
            ReduceDownloadCount();
            Debug.Log(_download_count + ":" + load_card.card_name);
        }

        if (_download_queue.Count > 0) {
            yield return null;
        }

        Debug.Log("download_complete");
        _is_image_downloading = false;
    }

    public void ReduceDownloadCount() => _download_count-=1;
}
