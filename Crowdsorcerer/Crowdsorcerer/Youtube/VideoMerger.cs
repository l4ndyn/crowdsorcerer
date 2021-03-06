using DotNetTools.SharpGrabber.Converter;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;
using Serilog;

namespace Crowdsorcerer.Youtube
{
    public static class VideoMerger
    {
        static string FFmpegPath => Path.Combine(Environment.CurrentDirectory, "FFmpeg\\bin\\x64\\FFmpeg.exe");

        static CancellationTokenSource cts = new();
        static Uri previousOutputUri;

        public static async Task Merge(string sourceAudioPath, string sourceVideoPath, string outputPath)
        {
            Log.Debug("[VideoMerger] Merging audio and video into file\n" +
                      $"audio path: {sourceAudioPath}\n" +
                      $"video path: {sourceVideoPath}\n" +
                      $"output path: {outputPath}");

            var res = await Cli.Wrap(FFmpegPath)
                .WithArguments($"-y -i {sourceVideoPath} -i {sourceAudioPath} -c:v copy -c:a aac {outputPath}")
                .ExecuteBufferedAsync();

            Log.Debug($"[VideoMerger] Output file {outputPath} successfully created.");
        }

        //TODO: could be unsafe to return the output uri without lock
        public static async Task Merge(Uri sourceAudioUri, Uri sourceVideoUri, Uri outputUri)
        {
            Log.Debug("[VideoMerger] Merging audio and video into stream\n" +
                      $"audio uri: {sourceAudioUri}\n" +
                      $"video uri: {sourceVideoUri}\n" +
                      $"output uri: {outputUri}");

            cts.Cancel();
            if (previousOutputUri != null) ReturnOutputUri(previousOutputUri);

            cts = new();
            previousOutputUri = outputUri;

            var res = await Cli.Wrap(FFmpegPath)
                .WithArguments($"-i \"{sourceVideoUri}\" -i \"{sourceAudioUri}\" -c:v h264 -c:a aac -f flv -listen 1 {outputUri}")
                //.WithStandardErrorPipe(PipeTarget.ToDelegate(l => Console.WriteLine("[ffmpeg] " + l)))
                .ExecuteBufferedAsync();

            Log.Debug($"[VideoMerger] Output file {outputUri} successfully created.");
        }

        public static Uri GetUniqueOutputUri() => new($"http://localhost:{Unique.Port()}/crowdsorcerer/video");

        static void ReturnOutputUri(Uri uri)
        {
            string uriString = uri.ToString();

            int colon = uriString.LastIndexOf(':');
            string startingAtPort = uriString.Substring(colon + 1);

            int slash = startingAtPort.IndexOf('/');
            int port = int.Parse(startingAtPort.Substring(0, slash));

            Unique.ReturnPort(port);
        }
    }
}