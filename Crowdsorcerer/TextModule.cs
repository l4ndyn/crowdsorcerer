﻿using System;
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
                Text body = this.Bind();
                Console.WriteLine(body);

                return HttpStatusCode.OK;
            });
        }
    }
}