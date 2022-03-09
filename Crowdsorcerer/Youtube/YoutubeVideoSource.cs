namespace Crowdsorcerer.Youtube
{
    public record YoutubeVideoSource;

    public record YoutubeUrl : YoutubeVideoSource
    {
        public string url;
    }

    public record YoutubeTitle : YoutubeVideoSource
    {
        public string title;
    }
}