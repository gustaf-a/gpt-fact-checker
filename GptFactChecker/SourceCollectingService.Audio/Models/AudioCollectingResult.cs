using Shared.Models;
using YoutubeExplode.Common;

namespace SourceCollectingService.Audio.Models;

public class AudioCollectingResult : ResultBase
{
    public string SourceId { get; set; }
    public string AudioFilePath { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
    public TimeSpan? Duration { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public IReadOnlyList<string> Keywords { get; set; }
    public IReadOnlyList<Thumbnail> Thumbnails { get; set; }
    public string Context { get; set; }
    public string SourceUrl { get; internal set; }
    public string SourceType { get; internal set; }
}
