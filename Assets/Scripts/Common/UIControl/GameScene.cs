using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameScene : MonoBehaviour
{
    public GameCard cardList = new GameCard();//所有卡片集合
    private GameObject selectCardObject;
    private GameCardUIManager uiManager;

    private void Awake()
    {
        Global.Instance.scene = SceneType.GameScene;

        uiManager = GetComponent<GameCardUIManager>();
        //获取对战信息
        //获取卡片列表
    }

    private void Start()
    {
        //测试数据
        CreateGameCard(GameSide.Our, new Card(1, "Saber", 1));
        CreateGameCard(GameSide.Our, new Card(1, "Saber", 2));
        CreateGameCard(GameSide.Our, new Card(1, "Saber", 3));
        CreateGameCard(GameSide.Our, new Card(1, "Saber", 4));
        CreateGameCard(GameSide.Our, new Card(1, "Saber", 5));
        CreateGameCard(GameSide.Our, new Card(1, "Saber", 6));
        CreateGameCard(GameSide.Enemy, new Card(1, "Yaya", 1));
        CreateGameCard(GameSide.Enemy, new Card(1, "Rin", 2));
        CreateGameCard(GameSide.Enemy, new Card(1, "Yaya", 3));
        CreateGameCard(GameSide.Enemy, new Card(1, "Yaya", 4));
        CreateGameCard(GameSide.Enemy, new Card(1, "Yaya", 5));
        CreateGameCard(GameSide.Enemy, new Card(1, "Yaya", 6));
        //以上为测试数据
    }

    /// <summary>
    /// 生成游戏卡片
    /// </summary>
    public GameObject CreateGameCard(GameSide side)
    {
        Card cardinfo = new Card();
        return CreateGameCard(side, cardinfo);
    }
    public GameObject CreateGameCard(GameSide side, Card cardinfo)
    {
        if (Global.Instance.scene == SceneType.GameScene)
        {
            //实例化卡牌
            GameObject prefeb = Resources.Load<GameObject>("Card-small");
            GameObject parent = GameObject.Find("GamePanel/" + side.ToString() + "side/CardGrid");
            GameObject card = NGUITools.AddChild(parent, prefeb);

            CardContainer container = card.GetComponent<CardContainer>();
            container.SetCardData(cardinfo);//设置卡片属性
            container.UpdateCardUI();//更新贴图
            uiManager.AddUIListener(card);//添加UI事件监听

            return card;
        }
        else
        {
            LogsSystem.Instance.Print("不能在非游戏界面生成游戏卡牌", LogLevel.WARN);
            return null;
        }
    }

    /// <summary>
    /// 设置当前选中的卡片对象
    /// </summary>
    public void SetSelectedCard(GameObject go)
    {
        this.selectCardObject = go;
    }

    public enum GameSide
    {
        Our, Enemy
    }

    public class GameCard
    {
        public List<Card> OurCharacterCard = new List<Card>();
        public List<Card> EnemyCharacterCard = new List<Card>();
        public List<Card> OurHandCard = new List<Card>();
        public List<Card> EnemyHandCard = new List<Card>();
    }
}


