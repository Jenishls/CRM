using System;

namespace CRM.Application.Common.Utilities
{
    public static class EnumParser
    {
        public static T Parse<T>(string value) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"Enum value for {typeof(T).Name} cannot be null or empty.");

            if (!Enum.TryParse<T>(value, true, out var result))
                throw new ArgumentException($"Invalid value '{value}' for enum {typeof(T).Name}.");

            return result;
        }
    }
}