using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    void Start() {
        _parameter_data = GameObject.Find("ParameterData").GetComponent<ParameterData>();
        _card_data_controller = GameObject.Find("CardDataController").GetComponent<DataController>();
        _image_downloader = GameObject.Find("ImageDownloader").GetComponent<ImageDownloader>();
        _card_loader = this.gameObject.GetComponent<CardLoader>();
        
        _null_check = true;
        _is_image_downloading = false;
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
            _null_check = false;
        }

        if (_card_data_controller.isCompleteSave() ) {
            _card_data_controller.SaveCardDataList();
            EnqueueDownloads();
        
        }

        if(!_is_image_downloading && _download_queue.Count > 0)
        {
            StartCoroutine(ProcessDownloadQueue());
        }

        if (_image_downloader.IsFinish() && _download_queue.Count == 0 && _download_count <= 0) {
            SceneManager.LoadScene("HomeScene");
        }

        if (_parameter_data.isSameVersion() && _null_check) {
            Debug.Log("SameVersion");
            SceneManager.LoadScene("HomeScene");
        }
    }

    void EnqueueDownloads() {
        _download_count = _card_loader.GetNetworkCardData("card_data").Count;
        Debug.Log("download:" + _download_count);
        foreach (CardData card_data in _card_loader.GetNetworkCardData("card_data"))
        {
            _download_queue.Enqueue(card_data);
        }
    }

    IEnumerator ProcessDownloadQueue() {
        _is_image_downloading = true;
        while (_download_queue.Count > 0) {
            CardData current_card = _download_queue.Dequeue();
            _image_downloader.DownloadAndSave(current_card.card_name, current_card.image);
            while (!_image_downloader.IsFinish()) {
                yield return null;
            }
        }

        ReduceDownloadCount();
        Debug.Log(_download_count);
        _is_image_downloading = false;
    }

    public void ReduceDownloadCount() => _download_count--;
}
