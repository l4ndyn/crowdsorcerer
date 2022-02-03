using System;
using Nancy;
using Nancy.ModelBinding;

namespace Crowdsorcerer
{
    record Image
    {
        public string imageUrl, description;
    }

    public class ImageModule : NancyModule
    {
        public ImageModule()
        {
            Post("/images", x =>
            {
                Image body = this.Bind();
                Console.WriteLine(body);

                return HttpStatusCode.OK;
            });
        }
    }
}