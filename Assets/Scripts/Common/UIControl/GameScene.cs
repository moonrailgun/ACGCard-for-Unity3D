using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameScene : MonoBehaviour
{
    public GameCard cardList = new GameCard();//所有卡片集合
    public GameObject selectCardObject;
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
        CreateGameCard(GameSide.Our);
    }

    /// <summary>
    /// 生成游戏卡片
    /// </summary>
    public void CreateGameCard(GameSide side)
    {
        Hashtable cardinfo = new Hashtable();
        CreateGameCard(side, cardinfo);
    }
    public void CreateGameCard(GameSide side,Hashtable cardinfo)
    {
        if (Global.Instance.scene == SceneType.GameScene)
        {
            GameObject card = Resources.Load<GameObject>("Card-small");
            card.GetComponent<Card>().SetCardInfo(cardinfo);//设置属性
            uiManager.AddUIListener(card);//添加UI事件监听

            //实例化卡牌
            GameObject parent = GameObject.Find("GamePanel/" + side.ToString() + "side/CardGrid");
            NGUITools.AddChild(parent, card);
        }
        else
        {
            LogsSystem.Instance.Print("不能在非游戏界面生成游戏卡牌", LogLevel.WARN);
        }
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


