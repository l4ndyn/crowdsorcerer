using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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

        Queue<YoutubeVideoSource> youtubeQueue;

        Projector projector;
        object projectionTaskLock = new();
        Task currentProjectionTask;

        public Sorcerer()
        {
            yts = new();
            orderedYts = new();

            youtubeQueue = new();

            projector = new();
            projector.VideoFinished += PlayNext;
        }

        public void PlayNext()
        {
            if (youtubeQueue.Count == 0) return;

            var url = youtubeQueue.Dequeue();
            projector.ProjectYoutube(url);
        }

        public void AddText(Text text)
        {
            Console.WriteLine("Text added.");
        }

        public void AddYoutube(Url url)
        {
            lock (projectionTaskLock)
            {
                var ytUrl = new YoutubeUrl { url = url.url };

                if (!projector.IsVideoPlaying && (currentProjectionTask == null || currentProjectionTask.IsCompleted))
                    currentProjectionTask = projector.ProjectYoutube(ytUrl);
                else youtubeQueue.Enqueue(ytUrl);
            }
        }

        public void AddYoutube(Text title)
        {
            lock (projectionTaskLock)
            {
                var ytTitle = new YoutubeTitle { title = title.text };

                if (!projector.IsVideoPlaying && (currentProjectionTask == null || currentProjectionTask.IsCompleted))
                    currentProjectionTask = projector.ProjectYoutube(ytTitle);
                else youtubeQueue.Enqueue(ytTitle);
            }
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