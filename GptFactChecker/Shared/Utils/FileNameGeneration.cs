using System.Text;

namespace Shared.Utils;

public static class FileNameGeneration
{
    /// <summary>
    /// Creates a valid filename by removing any invalid characters from the input string.
    /// </summary>
    /// <param name="fileName">The string to transform into a valid filename.</param>
    /// <returns>A valid filename string.</returns>
    public static string CreateFileNameFromString(string fileName)
    {
        if (fileName == null)
            throw new ArgumentNullException(nameof(fileName));

        var invalidChars = Path.GetInvalidFileNameChars();

        var validFileName = new StringBuilder();

        foreach (var ch in fileName)
            if (!invalidChars.Contains(ch))
                validFileName.Append(ch);

        var result = validFileName.ToString();

        result = result.Replace(" ", "");

        if (result.Length > 40)
            result = result.Substring(0, 40);

        if (string.IsNullOrWhiteSpace(result))
            result = IdGeneration.GenerateStringId();

        return result;
    }

    public static string EnsureFilePathIsUnique(string filePath)
    {
        if (!File.Exists(filePath))
            return filePath;

        string directory = Path.GetDirectoryName(filePath);
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
        string extension = Path.GetExtension(filePath);

        string newFileName = $"{fileNameWithoutExtension}_{IdGeneration.GenerateStringId()}{extension}";

        return Path.Combine(directory, newFileName);
    }
}

