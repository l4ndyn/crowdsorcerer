using System;
using Nancy;
using Nancy.Extensions;
using Nancy.ModelBinding;

namespace Crowdsorcerer
{
    record Text
    {
        public string text;
    }

    public class TextModule : NancyModule
    {
        public TextModule()
        {
            Post("/texts", x =>
            {
                var body = this.Bind<Text>();
                Console.WriteLine(body);

                return HttpStatusCode.OK;
            });
        }
    }
}