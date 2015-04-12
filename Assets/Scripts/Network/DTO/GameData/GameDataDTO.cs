using System;
using System.Text;

//游戏数据通用
//TCP数据传输
public class GameDataDTO : CommonDTO
{
    public int roomID;//房间名
    public int returnCode;//返回值
    public int operateCode;//游戏操作名
    public string operateData;//操作数据

    public GameDataDTO()
        :base()
    {
        operateData = "";
    }
}