using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using Nancy;
using Nancy.Hosting.Self;

const string ENDPOINT = "http://localhost:12234/";

using (var nancyHost = new NancyHost(new Uri(ENDPOINT)))
{
    nancyHost.Start();

    Console.WriteLine($"Listening on {ENDPOINT}. Press enter to stop.");

    Console.ReadKey();
}