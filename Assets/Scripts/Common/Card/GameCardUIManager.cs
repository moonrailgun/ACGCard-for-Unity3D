using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 游戏界面侧边栏
/// </summary>
public class GameCardUIManager : MonoBehaviour
{
    private GameObject sceneManager;
    private UILabel CardName;
    private UILabel CardType;
    private UILabel CardRarity;
    private UILabel CardOwner;
    private UILabel CardDes;
    private UIGrid CardSkillList;
    private UIPanel CardDesPanel;

    private void Awake()
    {
        if (Global.Instance.scene == SceneType.GameScene)
        {
            this.CardName = GameObject.Find("CardInfo/Name/Label").GetComponent<UILabel>();
            this.CardType = GameObject.Find("CardInfo/Type/Label").GetComponent<UILabel>();
            this.CardRarity = GameObject.Find("CardInfo/Rarity/Label").GetComponent<UILabel>();
            this.CardOwner = GameObject.Find("CardInfo/Owner/Label").GetComponent<UILabel>();
            this.CardDes = GameObject.Find("CardInfo/Description/Label").GetComponent<UILabel>();
            this.CardSkillList = GameObject.Find("SkillList/Grid").GetComponent<UIGrid>();
            this.CardDesPanel = GameObject.Find("GamePanel/CardDes").GetComponent<UIPanel>();

            this.CardName.text = "";
            this.CardType.text = "";
            this.CardRarity.text = "";
            this.CardOwner.text = "";
            this.CardDes.text = "";

            this.sceneManager = GameObject.FindGameObjectWithTag(Tags.SceneController);
            ClearSkillButton();
        }
    }

    public void AddUIListener(GameObject go, GameScene.GameSide side)
    {
        UIEventListener listener = UIEventListener.Get(go);

        //点击事件
        if (side == GameScene.GameSide.Our)
        {
            listener.onClick += OnOurCardSelected;
        }
        else if (side == GameScene.GameSide.Enemy)
        {
            listener.onClick += OnEnemyCardSelected;
        }

        //鼠标指向事件
        listener.onHover += OnCardHover;
    }

    #region 卡片被指向
    /// <summary>
    /// 当卡片被选中后
    /// 显示放大版的卡片
    /// </summary>
    /// <param name="go"></param>
    /// <param name="state"></param>
    private void OnCardHover(GameObject go, bool state)
    {
        if (state == true)
        {
            CardDesPanel.alpha = 1;

            //修改显示的内容
            Card cardData = go.GetComponent<CardContainer>().GetCard();
            CardContainer desCardContainer = CardDesPanel.transform.FindChild("Container/Card").gameObject.GetComponent<CardContainer>();
            desCardContainer.SetCardData(cardData);//设置卡片数据
            desCardContainer.UpdateCardUI();//根据卡片数据刷新卡片UI

            /* ----------------------------------------------------------因为坐标换算问题暂时注释--------------------------------------------------------------------------------------
            //修改显示的位置
            Transform container = CardDesPanel.transform.FindChild("Container");

            Vector2 cardPos = new Vector2(go.transform.localPosition.x, go.transform.localPosition.y);//被选中卡片坐标
            container.transform.localPosition = new Vector3(cardPos.x, cardPos.y, 0);
                
            Vector2 desSize = CardDesPanel.transform.FindChild("Container/Bg").GetComponent<UISprite>().localSize;//卡片描述框大小
            Vector2 cardPos = new Vector2(go.transform.localPosition.x, go.transform.localPosition.y);//被选中卡片坐标
            Debug.Log(cardPos);
            Vector2 cardSize = go.GetComponent<UISprite>().localSize;
            Debug.Log(cardSize);
            float x = cardPos.x + cardSize.x/2;
            float y = cardPos.y + desSize.y > Global.Instance.screenSize.y ? Global.Instance.screenSize.y - desSize.y : cardPos.y;//如果会超出屏幕则贴边否则就和卡片上对齐
            Debug.Log(new Vector2(x, y));
            container.transform.localPosition = new Vector3(x, y, 0);*/
        }
        else
        {
            CardDesPanel.alpha = 0;
        }
    }
    #endregion

    #region 卡片选中
    /// <summary>
    /// 我方卡片选中
    /// 生成技能列表
    /// 修改卡片信息
    /// 设定为选中
    /// </summary>
    private void OnOurCardSelected(GameObject go)
    {
        CardContainer container = go.GetComponent<CardContainer>();
        Card card = container.GetCard();
        if (container != null && card != null && Global.Instance.scene == SceneType.GameScene)
        {
            if (go.transform.IsChildOf(GameObject.Find("Ourside/CardGrid").transform))
            {
                //清空技能按钮列表
                ClearSkillButton();
            }

            ShowCardInfo(card);//显示出卡片信息

            //显示卡片技能列表
            List<Skill> skillList = card.GetCardSkillList();
            foreach (Skill skill in skillList)
            {
                skill.CreateSkillButton();//创建技能图标
            }

            //设置被选中
            this.sceneManager.GetComponent<GameScene>().SetSelectedCard(go);
        }
    }
    /// <summary>
    /// 敌方卡片被选中
    /// </summary>
    /// <param name="go"></param>
    private void OnEnemyCardSelected(GameObject go)
    {
        if (Global.Instance.scene == SceneType.GameScene)
        {
            GameScene gs = sceneManager.GetComponent<GameScene>();
            Skill skill = gs.GetSelectedSkillAndReset();
            GameObject card = gs.GetSelectedCard();
            if (skill != null)
            {
                skill.OnUse(card, go);
            }
            else if (card != null)
            {
                LogsSystem.Instance.Print("尚未实现普通攻击功能", LogLevel.WARN);
                gs.ResetSelectedCard();
            }
            else
            {
                ShortMessagesSystem.Instance.ShowShortMessage("请选择技能或卡片");
            }
        }
    }
    #endregion



    /// <summary>
    /// 清空技能列表
    /// </summary>
    private void ClearSkillButton()
    {
        int childcount = CardSkillList.transform.childCount;
        for (int i = childcount; i > 0; i--)
        {
            DestroyImmediate(CardSkillList.transform.GetChild(0).gameObject);
        }
    }

    /// <summary>
    /// 根据脚本显示出卡片信息
    /// </summary>
    private void ShowCardInfo(Card card)
    {
        this.CardName.text = card.GetCardName();
        this.CardType.text = CardTypes.GetCardTypeNames(card.GetCardType());
        this.CardRarity.text = CardRaritys.GetCardRarityNames(card.GetCardRarity());
        this.CardOwner.text = card.GetCardOwner();
        this.CardDes.text = card.GetCardDescription();
    }
}