using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    const float EFFECT_DISTANCE = 3;

    [SerializeField]string _card_name;
    [SerializeField]int _damage_dealt;
    [SerializeField]Vector3 _return_pos;
    [SerializeField]bool _is_dragging = false;
    [SerializeField]bool _is_card_played = false;
    [SerializeField] Player _player;
    [SerializeField] bool _draggable = false;   

    public void Init(string name , int dmg ) {
        _card_name = name;
        _damage_dealt = dmg;
        _return_pos = transform.position;
    }

    public string GetName() => _card_name;
    public void SetName(string name) => _card_name = name;

    public int GetDamage() => _damage_dealt;
    public void SetDamage(int dmg) => _damage_dealt = dmg;

    public Vector2 GetPos() => _return_pos;
    public void SetPos(Vector3 pos) => _return_pos = pos;

    public bool GetPlayed() => _is_card_played;
    public void SetPlayed(bool played) => _is_card_played = played;
    public void SetDraggable(bool drag) => _draggable = drag;

    private void Update()
    {
        DragAndDrop();
    }

    void DragAndDrop()
    {
        if (!_draggable) {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !_is_dragging)
        {
            StartDragging();
        }

        if (_is_dragging) {
            if (Input.GetMouseButton(0) )
            {
                Drag();
            }

            if (Input.GetMouseButtonUp(0) )
            {
                EndDragging();
            }
        }
    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Mathf.Abs(Camera.main.transform.position.z);
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }

    void StartDragging() {
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

    void Drag() {
        this.transform.position = GetMouseWorldPosition();
    }

    void EndDragging() {
        if(Vector3.Distance(this.transform.position , _return_pos) >= EFFECT_DISTANCE)
        {
            SetPlayed(true);
            _is_dragging = false;
            return;
        }

        this.transform.position = _return_pos;
        _is_dragging = false;
        SetPlayed(false);
    }

    public void Trash() {
        Destroy(this.gameObject);
    }

}
