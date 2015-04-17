using UnityEngine;
using System.Collections;

/// <summary>
/// 游戏界面侧边栏
/// </summary>
public class GameCardUIManager : MonoBehaviour
{
    private UILabel CardName;
    private UILabel CardType;
    private UILabel CardRarity;
    private UILabel CardOwner;
    private UILabel CardDes;

    private void Awake()
    {
        if (Global.Instance.scene == SceneType.GameScene)
        {
            this.CardName = GameObject.Find("CardInfo/Name/Label").GetComponent<UILabel>();
            this.CardType = GameObject.Find("CardInfo/Type/Label").GetComponent<UILabel>();
            this.CardRarity = GameObject.Find("CardInfo/Rarity/Label").GetComponent<UILabel>();
            this.CardOwner = GameObject.Find("CardInfo/Owner/Label").GetComponent<UILabel>();
            this.CardDes = GameObject.Find("CardInfo/Description/Label").GetComponent<UILabel>();

            this.CardName.text = "";
            this.CardType.text = "";
            this.CardRarity.text = "";
            this.CardOwner.text = "";
            this.CardDes.text = "";
        }
    }

    public void AddUIListener(GameObject go)
    {
        UIEventListener.Get(go).onClick += OnCardSelected;
    }

    //选中脚本
    private void OnCardSelected(GameObject go)
    {
        Card card = go.GetComponent<CardContainer>().GetCard();
        if (card != null)
        {
            //根据脚本显示出卡片信息
            this.CardName.text = card.GetCardName();
            this.CardType.text = CardTypes.GetCardTypeNames(card.GetCardType());
            this.CardRarity.text = CardRaritys.GetCardRarityNames(card.GetCardRarity());
            this.CardOwner.text = card.GetCardOwner();
            this.CardDes.text = card.GetCardDescription();
        }
    }
}