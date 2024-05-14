/*
Topic Name: Electronics Store
Project Name: PcPulse
Group Name: SoftByte
Group Members:
    Shree Dhar Acharya: 8899288
    Karamjot Singh: 8869324
    Prashant Sahu: 8877584
    Jovan Sandhu: 8873660

Description:
- This static class provides helper methods for working with session variables in the PcPulse application.
- It contains two extension methods: Set and Get.
- The Set method is used to serialize and store an object of type T in the session with the specified key.
- The Get method is used to deserialize and retrieve an object of type T from the session using the specified key.
*/
using System.Text.Json;

namespace PcPulse.SessionHelper
{
    public static class SessionExc
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
