using System;
using Nancy;
using Nancy.ModelBinding;
using Serilog;

namespace Crowdsorcerer.Sorcerer
{
    public class EventModule : NancyModule
    {
        static Sorcerer sorcerer;
        public static void RegisterSorcerer(Sorcerer sorcerer) => EventModule.sorcerer = sorcerer;

        public EventModule()
        {
            AddEndpoint<Text>("texts", sorcerer.AddText);
            AddEndpoint<Url>("youtubeUrls", sorcerer.AddYoutube);
            AddEndpoint<Url>("spotifyUrls", sorcerer.AddSpotify);
            AddEndpoint<Reaction>("reactions", sorcerer.AddReaction);
            AddEndpoint<Reaction>("removedReactions", sorcerer.RemoveReaction);
            AddEndpoint<Text>("youtubeTitles", sorcerer.AddYoutube);
        }

        void AddEndpoint<T>(string endpoint, Action<T> action) => Post($"/{endpoint}", x =>
        {
            T body = this.Bind();

            Log.Information($"[Events] Event received on endpoint {endpoint} with body: {body}");
            action(body);

            return HttpStatusCode.OK;
        });
    }
}