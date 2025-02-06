using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardListManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void backToHome()
    {
        SoundManager.PlaySoundStatic(SoundType.ReturnSound);
        SceneManager.LoadScene("HomeScene");
    }
}
