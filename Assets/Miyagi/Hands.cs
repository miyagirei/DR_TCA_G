using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    List<Card> _hands = new List<Card>();
    GameObject selectObj;

    void Start()
    {
        
    }

    void Update()
    {
        pickDrawDeck();
        check();
    }

    void pickDrawDeck() {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.origin);

            if (!hit2d)
            {
                return;
            }

            selectObj = hit2d.transform.gameObject;
            if (selectObj.TryGetComponent<Deck>(out Deck deck))
            {
                deck.drawDeck(_hands);
                Debug.Log(_hands.Count);
            }
        }
    }

    void check() {
        if (!Input.GetKeyDown(KeyCode.A)) {
            return;
        }

        for (int i = 0; i < _hands.Count; i++) {
            Debug.Log(_hands[i]._card_name + ":" + _hands[i]._damage_dealt);
        }
    }
}
