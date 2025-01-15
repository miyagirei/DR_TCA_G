using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    const string EFFECT_ATTACK_TEXT = "Attack";
    const string EFFECT_HEAL_TEXT = "Heal";
    const string EFFECT_DRAW_TEXT = "Draw";
    const string EFFECT_HOPE_CHANGE = "Hope";
    const string EFFECT_DESPAIR_CHANGE = "DESPAIR";

    DataController _data_controller;
    float EFFECT_DISTANCE = 100;
    [SerializeField] GameObject _popup_prefab;
    GameObject _popup_instance;
    [SerializeField] bool _is_hovering = false;

    [Header("ステータス")]
    [SerializeField] string _card_name;//名前
    [SerializeField] int _damage_dealt;//攻撃力
    [SerializeField] int _healing_amount;//回復量
    [SerializeField] int _draw_amount;//ドローする回数
    [SerializeField] int _cost;//コスト
    [SerializeField] int _cost_hope;//希望コスト
    [SerializeField] int _cost_despair;//絶望コスト
    [SerializeField] string _effect;//効果
    [SerializeField] string _effect_hope;//希望効果
    [SerializeField] string _effect_despair;//絶望効果
    [SerializeField] Vector3 _return_pos;//戻るときの場所
    [SerializeField] bool _is_card_played = false;//プレイするカードかどうか
    [SerializeField] bool _is_dragging = false;//ドラッグ中かどうかの判定
    [SerializeField] bool _draggable = false;//ドラッグができるかどうか
    [SerializeField] bool _can_selected = false;//選択できるかどうかの判定
    [SerializeField] bool _is_discarded = false;//捨てるかどうかの判定
    [SerializeField] bool _is_normal = true;//通常カードであるかの判定
    [SerializeField] CardType _card_type;

    public void Init(string name, int amount, int cost, string effect, CardType type)
    {
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
        _card_type = type;
        switch (_card_type)
        {
            case CardType.Normal:
                _cost = cost;
                _effect = effect;
                _is_normal = true;
                break;
            case CardType.OnlyHope:
                _cost_hope = cost;
                _effect_hope = effect;
                _is_normal = false;
                break;
            case CardType.OnlyDespair:
                _cost_despair = cost;
                _effect_despair = effect;
                _is_normal = false;
                break;
        }

        _return_pos = transform.position;
    }

    public void Init(string name, string hope, int hope_amount, int hope_cost, string despair, int despair_amount, int despair_cost)
    {
        _card_name = name;
        switch (hope)
        {
            case EFFECT_ATTACK_TEXT:
                _damage_dealt = hope_amount;
                break;
            case EFFECT_HEAL_TEXT:
                _healing_amount = hope_amount;
                break;
            case EFFECT_DRAW_TEXT:
                _draw_amount = hope_amount;
                break;
        }

        switch (despair)
        {
            case EFFECT_ATTACK_TEXT:
                _damage_dealt = despair_amount;
                break;
            case EFFECT_HEAL_TEXT:
                _healing_amount = despair_amount;
                break;
            case EFFECT_DRAW_TEXT:
                _draw_amount = despair_amount;
                break;
        }
        _cost_hope = hope_cost;
        _cost_despair = despair_cost;

        _effect_hope = hope;
        _effect_despair = despair;

        _return_pos = transform.position;
        _is_normal = false;
        _card_type = CardType.HopeAndDespair;
    }

    async void Start()
    {
        _data_controller = GameObject.Find("DataController").GetComponent<DataController>();
        EFFECT_DISTANCE = await _data_controller.GetParamValueFloat("CARD_EFFECT_DISTANCE");
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
    public int GetCost() => _cost;//通常カード時のコストを取得
    public void SetCost(int cost) => _cost = cost;//通常カード時のコストを設定する
    public int GetCostOfHope() => _cost_hope;//希望カード時のコストを取得する
    public int GetCostOfDespair() => _cost_despair;//絶望カード時のコストを取得する
    public bool GetCanSelected() => _can_selected;
    public void SetCanSelected(bool can) => _can_selected = can;
    public bool GetDiscard() => _is_discarded;
    public void SetDiscarded(bool discard) => _is_discarded = discard;
    public string GetEffect() => _effect;
    public string GetHopeEffect() => _effect_hope;
    public string GetDespairEffect() => _effect_despair;
    public void SetEffect(string effect) => _effect = effect;
    public int GetHealAmount() => _healing_amount;
    public void SetHealAmount(int amount) => _healing_amount = amount;
    public int GetDrawAmount() => _draw_amount;
    public void SetDrawAmount(int amount) => _draw_amount = amount;
    public bool GetIfNormalCard() => _is_normal;
    public CardType GetCardType() => _card_type;
    public bool GetDragging() => _is_dragging;
    public void SetScale(Vector3 scale) =>this.transform.localScale = scale;

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

        if (!_draggable || _can_selected)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !_is_dragging)
        {
            StartDragging();
        }

        if (_is_dragging)
        {
            if (Input.GetMouseButton(0))
            {
                Drag();
            }

            if (Input.GetMouseButtonUp(0))
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
    void StartDragging()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, Vector2.zero, Mathf.Infinity);
        if (hit2d.collider == null)
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
    void Drag()
    {
        this.transform.position = GetMouseWorldPosition();
    }

    //ドラッグ終了
    void EndDragging()
    {
        if (this.transform.position.y > _return_pos.y + EFFECT_DISTANCE)
        {
            SetPlayed(true);
            _is_dragging = false;
            return;
        }

        ReturnCard();
    }

    //カードを初期位置に戻し、プレイされていない状態にする
    public void ReturnCard()
    {
        ReturnPos();
        _is_dragging = false;
        SetPlayed(false);
    }

    //ポップアップの情報を消し、カードの情報を破棄
    public void Trash()
    {
        if (_popup_instance != null)
        {
            Destroy(_popup_instance);
        }
        Destroy(this.gameObject);
    }

    //捨てるカードを選択する
    void TrashSelect()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (!_can_selected)
        {
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
    void DisplayPopup()
    {
        if (_is_hovering)
        {
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
    public void CreatePopup()
    {
        _popup_instance = Instantiate(_popup_prefab, FindObjectOfType<Canvas>().transform);
        _popup_instance.SetActive(false);
        Text popup_text = _popup_instance.GetComponentInChildren<Text>();

        int amount = 0;
        switch (GetCardType())
        {
            case CardType.Normal:
                amount = GetEffectAmount(_effect);
                popup_text.text = _card_name + "\neffect : " + _effect + "\ncost : " + _cost + "\namount : " + amount;
                break;
            case CardType.HopeAndDespair:
                int hope_amount = GetEffectAmount(_effect_hope);
                int despair_amount = GetEffectAmount(_effect_despair);

                popup_text.text = _card_name + "\nhope_effect : " + _effect_hope + "\nhope_cost : " + _cost_hope + "\nhope_amount : " + hope_amount
                    + "\ndespair_effect : " + _effect_despair + "\ndespair_cost : " + _cost_despair + "\ndespair_amount : " + despair_amount;
                break;
            case CardType.OnlyHope:
                amount = GetEffectAmount(_effect_hope);
                popup_text.text = _card_name + "\neffect : " + _effect_hope + "\ncost : " + _cost_hope + "\namount : " + amount;
                break; 
            case CardType.OnlyDespair:
                amount = GetEffectAmount(_effect_despair);
                popup_text.text = _card_name + "\neffect : " + _effect_despair + "\ncost : " + _cost_despair + "\namount : " + amount;
                break;
        }
    }

    //カードを出したときの効果
    public void Effect(Player player, Player enemy, Vector3 location , Vector3 scale)
    {
        if (GetIfNormalCard() && player.GetNormalCondition())
        {
            EffectList(_effect, player, enemy, location, scale);
        }
        else if (!GetIfNormalCard() && player.GetHopeCondition() && !player.GetDespairCondition())
        {
            Debug.Log("Hope Effect");
            EffectList(_effect_hope, player, enemy, location, scale);
        }
        else if (!GetIfNormalCard() && !player.GetHopeCondition() && player.GetDespairCondition())
        {
            Debug.Log("Despair Effect");
            EffectList(_effect_despair, player, enemy, location, scale);
        }
    }

    //エフェクトに応じて対応した数値を呼び出す
    public int GetEffectAmount(string effect)
    {
        switch (effect)
        {
            case EFFECT_ATTACK_TEXT:
                return GetDamage();
            case EFFECT_HEAL_TEXT:
                return GetHealAmount();
            case EFFECT_DRAW_TEXT:
                return GetDrawAmount();
            case EFFECT_HOPE_CHANGE:
                return 1;
            case EFFECT_DESPAIR_CHANGE:
                return 1;
        }

        return 0;
    }

    //効果を数字で呼び出す//デバック用
    public string GetEffectNumber(int effect_number)
    {
        switch (effect_number)
        {
            case 0:
                return EFFECT_ATTACK_TEXT;
            case 1:
                return EFFECT_HEAL_TEXT;
            case 2:
                return EFFECT_DRAW_TEXT;
            case 3:
                return EFFECT_HOPE_CHANGE;
            case 4:
                return EFFECT_DESPAIR_CHANGE;
        }

        return null;
    }

    void ActivateDraw(Player player, Vector3 location , Vector3 scale)
    {
        for (int i = 0; i < GetDrawAmount(); i++)
        {
            if (player.GetDeck().GetDeckCount() == 0)
            {
                break;
            }

            player.GetHands().CreateCard(player.GetDeck().DrawDeck(), location , scale);
        }
    }

    void EffectList(string effect_text, Player player, Player enemy, Vector3 location , Vector3 scale)
    {
        switch (effect_text)
        {
            case EFFECT_ATTACK_TEXT:
                enemy.AddHP(-GetDamage());
                break;
            case EFFECT_HEAL_TEXT:
                player.AddHP(GetHealAmount());
                break;
            case EFFECT_DRAW_TEXT:
                ActivateDraw(player, location, scale);
                break;
            case EFFECT_HOPE_CHANGE:
                player.SetHope();
                break;
            case EFFECT_DESPAIR_CHANGE:
                player.SetDespair();
                break;
        }
    }

    public int GetCostByCondition(Player player)
    {
        if (GetIfNormalCard())
        {
            return GetCost();
        }

        if (player.GetHopeCondition() && (GetCardType() == CardType.HopeAndDespair || GetCardType() == CardType.OnlyHope))
        {
            return GetCostOfHope();
        }

        if (player.GetDespairCondition() && (GetCardType() == CardType.HopeAndDespair || GetCardType() == CardType.OnlyDespair))
        {
            return GetCostOfDespair();
        }

        return 9999;
    }

    public int GetCostByCardType()
    {
        switch (GetCardType())
        {
            case CardType.Normal:
                return GetCost();
            case CardType.OnlyHope:
                return GetCostOfHope();
            case CardType.OnlyDespair:
                return GetCostOfDespair();
        }

        return 9999;
    }

    public string GetEffectByCardType()
    {
        switch (GetCardType())
        {
            case CardType.Normal:
                return GetEffect();
            case CardType.OnlyHope:
                return GetHopeEffect();
            case CardType.OnlyDespair:
                return GetDespairEffect();
        }

        return null;
    }

    public string GetEffectByCondition(Player player) {
        if (GetCardType() != CardType.Normal) {
            if (GetCardType() == CardType.HopeAndDespair) {
                if (player.GetHopeCondition()) {
                    return GetHopeEffect();
                }
                if (player.GetDespairCondition()) {
                    return GetDespairEffect();
                }
                return null;
            }

            if (GetCardType() == CardType.OnlyHope) {
                return GetHopeEffect();
            }
            
            if (GetCardType() == CardType.OnlyDespair) {
                return GetDespairEffect();
            }
        }

        
        if (GetIfNormalCard()) {
            return GetEffect();
        }

        return null;
    }
}
