using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]string _card_name;
    [SerializeField]int _damage_dealt;
    [SerializeField]Vector3 _return_pos;
    [SerializeField]bool _is_dragging = false;

    public void init(string name , int dmg) {
        _card_name = name;
        _damage_dealt = dmg;
        _return_pos = transform.position;
    }

    public string getName() => _card_name;
    public void setName(string name) => _card_name = name;

    public int getDamage() => _damage_dealt;
    public void setDamage(int dmg) => _damage_dealt = dmg;

    public Vector2 getPos() => _return_pos;
    public void setPos(Vector3 pos) => _return_pos = pos;

    private void Update()
    {
        dragAndDrop();
    }

    void dragAndDrop()
    {
        if (Input.GetMouseButtonDown(0) && !_is_dragging)
        {
            startDragging();
        }

        if (_is_dragging) {
            if (Input.GetMouseButton(0) )
            {
                drag();
            }

            if (Input.GetMouseButtonUp(0) )
            {
                endDragging();
            }
        }
    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Mathf.Abs(Camera.main.transform.position.z);
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }

    void startDragging() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, Vector2.zero , Mathf.Infinity);
            if (hit2d.collider == null )
            {
                return;
            }

            GameObject selectObj = hit2d.transform.gameObject;
            if (selectObj == this.gameObject)
            {
                _is_dragging = true;
            }
    }

    void drag() {
        this.transform.position = GetMouseWorldPosition();
    }

    void endDragging() {
        this.transform.position = _return_pos;
        _is_dragging = false;
    }
}
