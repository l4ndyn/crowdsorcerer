using System;
using System.Collections.Generic;
using System.IO;

namespace Crowdsorcerer
{
    public static class TempFiles
    {
        static readonly HashSet<string> tempFiles = new();

        public static string New()
        {
            var path = Path.GetTempFileName();
            tempFiles.Add(path);
            
            return path;
        }

        public static void Clear()
        {
            foreach (var tempFile in tempFiles)
            {
                try
                {
                    File.Delete(tempFile);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            tempFiles.Clear();
        }
    }
}