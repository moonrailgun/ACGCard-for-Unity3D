using System.Net.Sockets;

public class TCPGameDataHandler
{
    public GameDataDTO Process(GameDataDTO data, Socket socket)
    {
        switch (data.operateCode)
        {
            case OperateCode.Identify:
                {
                    return ProcessIdentify(data);
                }
            default:
                {
                    break;
                }
        }
        return null;
    }

    private GameDataDTO ProcessIdentify(GameDataDTO data)
    {
        if (data.returnCode == ReturnCode.Request)
        {
            //请求身份验证
            GameDataDTO returnData = new GameDataDTO();
            returnData.roomID = -1;
            returnData.returnCode = ReturnCode.Success;
            returnData.operateCode = OperateCode.Identify;
            returnData.operateData = Global.Instance.UUID;

            return returnData;
        }
        return null;
    }
}