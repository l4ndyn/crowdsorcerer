using DotNetTools.SharpGrabber.Converter;
using System;
using System.IO;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;

namespace Crowdsorcerer.Youtube
{
    public static class VideoMerger
    {
        static string FFmpegPath => Path.Combine(Environment.CurrentDirectory, "FFmpeg\\bin\\x64\\FFmpeg.exe");
        
        public static async Task Merge(string sourceAudioPath, string sourceVideoPath, string outputPath)
        {
            var res = await Cli.Wrap(FFmpegPath)
                .WithArguments($"-y -i {sourceVideoPath} -i {sourceAudioPath} -c:v copy -c:a aac {outputPath}")
                .ExecuteBufferedAsync();

            Console.WriteLine(res.StandardOutput);

            Console.WriteLine("Output file successfully created.");
        }
        public static async Task Merge(Uri sourceAudioUri, Uri sourceVideoUri, Uri outputUri)
        {
            var res = await Cli.Wrap(FFmpegPath)
                .WithArguments($"-i {sourceVideoUri} -i {sourceAudioUri} -c:v h264 -c:a aac -f flv -listen 1 {outputUri}")
                .ExecuteBufferedAsync();

            Console.WriteLine(res.StandardOutput);

            Console.WriteLine("Output file successfully created.");
        }

        public static Uri GetUniqueOutputUri() => new("http://localhost:12235/crowdsorcerer/video");
    }
}