using System;
using UnityEngine;

public class PlayerInfoDTO : CommonDTO
{
    public string UUID;
    public int uid;
    public string playerName;
    public int level;
    public int coin;
    public int gem;
    public DateTime vipExpire;

    public PlayerInfoDTO()
        : base()
    {

    }

    public PlayerInfoDTO(string UUID)
        : base()
    {
        this.UUID = UUID;
    }
}