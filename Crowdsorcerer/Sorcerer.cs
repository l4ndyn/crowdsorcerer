using System;
using System.Collections.Generic;

namespace Crowdsorcerer
{
    public static class Sorcerer
    {
        static Dictionary<string, Message> messages;

        public static void Init()
        {
            messages = new();
        }

        public static void AddText(Text text)
        {
            Console.WriteLine("text added");
        }

        public static void AddYoutube(Url url)
        {
            Console.WriteLine("yt added");
        }
        public static void AddSpotify(Url url)
        {
            Console.WriteLine("spot added");
        }
    }
}