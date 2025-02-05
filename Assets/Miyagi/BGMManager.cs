using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] string _start_BGM;
    void Start()
    {
        AudioClip bgm = Resources.Load<AudioClip>("Audio/" + _start_BGM);
        
        if(_start_BGM != null)
        {
            this.GetComponent<AudioSource>().clip = bgm;
            this.GetComponent<AudioSource>().Play();
            this.GetComponent<AudioSource>().loop = true;
            Debug.Log("BGM");
        }
    }

    void Update()
    {
        
    }
}
