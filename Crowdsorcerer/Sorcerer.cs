using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Crowdsorcerer.Projectors;
using Crowdsorcerer.Youtube;
using DotNetTools.SharpGrabber;
using DotNetTools.SharpGrabber.Grabbed;

namespace Crowdsorcerer
{
    public class Sorcerer
    {
        Dictionary<string, Url> yts;
        List<Url> orderedYts;

        Projector projector;

        public Sorcerer()
        {
            yts = new();
            orderedYts = new();

            projector = new();
        }

        public void AddText(Text text)
        {
            Console.WriteLine("Text added.");
        }

        public void AddYoutube(Url url)
        {
            yts.Add(url.messageId, url);
            Console.WriteLine("Yt added");
            ProjectYoutube(url);
        }

        public void AddSpotify(Url url)
        {
            Console.WriteLine("Spot added");
            ProjectSpotify(url);
        }

        public void AddReaction(Reaction reaction)
        {
            if (yts.TryGetValue(reaction.targetMessageId, out Url url))
            {
                url.votes++;
                SortYts();
            }
        }
        
        public void RemoveReaction(Reaction reaction)
        {
            if (yts.TryGetValue(reaction.targetMessageId, out Url url))
            {
                url.votes--;
                SortYts();
            }
        }

        void SortYts() => orderedYts.Sort((x, y) => x.votes.CompareTo(y.votes));

        public void ProjectYoutube(Url url) => projector.ProjectYoutube(url.url);
        void ProjectSpotify(Url url)
        {
            string urlString = url.url;

            try
            {
                Process.Start(urlString);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    urlString = urlString.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {urlString}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", urlString);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", urlString);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}