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

    [Header("ステータス")]
    [SerializeField]string _card_name;//名前
    [SerializeField]int _damage_dealt;//攻撃力
    [SerializeField] int _healing_amount;//回復量
    [SerializeField] int _draw_amount;
    [SerializeField]int _cost;//コスト
    [SerializeField]string _effect;//効果
    [SerializeField]Vector3 _return_pos;//戻るときの場所
    [SerializeField]bool _is_card_played = false;//プレイするカードかどうか
    [SerializeField]bool _is_dragging = false;//ドラッグ中かどうかの判定
    [SerializeField]bool _draggable = false;//ドラッグができるかどうか
    [SerializeField]bool _can_selected = false;//選択できるかどうかの判定
    [SerializeField]bool _is_discarded = false;//捨てるかどうかの判定

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
    /// ドラッグ&ドロップ
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

    //ドラッグ開始
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

    //ドラッグ中
    void Drag() {
        this.transform.position = GetMouseWorldPosition();
    }

    //ドラッグ終了
    void EndDragging() {
        if(Vector3.Distance(this.transform.position , _return_pos) >= EFFECT_DISTANCE)
        {
            SetPlayed(true);
            _is_dragging = false;
            return;
        }

        ReturnCard();
    }

    //カードを初期位置に戻し、プレイされていない状態にする
    public void ReturnCard() {
        ReturnPos();
        _is_dragging = false;
        SetPlayed(false);
    }

    //ポップアップの情報を消し、カードの情報を破棄
    public void Trash() {
        if(_popup_instance != null) { 
            Destroy(_popup_instance);
        }
        Destroy(this.gameObject);
    }

    //捨てるカードを選択する
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

    //マウスがオブジェクト内にあるときポップアップの表示
    private void OnMouseEnter()
    {
        _is_hovering = true;
        _popup_instance.SetActive(true);
    }

    //ポップアップ表示
    void DisplayPopup() {
        if (_is_hovering) {
            _popup_instance.transform.position = Input.mousePosition;
        }
    }

    //マウスがオブジェクト外に行ったときポップアップの非表示
    private void OnMouseExit()
    {
        _is_hovering = false;
        _popup_instance.SetActive(false);
    }

    //新しいポップアップの作成
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

    //エフェクトに応じて呼び出す数値を変える
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

    //効果を数字で呼び出す//デバック用
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
