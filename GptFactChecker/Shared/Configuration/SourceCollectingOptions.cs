using System.Reflection;

namespace Shared.Configuration;

public class SourceCollectingOptions
{
    public const string SourceCollecting = "SourceCollecting";

    public SourceCollectingOptions()
    {
        if(AudioFilesFolderPath is null)
        {
            string executingAssemblyLocation = Assembly.GetExecutingAssembly().Location;
            AudioFilesFolderPath = Path.GetDirectoryName(executingAssemblyLocation);
        }
    }

    public string AudioFilesFolderPath { get; set; }

    public string TranscriptionResultsConcatenatingCharacter { get; set; } = " ";
}
