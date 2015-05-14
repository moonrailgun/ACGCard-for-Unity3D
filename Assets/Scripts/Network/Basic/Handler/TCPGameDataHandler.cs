using System;
using System.Net.Sockets;
using UnityEngine;

public class TCPGameDataHandler
{
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
                    throw new NotImplementedException();
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

                //载入游戏界面
                Application.LoadLevel("GameScene");
            }
            catch (Exception ex)
            {
                LogsSystem.Instance.Print("分配房间失败:" + ex.ToString(), LogLevel.ERROR);
            }
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
        //添加到场景卡片集合
        /*
        if (side == GameSide.Our)
            this.gameCardCollection.OurCharacterCard.Add(character);
        else if (side == GameSide.Enemy)
            this.gameCardCollection.EnemyCharacterCard.Add(character);

        gameSceneManager.SummonCharacterUp(character, side);//让场景管理器能够调用召唤这张卡*/
        return null;
    }
}