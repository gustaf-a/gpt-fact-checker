using Shared.Utils;

namespace SharedTests.Utils;

public class FileNameGenerationTests
{
    [Fact]
    public void CreateFileNameFromString_ShouldThrowArgumentNullException_WhenNullIsPassed()
    {
        Assert.Throws<ArgumentNullException>(() => FileNameGeneration.CreateFileNameFromString(null));
    }

    [Fact]
    public void CreateFileNameFromString_ShouldRemoveInvalidChars_FromFileName()
    {
        string fileName = "test<>|\"?:*file.txt";
        string expected = "testfile.txt";

        string actual = FileNameGeneration.CreateFileNameFromString(fileName);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CreateFileNameFromString_ShouldNotChange_ValidFileName()
    {
        string fileName = "validfile.txt";
        string expected = "validfile.txt";

        string actual = FileNameGeneration.CreateFileNameFromString(fileName);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CreateFileNameFromString_ShouldTrimSpaces()
    {
        string fileName = "valid file ";
        string expected = "validfile";

        string actual = FileNameGeneration.CreateFileNameFromString(fileName);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CreateFileNameFromString_ShouldShortenFileName_WhenFileNameIsTooLong()
    {
        string fileName = new string('a', 50);
        string expected = new string('a', 40);

        string actual = FileNameGeneration.CreateFileNameFromString(fileName);

        Assert.Equal(expected, actual);
    }
}
