using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ItemCardManager
{
    #region 单例模式
    private static ItemCardManager _instance;
    public static ItemCardManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ItemCardManager();
            }
            return _instance;
        }
    }
    #endregion

    public void ReadItemJsonDate()
    {

    }
}
