using System;

namespace Crowdsorcerer
{
    public static class UniqueFileNames
    {
        public static string New(string extension) => Guid.NewGuid() + "." + extension;
    }
}