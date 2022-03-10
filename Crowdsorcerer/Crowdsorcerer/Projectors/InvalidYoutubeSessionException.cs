using System;

namespace Crowdsorcerer.Projectors
{
    public class InvalidYoutubeSessionException : Exception
    {
        public InvalidYoutubeSessionException(string message) : base(message)
        { }
    }
}