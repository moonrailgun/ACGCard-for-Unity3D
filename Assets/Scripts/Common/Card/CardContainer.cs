using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡片类容器
/// 对外接口
/// </summary>
public class CardContainer : MonoBehaviour
{
    private Card card;
    private GameManager.GameSide side;

    private void Awake()
    {
        //当没有卡片数据时，添加一份空数据
        if (this.card == null)
        {
            this.card = new Card();
        }
    }

    private void Update()
    {
        UpdateCardUI();
    }

    public void OnSelected()
    {

    }

    /// <summary>
    /// 受到攻击时。震动卡片
    /// </summary>
    public void ShakeCard()
    {
        Hashtable args = new Hashtable();
        args.Add("amount",  new Vector3(0.05f, 0.05f, 0));//震动范围
        args.Add("time", 0.2f);//震动时间
        args.Add("oncompletetarget", transform.parent.gameObject);
        args.Add("oncomplete", "Reposition");//震动完成后回归正常位

        iTween.ShakePosition(gameObject, args);
    }

    /// <summary>
    /// 更新UI贴图
    /// </summary>
    public void UpdateCardUI()
    {
        card.UpdateCardUIBaseByCardInfo(gameObject);
    }

    public void SetCardData(Card card)
    {
        this.card = card;
        this.card.SetCardContainer(this);//传递卡片容器
    }
    public Card GetCardData()
    {
        return card;
    }
    public Card GetCardClone()
    {
        return (Card)card.Clone();
    }
    public void SetGameSide(GameManager.GameSide side)
    {
        this.side = side;
    }
    public GameManager.GameSide GetGameSide()
    {
        return side;
    }
}