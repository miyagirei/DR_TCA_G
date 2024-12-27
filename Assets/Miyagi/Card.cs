using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    const string EFFECT_ATTACK_TEXT = "Attack";
    const string EFFECT_HEAL_TEXT = "Heal";
    const string EFFECT_DRAW_TEXT = "Draw";
    
    const float EFFECT_DISTANCE = 3;
    [SerializeField]GameObject _popup_prefab;
    GameObject _popup_instance;
    [SerializeField] bool _is_hovering = false;

    [Header("�X�e�[�^�X")]
    [SerializeField]string _card_name;//���O
    [SerializeField]int _damage_dealt;//�U����
    [SerializeField] int _healing_amount;//�񕜗�
    [SerializeField] int _draw_amount;
    [SerializeField]int _cost;//�R�X�g
    [SerializeField]string _effect;//����
    [SerializeField]Vector3 _return_pos;//�߂�Ƃ��̏ꏊ
    [SerializeField]bool _is_card_played = false;//�v���C����J�[�h���ǂ���
    [SerializeField]bool _is_dragging = false;//�h���b�O�����ǂ����̔���
    [SerializeField]bool _draggable = false;//�h���b�O���ł��邩�ǂ���
    [SerializeField]bool _can_selected = false;//�I���ł��邩�ǂ����̔���
    [SerializeField]bool _is_discarded = false;//�̂Ă邩�ǂ����̔���

    public void Init(string name , int amount , int cost , string effect) {
        _card_name = name;
        switch (effect)
        {
            case EFFECT_ATTACK_TEXT:
                _damage_dealt = amount;
                break;
            case EFFECT_HEAL_TEXT:
                _healing_amount = amount;
                break;
            case EFFECT_DRAW_TEXT:
                _draw_amount = amount;
                break;
        }
        _cost = cost;
        _effect = effect;
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
    public string GetEffect() => _effect;
    public void SetEffect(string effect) => _effect = effect;
    public int GetHealAmount() => _healing_amount;
    public void SetHealAmount(int amount) => _healing_amount = amount;
    public int GetDrawAmount() => _draw_amount;
    public void SetDrawAmount(int amount) => _draw_amount = amount; 

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

    //�h���b�O�J�n
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

    //�h���b�O��
    void Drag() {
        this.transform.position = GetMouseWorldPosition();
    }

    //�h���b�O�I��
    void EndDragging() {
        if(Vector3.Distance(this.transform.position , _return_pos) >= EFFECT_DISTANCE)
        {
            SetPlayed(true);
            _is_dragging = false;
            return;
        }

        ReturnCard();
    }

    //�J�[�h�������ʒu�ɖ߂��A�v���C����Ă��Ȃ���Ԃɂ���
    public void ReturnCard() {
        ReturnPos();
        _is_dragging = false;
        SetPlayed(false);
    }

    //�|�b�v�A�b�v�̏��������A�J�[�h�̏���j��
    public void Trash() {
        if(_popup_instance != null) { 
            Destroy(_popup_instance);
        }
        Destroy(this.gameObject);
    }

    //�̂Ă�J�[�h��I������
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

    //�}�E�X���I�u�W�F�N�g���ɂ���Ƃ��|�b�v�A�b�v�̕\��
    private void OnMouseEnter()
    {
        _is_hovering = true;
        _popup_instance.SetActive(true);
    }

    //�|�b�v�A�b�v�\��
    void DisplayPopup() {
        if (_is_hovering) {
            _popup_instance.transform.position = Input.mousePosition;
        }
    }

    //�}�E�X���I�u�W�F�N�g�O�ɍs�����Ƃ��|�b�v�A�b�v�̔�\��
    private void OnMouseExit()
    {
        _is_hovering = false;
        _popup_instance.SetActive(false);
    }

    //�V�����|�b�v�A�b�v�̍쐬
    public void CreatePopup() {
        _popup_instance = Instantiate(_popup_prefab, FindObjectOfType<Canvas>().transform);
        _popup_instance.SetActive(false);
        Text popup_text = _popup_instance.GetComponentInChildren<Text>();
        int amount = 0;
        switch (_effect)
        {
            case EFFECT_ATTACK_TEXT:
                amount = GetDamage();
                break;
            case EFFECT_HEAL_TEXT:
                amount = GetHealAmount();
                break;
            case EFFECT_DRAW_TEXT:
                amount = GetDrawAmount();
                break;
        }

        popup_text.text = _card_name + "\neffect : " + _effect + "\ncost : " + _cost + "\namount : " + amount;
    }

    public void Effect(Player player , Player enemy , Vector3 location ) {
        switch (_effect) {
            case EFFECT_ATTACK_TEXT:
                enemy.AddHP(-GetDamage());
                break;
            case EFFECT_HEAL_TEXT:
                player.AddHP(GetHealAmount());
                break;
            case EFFECT_DRAW_TEXT:
                for (int i = 0; i < GetDrawAmount();i++) {
                    player.GetHands().CreateCard(player.GetDeck().DrawDeck() , location.y );
                }
                break;
        }
    }

    //�G�t�F�N�g�ɉ����ČĂяo�����l��ς���
    public int GetEffectAmount(string effect) {
        switch (effect) {
            case EFFECT_ATTACK_TEXT:
                return GetDamage();
            case EFFECT_HEAL_TEXT:
                return GetHealAmount();
            case EFFECT_DRAW_TEXT:
                return GetDrawAmount();
        }

        return 0;
    }

    //���ʂ𐔎��ŌĂяo��//�f�o�b�N�p
    public string GetEffect( int effect_number ) {
        switch (effect_number)
        {
            case 0:
                return EFFECT_ATTACK_TEXT;
            case 1:
                return EFFECT_HEAL_TEXT;
            case 2:
                return EFFECT_DRAW_TEXT;
        }

        return null;
    }
}
