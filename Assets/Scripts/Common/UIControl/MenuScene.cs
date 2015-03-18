using UnityEngine;
using System.Collections;

public class MenuScene : MonoBehaviour
{
    private CardClient cardClient;
    private void Awake()
    {
        cardClient = GameObject.FindGameObjectWithTag(Tags.Networks).GetComponent<CardClient>();
    }

    public void OnPublicChattingSubmit(UIInput input)
    {
        string text = input.value;
        LogsSystem.Instance.Print("[用户]" + text);
        if (cardClient != null)
        {
            text.Replace(" ", "-");
            cardClient.SendPacket(Packets.ChatPacket(text));
        }
        input.value = "";
    }
}
