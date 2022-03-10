using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Crowdsorcerer.Projectors;
using Crowdsorcerer.Youtube;
using Serilog;

namespace Crowdsorcerer.Sorcerer
{
    public class Sorcerer
    {
        Queue<Message> youtubeQueue;

        Projector projector;
        object projectionTaskLock = new();
        Task currentProjectionTask = Task.CompletedTask;

        public Sorcerer()
        {
            youtubeQueue = new();

            projector = new();
            projector.VideoFinished += PlayNext;
        }

        public void PlayNext()
        {
            Log.Information("[Sorcerer] Playing next media...");
            if (youtubeQueue.Count == 0)
            {
                Log.Information("[Sorcerer] There is no more media in the queue.");
                return;
            }

            var url = youtubeQueue.Dequeue();
            projector.ProjectYoutube(url);
        }

        public void AddYoutube(Url url) => ProjectYoutube(url);
        public void AddYoutube(Text title) => ProjectYoutube(title);

        public void ProjectYoutube(Message source)
        {
            lock (projectionTaskLock)
            {
                if (!projector.IsVideoPlaying && currentProjectionTask.IsCompleted)
                    currentProjectionTask = projector.ProjectYoutube(source);
                else youtubeQueue.Enqueue(source);
            }
        }

        public void AddText(Text text)
        {
            Log.Debug("Text added.");
        }

        public void AddSpotify(Url url)
        {
            Log.Debug("Spot added");
            ProjectSpotify(url);
        }

        public void AddReaction(Reaction reaction)
        {
            //if (yts.TryGetValue(reaction.targetMessageId, out Url url))
            //{
            //    url.votes++;
            //    SortYts();
            //}
        }
        
        public void RemoveReaction(Reaction reaction)
        {
            //if (yts.TryGetValue(reaction.targetMessageId, out Url url))
            //{
            //    url.votes--;
            //    SortYts();
            //}
        }

        //void SortYts() => orderedYts.Sort((x, y) => x.votes.CompareTo(y.votes));

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