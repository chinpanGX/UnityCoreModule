using System;

namespace UserData
{
    public class UserName
    {
        private static readonly int MinNameLength = 1;
        private static readonly int MaxNameLength = 10;

        private UserName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("値がnull または 空です。");
            }
            if (value.Length < MinNameLength || value.Length > MaxNameLength)
            {
                throw new ArgumentException($"名前の長さは {MinNameLength} 文字以上 {MaxNameLength} 文字以内でなければなりません。");
            }
            Value = value;
        }

        public string Value { get; }
        public static UserName DefaultValue => new("Guest");

        public static UserName Create(string value)
        {
            return new UserName(value);
        }
    }
}