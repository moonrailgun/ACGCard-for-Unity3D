using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

/*
 * 暂时不考虑大数据包传输
 * 游戏界面转到TCP协议处理
 */
public class CardClient : MonoBehaviour
{
    public static string ServerAddress;
    public string hostName = "0.0.0.0";
    private int remotePort = 23333;
    private int localPort = 22233;//固定
    private bool isThreadRun = false;

    private UdpClient udpReceiveClient;
    private UdpClient udpSendClient;

    private List<SocketModel> messageList = new List<SocketModel>();

    /// <summary>
    /// 设置服务器地址并分割为IP和port
    /// </summary>
    /// <param name="serverAddress">完整服务器地址</param>
    public void SetHost(string serverAddress)
    {
        CardClient.ServerAddress = serverAddress;

        //设置ip和port
        string[] iped = serverAddress.Split(new char[] { ':' });
        this.hostName = iped[0];
        this.remotePort = Convert.ToInt32(iped[1]);

        //初始化网络数据
        NetworkInit();
    }

    /// <summary>
    /// 初始化网络数据
    /// </summary>
    private void NetworkInit()
    {
        if (udpReceiveClient == null || udpSendClient == null)
        {
            udpReceiveClient = new UdpClient(localPort);
            udpSendClient = new UdpClient();
        }

        //开始监听
        BeginListen();
    }

    /// <summary>
    /// 同步发送数据到远程
    /// 默认的IP和端口号为hostName和port
    /// </summary>
    public void SendMsg(string message)
    {
        SendMsg(this.hostName, this.remotePort, message);
    }
    public void SendMsg(string hostname, int port, string message)
    {
        if (hostname != "0.0.0.0" && udpSendClient != null)
        {
            try
            {
                byte[] dgram = Encoding.UTF8.GetBytes(message);
                udpSendClient.Send(dgram, dgram.Length, hostname, port);
                LogsSystem.Instance.Print(string.Format("[To {0}:{1}]{2}", hostname, port, message));
            }
            catch (Exception ex)
            {
                LogsSystem.Instance.Print(ex.ToString(), LogLevel.ERROR);
            }
        }
        else
        {
            LogsSystem.Instance.Print("信息发送目标IP未指定或udp发送端不可用", LogLevel.WARN);
        }

    }
    /// <summary>
    /// 发送二进制数据包
    /// </summary>
    /// <param name="packet">数据包</param>
    public void SendPacket(byte[] packet)
    {
        if (this.hostName != "0.0.0.0")
        {
            try
            {
                udpSendClient.Send(packet, packet.Length, hostName, remotePort);
            }
            catch (Exception ex)
            {
                LogsSystem.Instance.Print(ex.ToString(), LogLevel.ERROR);
            }
        }
        else
        {
            LogsSystem.Instance.Print("信息发送目标IP未指定", LogLevel.WARN);
        }
    }

    #region 数据接受
    private void BeginListen()
    {
        LogsSystem.Instance.Print(string.Format("正在监听本地端口{0}!", localPort));
        isThreadRun = true;
        udpReceiveClient.BeginReceive(ReceiveCallBack, null);
    }
    private void ReceiveCallBack(IAsyncResult ar)
    {
        //如果线程标识符为false则关闭线程
        if (isThreadRun == false)
        {
            Debug.Log("已关闭线程");
            return;
        }

        IPEndPoint iped = null;
        try
        {
            //读取消息长度
            byte[] receiveBytes = udpReceiveClient.EndReceive(ar, ref iped);//调用这个函数来结束本次接收并返回接收到的数据长度。 
            LogsSystem.Instance.Print(string.Format("收到来自[{0}]的数据包，长度{1}", iped.ToString(), receiveBytes.Length));
            LogsSystem.Instance.Print(Encoding.UTF8.GetString(receiveBytes));

            //添加到消息列表
            AddMessageList(receiveBytes);
        }
        catch (SocketException)//出现Socket异常就关闭连接 
        {
            udpReceiveClient.Close();//这个函数用来关闭客户端连接 
            return;
        }
        udpReceiveClient.BeginReceive(ReceiveCallBack, null);
    }
    #endregion

    /// <summary>
    /// 调用后关闭线程
    /// </summary>
    public void StopListen()
    {
        this.isThreadRun = false;
        SendMsg("127.0.0.1", 22233, "请求关闭监听");
    }
    private void OnDestroy()
    {
        StopListen();
    }

    #region 消息列表操作
    /// <summary>
    /// 将二进制数据转化为socket对象加入消息列表
    /// </summary>
    /// <param name="receiveBytes">接收到的数据</param>
    private void AddMessageList(byte[] receiveBytes)
    {
        string message = Encoding.UTF8.GetString(receiveBytes);
        SocketModel model = JsonCoding<SocketModel>.decode(message);
        messageList.Add(model);
    }
    /// <summary>
    /// 获取消息列表
    /// </summary>
    public List<SocketModel> GetMessageList()
    {
        return messageList;
    }
    #endregion

}