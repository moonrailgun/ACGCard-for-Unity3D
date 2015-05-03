using UnityEngine;
using System.Collections;

public class ItemCard : Card
{
    #region 构造函数
    public ItemCard(int cardId, string cardName, CardRarity cardRarity, string cardDescription = "")
        : base(cardId, cardName, CardType.Item, cardRarity, cardDescription)
    { }
    #endregion

    public virtual void OnUse()
    {
        gs.ResetSelectedCard();
    }
    public virtual void OnUse(GameObject target)
    {
        gs.ResetSelectedCard();
        Debug.Log("道具被使用,目标为" + target.GetComponent<CardContainer>().GetCardData().GetCardName());
        Debug.Log("销毁卡片");
        gs.DestoryCard(this);
    }

    /// <summary>
    /// 更新卡片UI
    /// </summary>
    public override void UpdateCardUIBaseByCardInfo(GameObject container)
    {
        base.UpdateCardUIBaseByCardInfo(container);

        //更换图片
        GameObject character = container.transform.FindChild("Character").gameObject;
        character.GetComponent<UISprite>().spriteName = string.Format("Item-{0}", this.cardName);
    }
}