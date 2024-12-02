using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public string _card_name;
    public int _damage_dealt;
    public Card(string name , int dmg) {
        _card_name = name;
        _damage_dealt = dmg;
    }
}
