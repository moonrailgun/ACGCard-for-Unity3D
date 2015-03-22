using LitJson;

public class JsonCoding<T> {
    /// <summary>
    /// 将DTO编码成json数据
    /// </summary>
    public static string encode(T model)
    {
        return JsonMapper.ToJson(model);
    }

    /// <summary>
    /// 将json数据解码成DTO
    /// </summary>
    public static T decode(string message)
    {
        return JsonMapper.ToObject<T>(message);
    }
}