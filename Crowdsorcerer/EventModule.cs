using System;
using Nancy;
using Nancy.ModelBinding;

namespace Crowdsorcerer
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