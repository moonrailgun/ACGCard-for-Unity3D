
public class CardTypes
{
    public const string Character = "角色卡";
    public const string Cast = "声优卡";
    public const string Item = "道具卡";

    public static string GetCardTypeNames(CardType type)
    {
        switch (type)
        {
            case CardType.Character:
                return "角色卡";
            case CardType.Cast:
                return "声优卡";
            case CardType.Item:
                return "道具卡";
            default:
                return "未知";
        }
    }
}
