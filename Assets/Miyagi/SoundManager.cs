using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    const string SE_DECISION = "SE_Decision";
    const string SE_RETURN = "SE_Return";

    private static SoundManager instance;
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public static void PlaySoundStatic(SoundType sound)
    {
        if (instance != null)
        {
            instance.PlaySound(sound);
        }
    }

    public void PlaySound(SoundType sound)
    {
        string source_file = "";
        switch (sound)
        {
            case SoundType.DecisionSound:
                source_file = SE_DECISION;
                break;
            case SoundType.ReturnSound:
                source_file = SE_RETURN;
                break;
            default:
                return;
        }


        AudioClip sound_clip = Resources.Load<AudioClip>("Audio/" + source_file);
        this.GetComponent<AudioSource>().clip = sound_clip;
        this.GetComponent<AudioSource>().Play();
    }
}

public enum SoundType
{
    DecisionSound,
    ReturnSound
}
