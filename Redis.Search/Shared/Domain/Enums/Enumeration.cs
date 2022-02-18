using System.Collections.Concurrent;
using System.Reflection;

namespace Redis.Search.Shared.Domain.Enums
{
    public class Enumeration
    {
        private static readonly ConcurrentDictionary<string, Enumeration> _enumerations = new();

        public string Id { get; private set; }
        public string Name { get; private set; }

        protected Enumeration(
            string id,
            string name)
        {
            Id = id;
            Name = name;
        }

        public static void LoadValue<T>() where T : Enumeration
        {
            foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                if (field.GetValue(null) is Enumeration valueField)
                {
                    _enumerations.TryAdd(valueField.Id, (T)valueField);
                }
            }
        }

        public static T? FromValue<T>(string? value) where T : Enumeration
        {
            if (_enumerations.TryGetValue(value ?? string.Empty, out Enumeration? valueObject))
            {
                return (T)valueObject;
            }

            return default;
        }
    }
}
