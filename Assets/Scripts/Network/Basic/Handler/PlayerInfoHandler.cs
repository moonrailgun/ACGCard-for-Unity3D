using UnityEngine;

public class PlayerInfoHandler : IHandler
{
    public void Process(SocketModel model)
    {
        if (model.returnCode == ReturnCode.Success)
        {
            //服务器正常返回
            PlayerInfoDTO data = JsonCoding<PlayerInfoDTO>.decode(model.message);

            PlayerInfo playerInfo = new PlayerInfo();
            playerInfo.UUID = data.UUID;
            playerInfo.uid = data.uid;
            playerInfo.playerName = data.playerName;
            playerInfo.level = data.level;
            playerInfo.coin = data.coin;
            playerInfo.gem = data.gem;
            playerInfo.vipExpire = data.vipExpire;

            //UDP返回的数据不提交给全局
            Global.Instance.playerInfo = playerInfo;

            if (Global.Instance.scene == SceneType.MenuScene)
            {
                //更新UI数据
                GameObject.FindGameObjectWithTag(Tags.SceneController).GetComponent<MenuScene>().UpdatePlayerInfo();
            }
        }
    }
}

