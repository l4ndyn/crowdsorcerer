using System.Text;

namespace Crowdsorcerer
{
    public record Reaction
    {
        public string targetMessageId;
    }

    public record Message
    {
        public string messageId;
        public int votes;
    }

    public record Image : Message
    {
        public string imageUrl, description;
    }

    public record Text : Message
    {
        public string text;
    }

    public record Url : Message
    {
        public string url;
    }

    public record Video : Message
    {
        public string videoUrl, description;
    }

    public record VoiceMessage : Message
    {
        public string audioUrl, description;
    }
}