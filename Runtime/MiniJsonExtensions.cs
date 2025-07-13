using System;
using System.Collections.Generic;

namespace com.ez.engine.json.mini
{
    public static class MiniJsonExtensions
    {
        /// <summary>
        /// Get the HashDictionary of a key in JSON dictionary.
        /// </summary>
        /// <param name="dic">The JSON in dictionary representations.</param>
        /// <param name="key">The Key to get the HashDictionary from in the JSON dictionary.</param>
        /// <returns>The HashDictionary found in the JSON</returns>
        public static Dictionary<string, object> GetHash(this Dictionary<string, object> dic, string key)
        {
            return (Dictionary<string, object>)dic[key];
        }

        /// <summary>
        /// Get the casted enum in the JSON dictionary.
        /// </summary>
        /// <param name="dic">The JSON in dictionary representations.</param>
        /// <param name="key">The Key to get the casted enum from in the JSON dictionary.</param>
        /// <typeparam name="T">The class to cast the enum.</typeparam>
        /// <returns>The casted enum or will return T if the key was not found in the JSON dictionary.</returns>
        public static T GetEnum<T>(this Dictionary<string, object> dic, string key)
        {
            if (dic.ContainsKey(key))
            {
                return (T)Enum.Parse(typeof(T), dic[key].ToString(), true);
            }

            return default;
        }

        /// <summary>
        /// Get the string in the JSON dictionary.
        /// </summary>
        /// <param name="dic">The JSON in dictionary representations.</param>
        /// <param name="key">The Key to get the string from in the JSON dictionary.</param>
        /// <param name="defaultValue">The default value to send back if the JSON dictionary doesn't contains the key.</param>
        /// <returns>The string from the JSON dictionary or the default value if there is none</returns>
        public static string GetString(this Dictionary<string, object> dic, string key, string defaultValue = "")
        {
            if (dic.ContainsKey(key))
            {
                return dic[key].ToString();
            }

            return defaultValue;
        }

        /// <summary>
        /// Get the long in the JSON dictionary.
        /// </summary>
        /// <param name="dic">The JSON in dictionary representations.</param>
        /// <param name="key">The Key to get the long from in the JSON dictionary.</param>
        /// <returns>The long from the JSON dictionary or 0 if the key was not found in the JSON dictionary</returns>
        public static long GetLong(this Dictionary<string, object> dic, string key)
        {
            if (dic.ContainsKey(key))
            {
                return long.Parse(dic[key].ToString());
            }

            return 0;
        }

        /// <summary>
        /// Get the list of strings in the JSON dictionary.
        /// </summary>
        /// <param name="dic">The JSON in dictionary representations.</param>
        /// <param name="key">The Key to get the list of strings from in the JSON dictionary.</param>
        /// <returns>The list of strings from the JSON dictionary or an empty list of strings if the key was not found in the JSON dictionary</returns>
        public static List<string> GetStringList(this Dictionary<string, object> dic, string key)
        {
            if (dic.ContainsKey(key))
            {
                var result = new List<string>();
                var objs = (List<object>)dic[key];
                foreach (var v in objs)
                {
                    result.Add(v.ToString());
                }

                return result;
            }

            return new List<string>();
        }

        /// <summary>
        /// Get the bool in the JSON dictionary.
        /// </summary>
        /// <param name="dic">The JSON in dictionary representations.</param>
        /// <param name="key">The Key to get the bool from in the JSON dictionary.</param>
        /// <returns>The bool from the JSON dictionary or false if the key was not found in the JSON dictionary</returns>
        public static bool GetBool(this Dictionary<string, object> dic, string key)
        {
            if (dic.ContainsKey(key))
            {
                return bool.Parse(dic[key].ToString());
            }

            return false;
        }

        /// <summary>
        /// Get the casted object in the JSON dictionary.
        /// </summary>
        /// <param name="dic">The JSON in dictionary representations.</param>
        /// <param name="key">The Key to get the casted object from in the JSON dictionary.</param>
        /// <typeparam name="T">The class to cast the object.</typeparam>
        /// <returns>The casted object or will return T if the key was not found in the JSON dictionary.</returns>
        public static T Get<T>(this Dictionary<string, object> dic, string key)
        {
            if (dic.ContainsKey(key))
            {
                return (T)dic[key];
            }

            return default;
        }

        /// <summary>
        /// Convert a Dictionary to JSON.
        /// </summary>
        /// <param name="obj">The dictionary to convert to JSON.</param>
        /// <returns>The converted dictionary in JSON string format.</returns>
        public static string toJson(this Dictionary<string, object> obj)
        {
            return MiniJson.JsonEncode(obj);
        }

        /// <summary>
        /// Convert a Dictionary to JSON.
        /// </summary>
        /// <param name="obj">The dictionary to convert to JSON.</param>
        /// <returns>The converted dictionary in JSON string format.</returns>
        public static string toJson(this Dictionary<string, string> obj)
        {
            return MiniJson.JsonEncode(obj);
        }

        /// <summary>
        /// Convert a string array to JSON.
        /// </summary>
        /// <param name="array">The string array to convert to JSON.</param>
        /// <returns>The converted dictionary in JSON string format.</returns>
        public static string toJson(this string[] array)
        {
            var list = new List<object>();
            foreach (var s in array)
            {
                list.Add(s);
            }

            return MiniJson.JsonEncode(list);
        }

        /// <summary>
        /// Convert string JSON into List of Objects.
        /// </summary>
        /// <param name="json">String JSON to convert.</param>
        /// <returns>List of Objects converted from string json.</returns>
        public static List<object> ArrayListFromJson(this string json)
        {
            return MiniJson.JsonDecode(json) as List<object>;
        }

        /// <summary>
        /// Convert string JSON into Dictionary.
        /// </summary>
        /// <param name="json">String JSON to convert.</param>
        /// <returns>Dictionary converted from string json.</returns>
        public static Dictionary<string, object> HashtableFromJson(this string json)
        {
            return MiniJson.JsonDecode(json) as Dictionary<string, object>;
        }
    }
}