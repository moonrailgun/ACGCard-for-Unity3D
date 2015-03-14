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

        //开始监听协同
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


    //异步接受数据
    private void ReceiveMsg()
    {
        udpReceiveState.udpClient.BeginReceive(new AsyncCallback(ProcessResponse), udpReceiveState);
    }

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