using System;
using System.Collections.Generic;

namespace Crowdsorcerer
{
    public static class Sorcerer
    {
        static Dictionary<string, Url> yts;

        public static void Init()
        {
            yts = new();
        }

        public static void AddText(Text text)
        {
            Console.WriteLine("Text added.");
        }

        public static void AddYoutube(Url url)
        {
            yts.Add(url.messageId, url);
        }

        public static void AddSpotify(Url url)
        {
            Console.WriteLine("Spot added");
        }

        public static void AddReaction(Reaction reaction)
        {
            if (yts.TryGetValue(reaction.targetMessageId, out Url url))
            {
                url.votes++;
            }
        }
        
        public static void RemoveReaction(Reaction reaction)
        {
            if (yts.TryGetValue(reaction.targetMessageId, out Url url))
            {
                url.votes--;
            }
        }


    }
}