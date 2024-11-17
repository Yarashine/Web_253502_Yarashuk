using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Web_253502_Yarashuk.UI.Extensions
{
    public static class SessionExtensions
    {
        // Метод для получения объекта из сессии
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (value == null)
                return default;

            return JsonConvert.DeserializeObject<T>(value);
        }

        // Метод для установки объекта в сессию
        public static void Set<T>(this ISession session, string key, T value)
        {
            var json = JsonConvert.SerializeObject(value);
            session.SetString(key, json);
        }
    }
}
