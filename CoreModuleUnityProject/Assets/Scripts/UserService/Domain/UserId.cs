using System;

namespace UserData
{
    public class UserId
    {

        private UserId(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("値がnull または 空です。");
            }
            if (!Guid.TryParseExact(value, "D", out _))
            {
                throw new ArgumentException($"{value}のフォーマットが不正です。");
            }
            Value = value;
        }
        public string Value { get; }

        public static UserId New()
        {
            return new UserId(Guid.NewGuid().ToString("D"));
        }
        public static UserId Create(string value)
        {
            return new UserId(value);
        }
    }
}