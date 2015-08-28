using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class InvInfoHandler : IHandler
{
    public void Process(SocketModel model)
    {
        if (model.returnCode == ReturnCode.Success)
        {
            if (Global.Instance.scene == SceneType.MenuScene && Global.Instance.activedSceneManager is MenuScene)
            {
                MenuScene menuScene = Global.Instance.activedSceneManager as MenuScene;
                InvInfoDTO data = JsonCoding<InvInfoDTO>.decode(model.message);
                int type = data.type;//player = 1,Hero = 2,Guide = 3,Inv = 4
                switch (type)
                {
                    case 1:
                        {
                            menuScene.ResponsePlayerPage(data.returnData);
                            break;
                        }
                    case 2:
                        {
                            menuScene.ResponseHeroPage(data.returnData);
                            break;
                        }
                    case 3:
                        {
                            menuScene.ResponseGuidePage(data.returnData);
                            break;
                        }
                    case 4:
                        {
                            menuScene.ResponseInvPage(data.returnData);
                            break;
                        }
                    default:
                        {
                            LogsSystem.Instance.Print("接收到非法的数据类型:" + type, LogLevel.WARN);
                            break;
                        }
                }
            }
            else
            {
                LogsSystem.Instance.Print("不合理的游戏场合，数据被抛弃", LogLevel.WARN);
            }
        }
        else
        {
            LogsSystem.Instance.Print("接收到失败的返回值:" + model.returnCode, LogLevel.WARN);
        }
    }
}
