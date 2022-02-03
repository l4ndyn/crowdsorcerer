using System;
using Nancy;
using Nancy.ModelBinding;

namespace Crowdsorcerer
{
    record VoiceMessage
    {
        public string audioUrl, description;
    }

    public class VoiceMessageModule : NancyModule
    {
        public VoiceMessageModule()
        {
            Post("/voiceMessages", x =>
            {
                VoiceMessage body = this.Bind();
                Console.WriteLine(body);

                return HttpStatusCode.OK;
            });
        }
    }
}