using System;

namespace Crowdsorcerer
{
    public static class Unique
    {
        public static string Name() => Guid.NewGuid().ToString();
        public static string FileName(string extension) => Name() + "." + extension;
    }
}