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