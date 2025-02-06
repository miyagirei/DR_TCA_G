using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    [SerializeField] string _start_BGM;
    [SerializeField] string _main_BGM;
    [SerializeField] string _fight_BGM;
    [SerializeField] string _result_BGM;

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
        
        if (SceneManager.GetActiveScene().name == "GameFightingScene") {

            if(_now_bgm != _fight_BGM)
            {
                _now_bgm = _fight_BGM;
                AudioClip bgm = Resources.Load<AudioClip>("Audio/" + _fight_BGM);
                this.GetComponent<AudioSource>().clip = bgm;
                this.GetComponent<AudioSource>().Play();
            }
        }        
        
        if (SceneManager.GetActiveScene().name == "ResultScene") {

            if(_now_bgm != _result_BGM)
            {
                _now_bgm = _result_BGM;
                AudioClip bgm = Resources.Load<AudioClip>("Audio/" + _result_BGM);
                this.GetComponent<AudioSource>().clip = bgm;
                this.GetComponent<AudioSource>().Play();
            }
        }
    }

    public void ChangeFightBGM(string bgm) => _fight_BGM = bgm;
}
