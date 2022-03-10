using System;
using Nancy;
using Nancy.Extensions;

namespace Crowdsorcerer
{
    public class PingModule : NancyModule
    {
        public PingModule()
        {
            Get("/ping", x => HttpStatusCode.OK);
        }
    }
}