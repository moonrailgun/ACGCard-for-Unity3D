using System;
using System.Collections.Generic;

/// <summary>
/// 一局游戏的管理器
/// 每次游戏开始都会建立一个新的
/// </summary>
public class GameManager
{
    private GameScene gameSceneManager;//游戏管理器
    private AllocRoomData playerRoomData;//玩家分配到的房间信息
    private List<CardInfo> playerOwnCard;//玩家拥有的卡片
    private GameCard cardList = new GameCard();//所有卡片集合

    public GameManager(GameScene gameSceneManager)
    {
        this.gameSceneManager = gameSceneManager;
        UpdateGameInfo();
    }

    /// <summary>
    /// 更新游戏信息
    /// </summary>
    public void UpdateGameInfo()
    {
        //从全局变量获取信息
        this.playerRoomData = Global.Instance.playerRoomData;
        this.playerOwnCard = Global.Instance.playerOwnCard;

        if (this.gameSceneManager != null)
        {

        }
    }

    /// <summary>
    /// 游戏结束，清空变量
    /// </summary>
    public void GameOver()
    {
        Global.Instance.playerRoomData = null;
        Global.Instance.playerOwnCard = null;

        this.UpdateGameInfo();
    }
    #region 附属结构
    /// <summary>
    /// 游戏中所有卡片列表
    /// </summary>
    public class GameCard
    {
        public List<Card> OurCharacterCard = new List<Card>();
        public List<Card> EnemyCharacterCard = new List<Card>();
        public List<Card> OurHandCard = new List<Card>();
        public List<Card> EnemyHandCard = new List<Card>();
    }
    /// <summary>
    /// 游戏方
    /// </summary>
    public enum GameSide
    {
        Our, Enemy
    }

    #endregion

}