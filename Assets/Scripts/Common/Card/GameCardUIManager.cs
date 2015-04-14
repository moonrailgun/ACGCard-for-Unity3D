using UnityEngine;
using System.Collections;

public class GameCardUIManager : MonoBehaviour
{
    public void AddUIListener(GameObject go)
    {
        UIEventListener.Get(go).onClick += OnCardSelected;
    }

    //选中脚本
    private void OnCardSelected(GameObject go)
    {
        Card card = go.GetComponent<Card>();
        if (card != null)
        {
            //根据脚本显示出卡片信息
            Debug.LogWarning("尚未实现");
        }
    }
}