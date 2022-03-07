using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crowdsorcerer.Projector;
using Crowdsorcerer.Youtube;
using Nancy;
using Nancy.Hosting.Self;

const string ENDPOINT = "http://localhost:12234/";

//Sorcerer.Init();

var stopwatch = Stopwatch.StartNew();
//var path = await new YoutubeProviderLibvideoFast().Get("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
//var uris = await new YoutubeProviderLibvideoFast().GetUris("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
var uris = await new YoutubeProviderLibvideoFast().GetUris("https://www.youtube.com/watch?v=DvKKziPTB7w");
stopwatch.Stop();

var uri = VideoMerger.GetUniqueOutputUri();
VideoMerger.Merge(uris.audioUri, uris.videoUri, uri);

//VideoWindow videoWindow = new(path);
VideoWindow videoWindow = new();
videoWindow.Shown += (_, _) => videoWindow.Play(uri);
Application.Run(videoWindow);

/*using (var nancyHost = new NancyHost(new Uri(ENDPOINT)))
{
    nancyHost.Start();

    Console.WriteLine($"Listening on {ENDPOINT}. Press enter to stop.");

    Console.ReadKey();
}*/