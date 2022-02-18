using System.Collections.Concurrent;
using System.Reflection;

namespace Redis.Search.Shared.Domain.Enums
{
    public class Enumeration
    {
        private static readonly ConcurrentDictionary<int, Enumeration> _enumerations = new();

        public int Id { get; private set; }
        public string Name { get; private set; }

        protected Enumeration(
            int id,
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

        public static T? FromValue<T>(int value) where T : Enumeration
        {
            if (_enumerations.TryGetValue(value, out Enumeration? valueObject))
            {
                return (T)valueObject;
            }

            return default;
        }

        public static bool HasValue<T>(int value) where T : Enumeration
        {
            return _enumerations.TryGetValue(value, out _);
        }
    }
}
