using System;
using Crowdsorcerer.Reactions;
using Nancy;
using Nancy.ModelBinding;

namespace Crowdsorcerer
{
    public class EventModule : NancyModule
    {
        public EventModule()
        {
            AddEndpoint<Text>("texts", Sorcerer.AddText);
            AddEndpoint<Url>("youtubeUrls", Sorcerer.AddYoutube);
            AddEndpoint<Url>("spotifyUrls", Sorcerer.AddSpotify);
        }

        void AddEndpoint<T>(string endpoint, Action<T> action) => Post($"/{endpoint}", x =>
        {
            T body = this.Bind();

            Console.WriteLine(body);
            action(body);

            return HttpStatusCode.OK;
        });
    }
}