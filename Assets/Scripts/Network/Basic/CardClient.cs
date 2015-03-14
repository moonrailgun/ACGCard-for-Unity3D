using UnityEngine;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

public class CardClient : MonoBehaviour
{
    public static string ServerAddress;

    private string hostName = "0.0.0.0";
    private int remotePort = 23333;
    private int localPort = 22233;//固定
    private Thread listenThread;
    private bool isThreadRun = false;

    private UdpClient udpReceiveClient;
    private UdpClient udpSendClient;

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
        listenThread = new Thread(new ThreadStart(BeginListen));
        listenThread.Start();
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
        if (hostname != "0.0.0.0")
        {
            byte[] dgram = Encoding.UTF8.GetBytes(message);
            udpSendClient.Send(dgram, dgram.Length, hostname, port);
        }
        else
        {
            LogsSystem.Instance.Print("信息发送目标IP未指定", LogLevel.WARN);
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

    //------------------------------------------------------------------------------这里消息没有写入日志。后期要改为写入日志
    private void BeginListen()
    {
        //LogsSystem.Instance.Print(string.Format("正在监听本地端口{0}!", localPort));
        isThreadRun = true;
        Debug.Log(string.Format("正在监听本地端口{0}!", localPort));
        this.listenThread.IsBackground = true;//将线程设为后台线程,当前台线程全部关闭后会自动关闭后台线程
        while (true)
        {
            //当标识为否的时候，退出线程
            if (!this.isThreadRun) { return; }

            IPEndPoint remoteEP = null;
            byte[] bytes = udpReceiveClient.Receive(ref remoteEP);
            string message = Encoding.UTF8.GetString(bytes);
            //LogsSystem.Instance.Print(string.Format("[远程{0}]:{1}", remoteEP, message));
            Debug.Log(string.Format("[远程{0}]:{1}", remoteEP, message));
        }
    }

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
        Debug.Log("已关闭线程");
    }
}