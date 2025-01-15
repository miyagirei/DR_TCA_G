using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultDataController : MonoBehaviour
{
    [SerializeField]Text _result_text;

    public void SetResultData(bool player1 , bool player2) {
        if (player1 && player2) {
            _result_text.text = "ˆø‚«•ª‚¯";
            return;
        } else if (player1 && !player2) {
            _result_text.text = "Ÿ—˜";
            return;
        }else if (!player1 && player2) {
            _result_text.text = "”s–k";
            return;
        }
        _result_text.text = "ƒf[ƒ^‚ğæ“¾‚Å‚«‚Ü‚¹‚ñ‚Å‚µ‚½";
        return;
    }
}
