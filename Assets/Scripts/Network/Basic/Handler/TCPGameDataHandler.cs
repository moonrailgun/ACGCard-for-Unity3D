using System;
using System.Net.Sockets;
using UnityEngine;

public class TCPGameDataHandler
{
    private GameManager gameManager;

    public GameData Process(GameData data, Socket socket)
    {
        switch (data.operateCode)
        {
            case OperateCode.Identify:
                {
                    return ProcessIdentify(data);
                }
            case OperateCode.AllocRoom:
                {
                    return ProcessAllocRoom(data);
                }
            case OperateCode.Offline:
                {
                    //Windows.CreateWindows("断线", "您已经断开了连接", "重新登录", UIWidget.Pivot.Top, null, Windows.WindowsType.MessageWindow);
                    return ProcessOffline(data);
                }
            case OperateCode.PlayerOwnCard:
                {
                    return ProcessPlayerOwnCard(data);
                }
            case OperateCode.SummonCharacter:
                {
                    return ProcessSummonCharacter(data);
                }
            default:
                {
                    break;
                }
        }
        return null;
    }

    private GameData ProcessIdentify(GameData data)
    {
        if (data.returnCode == ReturnCode.Request)
        {
            //请求身份验证
            GameData returnData = new GameData();
            returnData.roomID = -1;
            returnData.returnCode = ReturnCode.Success;
            returnData.operateCode = OperateCode.Identify;
            returnData.operateData = Global.Instance.UUID;

            return returnData;
        }
        return null;
    }

    private GameData ProcessAllocRoom(GameData data)
    {
        if (data.returnCode == ReturnCode.Success)//如果分配成功
        {
            try
            {
                //赋值给全局
                AllocRoomData roomInfoData = JsonCoding<AllocRoomData>.decode(data.operateData);
                Global.Instance.playerRoomData = roomInfoData;
                if (GameClient.Instance.GetGameSceneManager() != null)
                {
                    GameClient.Instance.GetGameSceneManager().gameManager.UpdateGameInfo();
                }

                LogsSystem.Instance.Print("已经分配到房间号:" + roomInfoData.roomID + ",正在载入游戏");

                System.Threading.Thread.Sleep(200);//尝试用同步阻塞

                //载入游戏界面
                Application.LoadLevel("GameScene");
            }
            catch (Exception ex)
            {
                LogsSystem.Instance.Print("分配房间失败:" + ex.ToString(), LogLevel.ERROR);
            }
        }
        else
        {
            LogsSystem.Instance.Print("返回数据为不成功");
        }

        return null;
    }

    private GameData ProcessPlayerOwnCard(GameData data)
    {
        if (data.returnCode == ReturnCode.Success)
        {
            GamePlayerOwnCardData ownCardData = JsonCoding<GamePlayerOwnCardData>.decode(data.operateData);
            Global.Instance.playerOwnCard = ownCardData.cardInv;//传递给全局
            if (GameClient.Instance.GetGameSceneManager() != null)
            {
                GameClient.Instance.GetGameSceneManager().gameManager.UpdateGameInfo();
            }
        }

        return null;
    }

    private GameData ProcessOffline(GameData data)
    {
        if (GameClient.Instance.gameClient.Connected)
        {
            GameClient.Instance.gameClient.Client.EndReceive(null);
            GameClient.Instance.gameClient.Close();
            ShortMessagesSystem.Instance.ShowShortMessage("您已经断开了连接");
        }
        return null;
    }

    private GameData ProcessSummonCharacter(GameData data)
    {
        SummonCharacterData detailData = JsonCoding<SummonCharacterData>.decode(data.operateData);
        if (Global.Instance.activedSceneManager is GameScene)
        {
            GameScene gs = Global.Instance.activedSceneManager as GameScene;
            GameManager gm = gs.gameManager;

            gm.ResponseAddCharacterCard(detailData);//调用游戏管理器处理数据
        }
        else
        {
            LogsSystem.Instance.Print("在不正确的场合接收到了数据ID" + data.operateCode, LogLevel.WARN);
        }
        return null;
    }
}