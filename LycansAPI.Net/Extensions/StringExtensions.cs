using System;
using System.Security.Cryptography;
using System.Text;

namespace LycansAPI.Net.Extensions
{
    internal static class StringExtensions
    {
        public static Guid ToGuid(this string value)
        {
            MD5 hasher = MD5.Create();
            byte[] data = hasher.ComputeHash(Encoding.UTF8.GetBytes(value));
            return new Guid(data);
        }
    }
}