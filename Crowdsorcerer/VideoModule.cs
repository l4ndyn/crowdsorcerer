using System;
using Nancy;
using Nancy.ModelBinding;

namespace Crowdsorcerer
{
    record Video
    {
        public string videoUrl, description;
    }

    public class VideoModule : NancyModule
    {
        public VideoModule()
        {
            Post("/videos", x =>
            {
                Video body = this.Bind();
                Console.WriteLine(body);

                return HttpStatusCode.OK;
            });
        }
    }
}