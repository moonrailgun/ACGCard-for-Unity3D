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

                if (Global.Instance.scene == SceneType.MenuScene)
                {
                    MenuScene scene = GameObject.FindGameObjectWithTag(Tags.SceneController).GetComponent<MenuScene>();
                    if (scene.isWaittingForCardInv)
                    {
                        //如果场景正在等待卡片列表显示。马上显示
                        scene.UpdatePlayerCardList();
                        scene.isWaittingForCardInv = false;
                    }
                }
            }
        }
    }
}
