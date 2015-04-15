public class CardRaritys
{
    public const string Normal = "普通";
    public const string Excellent = "精良";
    public const string Scarce = "稀有";
    public const string Rare = "罕见";
    public const string Legend = "传说";
    public const string Epic = "史诗";

    public static string GetCardRarityNames(CardRarity rarity)
    {
        switch(rarity)
        {
            case CardRarity.Normal:
                return "普通";
            case CardRarity.Excellent:
                return "精良";
            case CardRarity.Scarce:
                return "稀有";
            case CardRarity.Rare:
                return "罕见";
            case CardRarity.Legend:
                return "传说";
            case CardRarity.Epic:
                return "史诗";
            default:
                return "未知";
        }
    }
}