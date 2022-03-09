using System;

namespace Crowdsorcerer.Youtube
{
    public record YoutubeVideoInfo(Uri videoUri, Uri audioUri, string title, int length);
}