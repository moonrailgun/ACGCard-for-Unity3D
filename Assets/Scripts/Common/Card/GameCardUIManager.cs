using UnityEngine;
using System.Collections;

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
        Card card = go.GetComponent<Card>();
        if (card != null)
        {
            //根据脚本显示出卡片信息
            this.CardName.text = card.cardName;
            this.CardType.text = CardTypes.GetCardTypeNames(card.cardType);
            this.CardRarity.text = CardRaritys.GetCardRarityNames(card.cardRarity);
            this.CardOwner.text = card.cardOwner;
            this.CardDes.text = card.cardDescription;
        }
    }
}