using UnityEngine;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;

public class CardClient : MonoBehaviour
{
    public static string ServerAddress;

    private string hostName = "0.0.0.0";
    private int port = 23333;

    private UdpState udpReceiveState = new UdpState();
    private UdpState udpSendState = new UdpState();

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
        this.port = Convert.ToInt32(iped[1]);

        //初始化网络数据
        NetworkInit();

        //开始监听
        ReceiveMsg();
    }

    /// <summary>
    /// 初始化网络数据
    /// </summary>
    private void NetworkInit()
    {
        udpReceiveState.udpClient = new UdpClient(hostName, port);
        udpSendState.udpClient = new UdpClient(hostName, port);
    }

    /// <summary>
    /// 同步发送数据到远程
    /// 默认的IP和端口号为hostName和port
    /// </summary>
    public void SendMsg(string message)
    {
        SendMsg(this.hostName, this.port, message); 
    }
    public void SendMsg(string hostname, int port, string message)
    {
        if (this.hostName != "0.0.0.0")
        {
            byte[] dgram = Encoding.UTF8.GetBytes(message);
            udpSendState.udpClient.Send(dgram, dgram.Length, hostname, port);
        }
        else
        {
            LogsSystem.Instance.Print("信息发送目标IP未指定", LogLevel.WARN);
        }

    }


    //异步接受数据
    private void ReceiveMsg()
    {
        udpReceiveState.udpClient.BeginReceive(new AsyncCallback(ProcessResponse), udpReceiveState);
    }

    /// <summary>
    /// 异步处理接受到的信息
    /// 并挂起下一次的异步接受
    /// </summary>
    private void ProcessResponse(IAsyncResult ar)
    {
        UdpState udpState = ar.AsyncState as UdpState;
        if (ar.IsCompleted)
        {
            byte[] receiveBytes = udpState.udpClient.EndReceive(ar, ref udpReceiveState.ipEndPoint);
            string receiveString = Encoding.UTF8.GetString(receiveBytes);
            LogsSystem.Instance.Print("接收到的数据:" + receiveString);

            //进行下一次的数据接受
            ReceiveMsg();
        }
    }
}

public class UdpState
{
    public UdpClient udpClient = null;
    public IPEndPoint ipEndPoint = null;
    public const int BufferSize = 1024;
    public byte[] buffer = new byte[BufferSize];
    public int counter = 0;
}