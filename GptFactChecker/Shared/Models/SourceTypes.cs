
namespace Shared.Models;

public static class SourceTypes
{
    public static List<string> AllTypes = new()
    {
        Video, Podcast, Text, SocialMedia
    };

    public const string Video = "video";
    public const string Podcast = "podcast";
    public const string Text = "text";
    public const string SocialMedia = "socialmedia";
}
