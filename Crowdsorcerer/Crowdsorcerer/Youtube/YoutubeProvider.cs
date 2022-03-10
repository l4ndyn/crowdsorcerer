using System;
using System.Threading.Tasks;

namespace Crowdsorcerer.Youtube
{
    public abstract class YoutubeProvider
    {
        public abstract Task<string> Get(string url);
        public abstract Task<YoutubeVideoInfo> GetUris(string url);
    }
}