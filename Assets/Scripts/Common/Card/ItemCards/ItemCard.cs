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
    { }
    public virtual void OnUse(GameObject target)
    { }

    public override void UpdateCardUIBaseByCardInfo(GameObject container)
    {
        base.UpdateCardUIBaseByCardInfo(container);

        //更换图片
        GameObject character = container.transform.FindChild("Character").gameObject;
        character.GetComponent<UISprite>().spriteName = string.Format("Item-{0}", this.cardName);
    }
}