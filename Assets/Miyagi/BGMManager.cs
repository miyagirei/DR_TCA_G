using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    [SerializeField] string _start_BGM;
    [SerializeField] string _main_BGM;

    string _now_bgm;
    void Start()
    {
        DontDestroyOnLoad(this);
        AudioClip bgm = Resources.Load<AudioClip>("Audio/" + _start_BGM);
        _now_bgm = _start_BGM;

        if (_start_BGM != null)
        {
            this.GetComponent<AudioSource>().clip = bgm;
            this.GetComponent<AudioSource>().Play();
            this.GetComponent<AudioSource>().loop = true;
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "HomeScene") {
            if(_now_bgm != _main_BGM)
            {
                _now_bgm = _main_BGM;
                AudioClip bgm = Resources.Load<AudioClip>("Audio/" + _main_BGM);
                this.GetComponent<AudioSource>().clip = bgm;
                this.GetComponent<AudioSource>().Play();
            }
        }
    }
}
