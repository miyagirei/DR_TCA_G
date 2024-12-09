using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    List<Card> _hands = new List<Card>();
    [SerializeField]GameObject _card_prefab;


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
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.origin , 0.01f);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.green, 3, false);
            if (!hit2d.collider)
            {
                return;
            }

            Deck selectObj = hit2d.transform.gameObject.GetComponent<Deck>();
            if (selectObj == null) {
                return;
            }

            if (selectObj.TryGetComponent<Deck>(out Deck deck))
            {
                deck.drawDeck(_hands);
                displayCard();
                Debug.Log(deck.name);
                Debug.Log(hit2d.transform.gameObject.name);
            }

        }
    }

    void check() {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.origin);
            if (!hit2d)
            {
                return;
            }

            GameObject selectObj = hit2d.transform.gameObject;
            if (!selectObj.TryGetComponent<Card>(out Card card))
            {
                return;
            }

            Debug.Log(card.getName() + ":" + card.getDamage());

        }

        if (!Input.GetKeyDown(KeyCode.A)) {
            return;
        }

        for (int i = 0; i < _hands.Count; i++) {
            Debug.Log(_hands[i].getName() + ":" + _hands[i].getDamage());
        }
    }

    void displayCard() {
        GameObject obj = Instantiate(_card_prefab, transform);
        int new_card_count = _hands.Count - 1;

        obj.transform.position = new Vector3(new_card_count * 2, -4);
        obj.name = _hands[new_card_count].getName();
        obj.GetComponent<Card>().setName(_hands[new_card_count].getName());
        obj.GetComponent<Card>().setDamage(_hands[new_card_count].getDamage());
        obj.GetComponent<Card>().setPos(obj.transform.position);
    }

}
