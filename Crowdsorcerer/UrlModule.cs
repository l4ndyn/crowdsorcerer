using System;
using Nancy;
using Nancy.ModelBinding;

namespace Crowdsorcerer
{
    record Url
    {
        public string url;
    }

    public class UrlModule : NancyModule
    {
        public UrlModule()
        {
            Post("/youtubeUrls", x =>
            {
                Url body = this.Bind();
                Console.WriteLine($"Youtube: {body}");

                return HttpStatusCode.OK;
            });

            Post("/spotifyUrls", x =>
            {
                Url body = this.Bind();
                Console.WriteLine($"Spotify: {body}");

                return HttpStatusCode.OK;
            });
        }
    }
}