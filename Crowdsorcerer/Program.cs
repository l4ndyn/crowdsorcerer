using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crowdsorcerer;
using Crowdsorcerer.Sorcerer;
using Crowdsorcerer.Youtube;
using Nancy;
using Nancy.Hosting.Self;
using Serilog;

const string ENDPOINT = "http://localhost:12234/";

Log.Logger = new LoggerConfiguration()
    .ReadFrom.AppSettings()
    .CreateLogger();

Sorcerer sorcerer = new();
EventModule.RegisterSorcerer(sorcerer);

//var path = await new YoutubeProviderLibvideoFast().Get("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
//var uris = await new YoutubeProviderLibvideoFast().GetUris("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
//var uris = await new YoutubeProviderLibvideoFast().GetUris("https://www.youtube.com/watch?v=DvKKziPTB7w");

//var uri = VideoMerger.GetUniqueOutputUri();
//VideoMerger.Merge(uris.audioUri, uris.videoUri, uri);

//VideoWindow videoWindow = new(path);
//VideoWindow videoWindow = new();
//videoWindow.Shown += (_, _) => videoWindow.Play(uri);
//Application.Run(videoWindow);

using var nancyHost = new NancyHost(new Uri(ENDPOINT));
nancyHost.Start();

Log.Information($"Listening on {ENDPOINT}...");

while (true) 
    Console.ReadLine();