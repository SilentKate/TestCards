using System;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
    public static List<T> FromJson<T>(string json)
    {
        try
        {
            var wrapper = JsonUtility.FromJson<JsonCollection<T>>(json);
            return wrapper?.Values ?? new List<T>();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        return new List<T>();
    }

    public static string ToJson<T>(List<T> collection)
    {
        if (collection == null) throw new InvalidOperationException("JsonHelper : Can't serialize empty collection");
        var wrapper = new JsonCollection<T> {Values = collection};
        return JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class JsonCollection<T>
    {
        public List<T> Values;
    }
}