using System;
using System.IO;
using UnityEngine;

namespace Rascar.Toolbox.Utilities
{
    public static class JsonUtils
    {
        public static TItem[] FromJson<TItem>(string json, bool hasNoWrapper)
        {
            if (hasNoWrapper)
            {
                json = $"{{\"Items\":{json}}}";
            }

            ArrayWrapper<TItem> wrapper = JsonUtility.FromJson<ArrayWrapper<TItem>>(json);

            return wrapper._items;
        }

        public static string GetJsonContent(string jsonFileName, string path)
        {
            string fullPath = $"{path}/{jsonFileName}.json";
            using StreamReader sr = new(fullPath);

            return sr.ReadToEnd();
        }

        [Serializable]
        private class ArrayWrapper<TItem>
        {
            public TItem[] _items;
        }
    }
}
