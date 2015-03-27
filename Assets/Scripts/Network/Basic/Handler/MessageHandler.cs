﻿using UnityEngine;
using System.Collections.Generic;

public class MessageHandler : MonoBehaviour {
    private CardClient cardClient;
    private LoginHandler loginHandler;
    private ChatHandler chatHandler;
    private PlayerInfoHandler playerInfoHandler;

    private void Awake()
    {
        cardClient = GameObject.FindGameObjectWithTag(Tags.Networks).GetComponent<CardClient>();
        loginHandler = new LoginHandler();
        chatHandler = new ChatHandler();
        playerInfoHandler = new PlayerInfoHandler();
    }
    private void Update()
    {
        List<SocketModel> messageList = cardClient.GetMessageList();
        if (messageList.Count > 0)
        {
            SocketModel model = messageList[0];
            ProcessMessage(model);
            messageList.RemoveAt(0);
        }
    }

    /// <summary>
    /// 处理SOCKET消息
    /// </summary>
    private void ProcessMessage(SocketModel model)
    {
        LogsSystem.Instance.Print(string.Format("数据返回：返回码[{0}] 协议[{1}] 消息[{2}]", model.returnCode, model.protocol, model.message));
        switch (model.protocol)
        {
            case SocketProtocol.LOGIN:
                {
                    loginHandler.Process(model);
                    break;
                }
            case SocketProtocol.CHAT:
                {
                    chatHandler.Process(model);
                    break;
                }
            case SocketProtocol.PLAYERINFO:
                {
                    playerInfoHandler.Process(model);
                    break;
                }
            default:
                {
                    LogsSystem.Instance.Print("未知的协议");
                    break;
                }
        }
    }
}
