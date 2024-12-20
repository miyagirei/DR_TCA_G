using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    const float EFFECT_DISTANCE = 3;
    [SerializeField]GameObject _popup_prefab;
    GameObject _popup_instance;
    [SerializeField] bool _is_hovering = false;

    [Header("�X�e�[�^�X")]
    [SerializeField]string _card_name;//���O
    [SerializeField]int _damage_dealt;//�U����
    [SerializeField]int _cost;//�R�X�g
    [SerializeField]Vector3 _return_pos;//�߂�Ƃ��̏ꏊ
    [SerializeField]bool _is_card_played = false;//�v���C����J�[�h���ǂ���
    [SerializeField]bool _is_dragging = false;//�h���b�O�����ǂ����̔���
    [SerializeField]bool _draggable = false;//�h���b�O���ł��邩�ǂ���
    [SerializeField]bool _can_selected = false;//�I���ł��邩�ǂ����̔���
    [SerializeField] bool _is_discarded = false;//�̂Ă邩�ǂ����̔���

    public void Init(string name , int dmg , int cost) {
        _card_name = name;
        _damage_dealt = dmg;
        _cost = cost;
        _return_pos = transform.position;
    }

    private void Start()
    {
        
    }

    public string GetName() => _card_name;
    public void SetName(string name) => _card_name = name;
    public int GetDamage() => _damage_dealt;
    public void SetDamage(int dmg) => _damage_dealt = dmg;
    public Vector2 GetPos() => _return_pos;
    public void SetPos(Vector3 pos) => _return_pos = pos;
    public void ReturnPos() => this.transform.position = _return_pos;
    public void temporarilySetPosition(Vector3 pos) => this.transform.position = pos;
    public bool GetPlayed() => _is_card_played;
    public void SetPlayed(bool played) => _is_card_played = played;
    public void SetDraggable(bool drag) => _draggable = drag;
    public int GetCost() => _cost;
    public void SetCost(int cost) => _cost = cost;
    public bool GetCanSelected() => _can_selected;
    public void SetCanSelected(bool can) => _can_selected = can;
    public bool GetDiscard() => _is_discarded;
    public void SetDiscarded(bool discard) => _is_discarded = discard;

    private void Update()
    {
        DragAndDrop();
        TrashSelect();
        DisplayPopup();
    }

    /// <summary>
    /// �h���b�O&�h���b�v
    /// </summary>
    void DragAndDrop()
    {
        if (!_draggable || _can_selected) {
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

        ReturnCard();
    }

    public void ReturnCard() {
        ReturnPos();
        _is_dragging = false;
        SetPlayed(false);
    }

    /// <summary>
    /// �J�[�h���̂Ă�
    /// </summary>
    public void Trash() {
        if(_popup_instance != null) { 
            Destroy(_popup_instance);
        }
        Destroy(this.gameObject);
    }

    void TrashSelect() {
        if (!Input.GetMouseButtonDown(0)){
            return;
        }

        if (!_can_selected) {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, Vector2.zero, Mathf.Infinity);
        if (hit2d.collider == null)
        {
            return;
        }

        GameObject selectObj = hit2d.transform.gameObject;
        if (selectObj != this.gameObject)
        {
            return;
        }

        _is_discarded = !_is_discarded;
    }

    /// <summary>
    /// �|�b�v�A�b�v
    /// </summary>
    private void OnMouseEnter()
    {
        _is_hovering = true;
        _popup_instance.SetActive(true);
    }

    void DisplayPopup() {
        if (_is_hovering) {
            _popup_instance.transform.position = Input.mousePosition;
        }
    }

    private void OnMouseExit()
    {
        _is_hovering = false;
        _popup_instance.SetActive(false);
    }

    public void CreatePopup() {
        _popup_instance = Instantiate(_popup_prefab, FindObjectOfType<Canvas>().transform);
        _popup_instance.SetActive(false);
        Text popup_text = _popup_instance.GetComponentInChildren<Text>();
        popup_text.text = _card_name + "\ndamage : " + _damage_dealt + "\ncost : " + _cost;
    }
}
