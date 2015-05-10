using UnityEngine;
using System.Collections;

/// <summary>
/// 断开连接
/// </summary>
public class DisconnectDTO : CommonDTO
{
    public string UUID;
    public int uid;
    public int protocol;
}

public enum LinkProtocol
{
    UDP = 0,
    TCP = 1
}