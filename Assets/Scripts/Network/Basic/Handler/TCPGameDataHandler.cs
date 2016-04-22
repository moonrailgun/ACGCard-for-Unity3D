using Assets.Scripts.Network.DTO.GameData;
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
            case OperateCode.Attack:
                {
                    return ProcessAttack(data);
                }
            case OperateCode.UseSkill:
                {
                    return ProcessUseSkill(data);
                }
            case OperateCode.PlayerOwnCard:
                {
                    return ProcessPlayerOwnCard(data);
                }
            case OperateCode.SummonCharacter:
                {
                    return ProcessSummonCharacter(data);
                }
            case OperateCode.OperateState:
                {
                    return ProcessOperateState(data);
                }
            case OperateCode.OperateEquip:
                {
                    return ProcessOperateEquip(data);
                }
            case OperateCode.RoundSwitch:
                {
                    return ProcessRoundSwitch(data);
                }
            case OperateCode.GameStart:
                {
                    return ProcessGameStart(data);
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

    private GameData ProcessUseSkill(GameData data)
    {
        UseSkillData detailData = JsonCoding<UseSkillData>.decode(data.operateData);
        if (data.returnCode == ReturnCode.Success)
        {
            this.GetGameManager().ResponseUseSkill(detailData);//将数据转交给游戏管理器处理
        }
        else
        {
            this.GetGameManager().ResponseUseSkill(detailData, false);
        }


        return null;
    }

    private GameData ProcessAttack(GameData data)
    {
        AttackData detailData = JsonCoding<AttackData>.decode(data.operateData);
        this.GetGameManager().ResponseCharacterAttack(detailData);
        return null;
    }

    private GameData ProcessPlayerOwnCard(GameData data)
    {
        if (data.returnCode == ReturnCode.Success)
        {
            LogsSystem.Instance.Print("正在处理玩家拥有卡片的信息");
            GamePlayerOwnCardData ownCardData = JsonCoding<GamePlayerOwnCardData>.decode(data.operateData);
            Global.Instance.playerGameCard = ownCardData.cardInv;//传递给全局
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

        this.GetGameManager().ResponseAddCharacterCard(detailData);//调用游戏管理器处理数据

        return null;
    }

    private GameData ProcessOperateState(GameData data)
    {
        if (data.returnCode == ReturnCode.Success)
        {
            OperateStateData detail = JsonCoding<OperateStateData>.decode(data.operateData);

            int operateCode = detail.stateOperate;
            int skillID = detail.skillID;
            string cardUUID = detail.ownerCardUUID;
            string appendData = detail.appendData;
            GameManager gameManager = this.GetGameManager();

            switch (operateCode)
            {
                case OperateStateData.StateOperateCode.AddState:
                    {
                        gameManager.AddState(cardUUID, skillID, appendData);
                        break;
                    }
                case OperateStateData.StateOperateCode.RemoveState:
                    {
                        gameManager.RemoveState(cardUUID, skillID, appendData);
                        break;
                    }
                case OperateStateData.StateOperateCode.UpdateState:
                    {
                        gameManager.UpdateState(cardUUID, skillID, appendData);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

        }

        return null;
    }

    private GameData ProcessOperateEquip(GameData data)
    {
        if (data.returnCode == ReturnCode.Success)
        {
            OperateEquipData detail = JsonCoding<OperateEquipData>.decode(data.operateData);
            GameManager gameManager = this.GetGameManager();

            int operateCode = detail.operateCode;

            if (operateCode == 0)
            {
                //equip
                gameManager.AddEquipment(detail.operateCardUUID, detail.equipCardId, detail.equipPosition, detail.equipCardAppendData);
            }
            else if (operateCode == 1)
            {
                //unequip
                gameManager.RemoveEquipment(detail.operateCardUUID, detail.equipCardId, detail.equipPosition);
            }
        }

        return null;
    }

    private GameData ProcessRoundSwitch(GameData data)
    {
        if (data.returnCode == ReturnCode.Success)
        {
            RoundSwitchData detail = JsonCoding<RoundSwitchData>.decode(data.operateData);
            if (detail.roundPosition == GetGameManager().GetPlayerRoomData().allocPosition)
            {
                GameManager gameManager = this.GetGameManager();
                gameManager.RoundStart();//回合开始
            }
        }

        return null;
    }

    private GameData ProcessGameStart(GameData data)
    {
        if (data.returnCode == ReturnCode.Success)
        {
            GameManager gameManager = this.GetGameManager();
            RoundSwitchData detail = JsonCoding<RoundSwitchData>.decode(data.operateData);
            gameManager.isGameStarted = true;
            if (detail.roundPosition == GetGameManager().GetPlayerRoomData().allocPosition)
            {
                LogsSystem.Instance.Print("游戏开始， 我方先手", LogLevel.GAMEDETAIL);
                
                gameManager.RoundStart();//回合开始
            }
            else
            {
                GameManager.GameCard gameCard = gameManager.GetGameCardCollection();
                foreach (CharacterCard card in gameCard.GetAllCharacterCard(GameManager.GameSide.Our))
                {
                    LogsSystem.Instance.Print("游戏开始， 敌方先手", LogLevel.GAMEDETAIL);

                    //将我方所有卡片都设为不可用
                    card.SetAvailable(false);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// 获取游戏管理器
    /// </summary>
    private GameManager GetGameManager()
    {
        if (Global.Instance.activedSceneManager is GameScene)
        {
            GameScene gs = Global.Instance.activedSceneManager as GameScene;
            GameManager gm = gs.gameManager;

            return gm;
        }
        else
        {
            LogsSystem.Instance.Print("代码在不正确的场合尝试获取游戏管理器", LogLevel.WARN);
            return null;
        }
    }
}