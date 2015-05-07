using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AllocRoomData : CommonDTO
{
    public int roomID;
    public int allocPosition;
    public string rivalUUID;
    public int rivalUid;
    public string rivalName;

    public static class Position
    {
        public const int A = 0;
        public const int B = 1;
    }
}
