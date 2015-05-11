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
        if (data.returnCode == ReturnCode.Success)
        {
            try
            {
                //如果分配成功
                AllocRoomData roomInfoData = JsonCoding<AllocRoomData>.decode(data.operateData);

                //赋值
                GameClient.Instance.allocRoomData = roomInfoData;

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
}