using System.Text.RegularExpressions;

namespace NextVerBackend.Helpers
{
    public class VideoLinkHelper
    {
        public static string ConvertVideoLink(string inputLink)
        {
            var regex = new Regex(@"(?:https?:\/\/)?(?:www\.)?(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|\S*?[?&]v=)|youtu\.be\/)([a-zA-Z0-9_-]{11})");
            var match = regex.Match(inputLink);

            if (match.Success)
            {
                var videoId = match.Groups[1].Value;
                return $"https://www.youtube.com/embed/{videoId}";
            }

            return inputLink;
        }
    }
}