using LitJson;

public class IntArray
{
    public static string IntArrayToString(int[] array)
    {
        JsonData data = new JsonData();
        foreach (int a in array)
        {
            data.Add(a);
        }

        return data.ToJson();
    }

    public static int[] StringToIntArray(string json)
    {
        int[] array = JsonMapper.ToObject<int[]>(json);

        return array;
    }
}