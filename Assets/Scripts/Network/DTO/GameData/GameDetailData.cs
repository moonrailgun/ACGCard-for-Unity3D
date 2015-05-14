using System;
using System.Collections.Generic;

/// <summary>
/// 所有游戏内操作通用传输类
/// </summary>
public class GameDetailData : CommonDTO
{
    public int operatePlayerUid;
    public string operatePlayerUUID;
    public int operatePlayerPosition;//A = 0，B = 1
}