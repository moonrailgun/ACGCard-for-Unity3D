using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Global
{
    #region 单例模式
    private static Global _instance;
    public static Global Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Global();
            }

            return _instance;
        }
    }
    #endregion

    public string internalVersion = "2015052501";//内部版本号
    public string officialVersion = "V0.1Beta";//正式版本号

    public Vector2 screenSize;
    public SceneType scene;
    public string serverName;
    public string UUID;
    public string playerName;
    public PlayerInfo playerInfo;
    public List<CardInfo> playerOwnCard;//玩家拥有的卡片
    public AllocRoomData playerRoomData;//玩家分配到的房间信息
    public MonoBehaviour activedSceneManager;//当前场景管理器

    public Global()
    {
        screenSize = new Vector2(1280, 720);//默认大小。所有基于屏幕坐标的换算都基于此变量
    }
}

public enum SceneType
{
    LoginScene, SkipScene, MenuScene, GameScene
}