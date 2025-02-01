using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
public class TitleManager : MonoBehaviour
{
    void ClearImageFolder() {
        string image_folder_path = Path.Combine(Application.persistentDataPath, "Image");

        if (Directory.Exists(image_folder_path)) {
            string[] files = Directory.GetFiles(image_folder_path);

            foreach(string file in files) {
                File.Delete(file);
            }
            Debug.Log("�������������܂���");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown) {
            if (Input.GetKeyDown(KeyCode.Delete)) {
                Debug.Log("Image�t�@�C�����N���A���Ă��܂�");
                ClearImageFolder();
                return;
            }

            SceneManager.LoadScene("CardDataLoadScene");
        }
    }
}
