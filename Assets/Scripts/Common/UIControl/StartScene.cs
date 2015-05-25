using UnityEngine;

public class StartScene : MonoBehaviour
{
    void Start()
    {
        LogsSystem.Instance.Print("游戏开始");
        Application.LoadLevel("LoginScene");
    }
}