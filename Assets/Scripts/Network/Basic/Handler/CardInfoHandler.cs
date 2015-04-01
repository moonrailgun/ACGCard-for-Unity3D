using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardInfoHandler : IHandler
{
    public void Process(SocketModel model)
    {
        if (model.returnCode == ReturnCode.Success)
        {
            CardInfoDTO data = JsonCoding<CardInfoDTO>.decode(model.message);

            if (data.cardOwnerId == Global.Instance.playerInfo.uid)
            {
                //将玩家拥有的卡片存到全局中
                Global.Instance.playerOwnCard = new List<CardInfo>(data.cardInfoList);
            }

            //throw new System.NotImplementedException();
        }
    }
}
