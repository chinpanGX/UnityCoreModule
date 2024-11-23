using System;

namespace AppCore.Runtime.UserData
{
    public class UserId
    {
        public string Value { get; }

        public static UserId New() => new(Guid.NewGuid().ToString("D"));
        public static UserId Create(string value) => new(value);
        
        private UserId(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("値がnull または 空です。");
            if (!Guid.TryParseExact(value, "D", out _))
                throw new ArgumentException($"{value}のフォーマットが不正です。");
            Value = value;
        }
    }
}
