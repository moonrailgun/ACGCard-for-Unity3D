using UnityEngine;

public class StartScene : MonoBehaviour
{
    void Start()
    {
        ItemCardManager.Instance.ReadItemJsonDate();
        //return;
        LogsSystem.Instance.Print("游戏开始");
        Application.LoadLevel("LoginScene");
    }
}