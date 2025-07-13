namespace com.ez.engine.json.mini
{
    public class MiniJson
    {
        /// <summary>
        /// Converts an object into a JSON string
        /// </summary>
        /// <param name="json">Object to convert to JSON string</param>
        /// <returns>JSON string</returns>
        public static string JsonEncode(object json)
        {
            return Json.Serialize(json);
        }

        /// <summary>
        /// Converts an string into a JSON object
        /// </summary>
        /// <param name="json">String to convert to JSON object</param>
        /// <returns>JSON object</returns>
        public static object JsonDecode(string json)
        {
            return Json.Deserialize(json);
        }
    }
}