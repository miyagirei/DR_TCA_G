using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]string _card_name;
    [SerializeField]int _damage_dealt;
    public Card(string name , int dmg) {
        _card_name = name;
        _damage_dealt = dmg;
    }

    public void init(string name , int dmg) {
        _card_name = name;
        _damage_dealt = dmg;
    }

    public string getName() {
        return _card_name;
    }    
    
    public void setName(string name) {
        _card_name = name;
    }    
    
    public int getDamage() {
        return _damage_dealt;
    }    
    
    public void setDamage(int dmg) {
        _damage_dealt = dmg;
    }
}
