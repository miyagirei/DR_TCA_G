using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultDataController : MonoBehaviour
{
    [SerializeField]Text _result_text;

    public void SetResultData(bool player1 , bool player2) {
        if (player1 && player2) {
            _result_text.text = "��������";
            return;
        } else if (player1 && !player2) {
            _result_text.text = "����";
            return;
        }else if (!player1 && player2) {
            _result_text.text = "�s�k";
            return;
        }
        _result_text.text = "�f�[�^���擾�ł��܂���ł���";
        return;
    }
}
