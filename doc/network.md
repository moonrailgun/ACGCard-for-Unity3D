# ACGCard网络通讯协议 #
----------

## 通信协议编码 ##
### UDP ###
	public class SocketProtocol
	{
	    public const int OFFLINE = 404;
	    public const int LOGIN = 10;
	    public const int CHAT = 11;
	    public const int PLAYERINFO = 12;
	    public const int CARDINFOLIST = 13;
	}

### TCP ###
    /// <summary>
	/// TCP数据操作代码
	/// </summary>
	public class OperateCode
	{
	    public const int Offline = 404;
	    public const int Identify = 99;
	    public const int AllocRoom = 30;
	    public const int Attack = 31;
	    public const int UseSkill = 32;
	    public const int PlayerOwnCard = 33;
	    public const int SummonCharacter = 34;
	    public const int OperateState = 35;
	    public const int OperateEquip = 36;
	}

### 返回码 ###
    public class ReturnCode
	{
	    public const int Success = 0;
	    public const int Failed = -1;
	    public const int Refuse = 40;//拒绝就是不经过验证。服务器直接拒绝请求
	    public const int Repeal = 80;
	    public const int Request = 1;
	    public const int Pass = 2;
	}