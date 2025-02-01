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
    const string EFFECT_DESPAIR_CHANGE = "Despair";
    const string EFFECT_TURN_CONDITION_HOPE = "Hopeful";
    const string EFFECT_TURN_CONDITION_DESPAIR = "Desperate";
    const string EFFECT_ENEMY_CONDITION_NORMAL = "CalmDown";
    const string EFFECT_ENEMY_CONDITION_HOPE = "GiveHope";
    const string EFFECT_ENEMY_CONDITION_DESPAIR = "GiveDespair";

    ParameterData _parameter_data_controller;
    [HideInInspector] float EFFECT_DISTANCE = 100;
    [SerializeField] float MOVING_SPEED = 2f;

    [SerializeField] GameObject _popup_prefab;
    GameObject _popup_instance;
    [SerializeField] bool _is_hovering = false;

    [Header("ステータス")]
    [SerializeField] string _card_name;//名前
    [SerializeField] CardType _card_type;//カードタイプ

    [SerializeField] string _normal_effect;//効果
    [SerializeField] int _normal_amount;//通常のカードの効果量
    [SerializeField] int _normal_cost;//コスト

    [SerializeField] string _effect_hope;//希望効果
    [SerializeField] int _hope_amount;//回復量//希望の効果量
    [SerializeField] int _cost_hope;//希望コスト
    [SerializeField] int _hope_bonus_amount;//希望一致ボーナス

    [SerializeField] string _effect_despair;//絶望効果
    [SerializeField] int _despair_amount;//ドローする回数//絶望の効果量
    [SerializeField] int _cost_despair;//絶望コスト
    [SerializeField] int _despair_bonus_amount;//絶望一致ボーナス

    [Header("デバック")]
    [SerializeField] Vector3 _return_pos;//戻るときの場所
    [SerializeField] bool _is_card_played = false;//プレイするカードかどうか
    [SerializeField] bool _is_dragging = false;//ドラッグ中かどうかの判定
    [SerializeField] bool _draggable = false;//ドラッグができるかどうか
    [SerializeField] bool _can_selected = false;//選択できるかどうかの判定
    [SerializeField] bool _is_discarded = false;//捨てるかどうかの判定
    [SerializeField] bool _is_normal = true;//通常カードであるかの判定
    [SerializeField] bool _is_moving = false;//アニメーション移動を起こすかどうかの判定

    public void Init(string name, CardType type, string effect, int amount, int cost, int bonus_amount = 0)
    {
        _card_name = name;
        _card_type = type;
        switch (_card_type)
        {
            case CardType.Normal:
                _normal_cost = cost;
                _normal_effect = effect;
                _normal_amount = amount;
                _is_normal = true;
                break;
            case CardType.OnlyHope:
                _cost_hope = cost;
                _effect_hope = effect;
                _hope_amount = amount;
                _hope_bonus_amount = bonus_amount;
                _is_normal = false;
                break;
            case CardType.OnlyDespair:
                _cost_despair = cost;
                _effect_despair = effect;
                _despair_amount = amount;
                _despair_bonus_amount = bonus_amount;
                _is_normal = false;
                break;
        }

        _return_pos = transform.position;
    }

    public void Init(string name, string hope, int hope_amount, int bonus_hope, int hope_cost, string despair, int despair_amount, int bonus_despair, int despair_cost)
    {
        _card_name = name;
        _effect_hope = hope;
        _cost_hope = hope_cost;
        _hope_amount = hope_amount;
        _hope_bonus_amount = bonus_hope;

        _effect_despair = despair;
        _cost_despair = despair_cost;
        _despair_amount = despair_amount;
        _despair_bonus_amount = bonus_despair;

        _return_pos = transform.position;
        _is_normal = false;
        _card_type = CardType.HopeAndDespair;
    }

    void Start()
    {
        _parameter_data_controller = GameObject.Find("ParameterDataController").GetComponent<ParameterData>();
        Parameter parameter_data = _parameter_data_controller.GetParameterData("parameter_data");

        EFFECT_DISTANCE = parameter_data.CARD_EFFECT_DISTANCE;
        MOVING_SPEED = parameter_data.CARD_MOVING_SPEED;
    }

    public string GetName() => _card_name;
    public void SetName(string name) => _card_name = name;
    public int GetNormalAmount() => _normal_amount;
    public int GetHopeAmount() => _hope_amount;
    public int GetHopeBonusAmount() => _hope_bonus_amount;
    public Vector2 GetPos() => _return_pos;
    public void SetPos(Vector3 pos) => _return_pos = pos;
    public void ReturnPos() => this.transform.position = _return_pos;
    public void temporarilySetPosition(Vector3 pos) => this.transform.position = pos;
    public bool GetPlayed() => _is_card_played;
    public void SetPlayed(bool played) => _is_card_played = played;
    public void SetDraggable(bool drag) => _draggable = drag;
    public int GetCost() => _normal_cost;//通常カード時のコストを取得
    public int GetCostOfHope() => _cost_hope;//希望カード時のコストを取得する
    public int GetCostOfDespair() => _cost_despair;//絶望カード時のコストを取得する
    public bool GetCanSelected() => _can_selected;
    public void SetCanSelected(bool can) => _can_selected = can;
    public bool GetDiscard() => _is_discarded;
    public void SetDiscarded(bool discard) => _is_discarded = discard;
    public string GetNormalEffect() => _normal_effect;
    public string GetHopeEffect() => _effect_hope;
    public string GetDespairEffect() => _effect_despair;
    public int GetDespairAmount() => _despair_amount;
    public int GetDespairBonusAmount() => _despair_bonus_amount;
    public bool GetIfNormalCard() => _is_normal;
    public CardType GetCardType() => _card_type;
    public bool GetDragging() => _is_dragging;
    public void SetScale(Vector3 scale) => this.transform.localScale = scale;
    public void SetMoving(bool moving) => _is_moving = moving;

    private void Update()
    {
        DragAndDrop();
        TrashSelect();
        DisplayPopup();
        Moving();
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

    public string GetEffectByCardType(CardType card_type, bool is_hope = true)
    {
        switch (card_type)
        {
            case CardType.Normal:
                return GetNormalEffect();
            case CardType.OnlyHope:
                return GetHopeEffect();
            case CardType.OnlyDespair:
                return GetDespairEffect();
            case CardType.HopeAndDespair:
                if (is_hope)
                {
                    return GetHopeEffect();
                }
                else
                {
                    return GetDespairEffect();
                }
        }
        return null;
    }

    public int GetAmountByCardType(CardType card_type, bool is_hope = true)
    {
        switch (card_type)
        {
            case CardType.Normal:
                return GetNormalAmount();
            case CardType.OnlyHope:
                return GetHopeAmount();
            case CardType.OnlyDespair:
                return GetDespairAmount();
            case CardType.HopeAndDespair:
                if (is_hope)
                {
                    return GetHopeAmount();
                }
                else
                {
                    return GetDespairAmount();
                }
        }
        return 0;
    }

    public int GetBonusAmountByCardType(CardType card_type, bool is_hope = true)
    {
        switch (card_type)
        {
            case CardType.Normal:
                return GetNormalAmount();
            case CardType.OnlyHope:
                return GetHopeBonusAmount();
            case CardType.OnlyDespair:
                return GetDespairBonusAmount();
            case CardType.HopeAndDespair:
                if (is_hope)
                {
                    return GetHopeBonusAmount();
                }
                else
                {
                    return GetDespairBonusAmount();
                }
        }
        return 0;
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
                amount = GetNormalAmount();
                popup_text.text = _card_name + "\neffect : " + _normal_effect + "\ncost : " + _normal_cost + "\namount : " + amount;
                break;
            case CardType.HopeAndDespair:
                int hope_amount = GetHopeAmount();
                int despair_amount = GetDespairAmount();

                popup_text.text = _card_name + "\nhope_effect : " + _effect_hope + "\nhope_cost : " + _cost_hope + "\nhope_amount : " + hope_amount
                    + "\ndespair_effect : " + _effect_despair + "\ndespair_cost : " + _cost_despair + "\ndespair_amount : " + despair_amount;
                break;
            case CardType.OnlyHope:
                amount = GetHopeAmount();
                popup_text.text = _card_name + "\neffect : " + _effect_hope + "\ncost : " + _cost_hope + "\namount : " + amount;
                break;
            case CardType.OnlyDespair:
                amount = GetDespairAmount();
                popup_text.text = _card_name + "\neffect : " + _effect_despair + "\ncost : " + _cost_despair + "\namount : " + amount;
                break;
        }
    }
    void ActivateDraw(Player player, Vector3 location, Vector3 scale, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (player.GetDeck().GetDeckCount() == 0)
            {
                break;
            }

            player.GetHands().CreateCard(player.GetDeck().DrawDeck(), location, scale, player.GetDeck());
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
    public int GetCostByCardType(CardType card_type, bool is_hope = true)
    {
        switch (card_type)
        {
            case CardType.Normal:
                return GetCost();
            case CardType.OnlyHope:
                return GetCostOfHope();
            case CardType.OnlyDespair:
                return GetCostOfDespair();
            case CardType.HopeAndDespair:
                if (is_hope)
                {
                    return GetCostOfHope();
                }
                else
                {
                    return GetCostOfDespair();
                }
        }

        return 9999;
    }
    public int GetAmountByCondition(Player player , Situation.PlayingSituation situation)
    {
        if (GetCardType() == CardType.Normal)
        {
            return GetNormalAmount();
        }

        if (GetCardType() == CardType.HopeAndDespair)
        {
            if (player.GetHopeCondition())
            {
                if (situation == Situation.PlayingSituation.Hopeful) {
                    return GetHopeBonusAmount();
                }

                return GetHopeAmount();
            }
            if (player.GetDespairCondition())
            {
                if (situation == Situation.PlayingSituation.Desperate)
                {
                    return GetDespairBonusAmount();
                }
                return GetDespairAmount();
            }
            return 0;
        }

        if (GetCardType() == CardType.OnlyHope)
        {
            return GetHopeAmount();
        }

        if (GetCardType() == CardType.OnlyDespair)
        {
            return GetDespairAmount();
        }

        return 0;
    }

    void Moving()
    {
        if (!_is_moving)
        {
            return;
        }
        if (Vector3.Distance(transform.position, GetPos()) < 0.01f)
        {
            _is_moving = false;
            return;
        }
        this.transform.position = Vector3.MoveTowards(transform.position, GetPos(), MOVING_SPEED * Time.deltaTime);
    }
    //カードを出したときの効果
    public void Effect(Player player, Player enemy, Vector3 location, Vector3 scale, Situation situation)
    {
        if (GetIfNormalCard())
        {
            EffectList(_normal_effect, player, enemy, location, scale, GetNormalAmount(), GetNormalAmount(), situation);
            return;
        }
        else if (!GetIfNormalCard() && player.GetHopeCondition() && !player.GetDespairCondition())
        {
            if (situation.GetSituation() == Situation.PlayingSituation.Hopeful)
            {
                EffectList(_effect_hope, player, enemy, location, scale, GetHopeAmount(), GetHopeBonusAmount(), situation, true);
                return;
            }
            EffectList(_effect_hope, player, enemy, location, scale, GetHopeAmount(), GetHopeBonusAmount(), situation);
            return;
        }
        else if (!GetIfNormalCard() && !player.GetHopeCondition() && player.GetDespairCondition())
        {
            if (situation.GetSituation() == Situation.PlayingSituation.Desperate)
            {
                EffectList(_effect_despair, player, enemy, location, scale, GetDespairAmount(), GetDespairBonusAmount(), situation, true);
                return;
            }
            EffectList(_effect_despair, player, enemy, location, scale, GetDespairAmount(), GetDespairBonusAmount() , situation);
            return;
        }
        Debug.LogError("効果が発動できませんでした");
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
            case 5:
                return EFFECT_TURN_CONDITION_HOPE;
            case 6:
                return EFFECT_TURN_CONDITION_DESPAIR;
            case 7:
                return EFFECT_ENEMY_CONDITION_NORMAL;
            case 8:
                return EFFECT_ENEMY_CONDITION_HOPE;
            case 9:
                return EFFECT_ENEMY_CONDITION_DESPAIR;
        }

        return null;
    }


    void EffectList(string effect_text, Player player, Player enemy, Vector3 location, Vector3 scale, int amount, int bonus_amount, Situation situation ,bool situation_match = false)
    {
        switch (effect_text)
        {
            case EFFECT_ATTACK_TEXT:
                //Debug.Log("攻撃:" + amount + "or" + GetBonusDamage());
                if (situation_match)
                {
                    enemy.AddHP(-bonus_amount);
                    break;
                }
                enemy.AddHP(-amount);
                break;
            case EFFECT_HEAL_TEXT:
                //Debug.Log("回復:" + GetHealAmount() + "or" + GetBonusHealAmount());
                if (situation_match)
                {
                    enemy.AddHP(bonus_amount);
                    break;
                }
                player.AddHP(amount);
                break;
            case EFFECT_DRAW_TEXT:
                //Debug.Log("ドロー:" + GetDrawAmount() + "or" + GetBonusDrawAmount());
                if (situation_match)
                {
                    ActivateDraw(player, location, scale, bonus_amount);
                    break;
                }
                ActivateDraw(player, location, scale, amount);
                break;
            case EFFECT_HOPE_CHANGE:
                //Debug.Log("希望チェンジ");
                player.SetHope();
                break;
            case EFFECT_DESPAIR_CHANGE:
                //Debug.Log("絶望チェンジ");
                player.SetDespair();
                break;
            case EFFECT_TURN_CONDITION_HOPE:
                situation.SetSituation(Situation.PlayingSituation.Hopeful);
                break;
            case EFFECT_TURN_CONDITION_DESPAIR:
                situation.SetSituation(Situation.PlayingSituation.Desperate);
                break;
            case EFFECT_ENEMY_CONDITION_NORMAL:
                enemy.SetNormal();
                Debug.Log("EnemyNormal");
                break;
            case EFFECT_ENEMY_CONDITION_HOPE:
                enemy.SetHope();
                Debug.Log("EnemyHope");
                break;
            case EFFECT_ENEMY_CONDITION_DESPAIR:
                enemy.SetDespair();
                Debug.Log("EnemyDespair");
                break;
        }
    }

    public string GetEffectByCondition(Player player)
    {
        if (GetCardType() != CardType.Normal)
        {
            if (GetCardType() == CardType.HopeAndDespair)
            {
                if (player.GetHopeCondition())
                {
                    return GetHopeEffect();
                }
                if (player.GetDespairCondition())
                {
                    return GetDespairEffect();
                }
                return null;
            }

            if (GetCardType() == CardType.OnlyHope)
            {
                return GetHopeEffect();
            }

            if (GetCardType() == CardType.OnlyDespair)
            {
                return GetDespairEffect();
            }
        }


        if (GetIfNormalCard())
        {
            return GetNormalEffect();
        }

        return null;
    }

}
