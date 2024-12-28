using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultDataController : MonoBehaviour
{
    [SerializeField]Text _result_text;

    public void SetResultData(bool player1 , bool player2) {
        if (player1 && player2) {
            _result_text.text = "引き分け";
            return;
        } else if (player1 && !player2) {
            _result_text.text = "勝利";
            return;
        }else if (!player1 && player2) {
            _result_text.text = "敗北";
            return;
        }
        _result_text.text = "データを取得できませんでした";
        return;
    }
}
