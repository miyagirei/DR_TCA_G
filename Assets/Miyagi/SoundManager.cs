using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    const string SE_DECISION = "SE_Decision";
    const string SE_RETURN = "SE_Return";
    const string SE_HEAL = "SE_Heal";
    const string SE_ATTACK = "SE_Attack";
    const string SE_HOPEFUL = "SE_Hopeful";
    const string SE_HOPE = "SE_Hope";
    const string SE_DESPAIR = "SE_Despair";
    const string SE_DRAW = "SE_Draw";
    const string SE_CALM_DOWN = "SE_CalmDown";
    const string SE_TRASH = "SE_Trash";

    private static SoundManager instance;
    [SerializeField] bool _test_sound = false;
    [SerializeField] SoundType _test_type;
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (_test_sound)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                PlaySound(_test_type);
            }
        }
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
                this.GetComponent<AudioSource>().pitch = 1f;
                source_file = SE_DECISION;
                break;
            case SoundType.ReturnSound:
                this.GetComponent<AudioSource>().pitch = 1f;
                source_file = SE_RETURN;
                break;
            case SoundType.HealSound:
                this.GetComponent<AudioSource>().pitch = 1f;
                source_file = SE_HEAL;
                break;
            case SoundType.AttackSound:
                this.GetComponent<AudioSource>().pitch = 1f;
                source_file = SE_ATTACK;
                break;
            case SoundType.HopefulSound:
                this.GetComponent<AudioSource>().pitch = 1f;
                source_file = SE_HOPEFUL;
                break;
            case SoundType.DesperateSound:
                this.GetComponent<AudioSource>().pitch = 0.7f;
                source_file = SE_HOPEFUL;
                break;
            case SoundType.HopeSound:
                this.GetComponent<AudioSource>().pitch = 1f;
                source_file = SE_HOPE;
                break;
            case SoundType.DespairSound:
                this.GetComponent<AudioSource>().pitch = 1f;
                source_file = SE_DESPAIR;
                break;
            case SoundType.DrawSound:
                this.GetComponent<AudioSource>().pitch = 1f;
                source_file = SE_DRAW;
                break;
            case SoundType.CalmDownSound:
                this.GetComponent<AudioSource>().pitch = 1f;
                source_file = SE_CALM_DOWN;
                break;
            case SoundType.TrashSound:
                this.GetComponent<AudioSource>().pitch = 1f;
                source_file = SE_TRASH;
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
    ReturnSound,
    HealSound,
    AttackSound,
    HopefulSound,
    DesperateSound,
    HopeSound,
    DespairSound,
    DrawSound,
    CalmDownSound,
    TrashSound
}
