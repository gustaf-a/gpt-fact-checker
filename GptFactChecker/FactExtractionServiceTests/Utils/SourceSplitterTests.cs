using FactExtractionService.Utils;

namespace FactExtractionServiceTests.Utils;

public class SourceSplitterTests
{
    private readonly ISourceSplitter _sourceSplitter;

    public SourceSplitterTests()
    {
        _sourceSplitter = new SourceSplitter();
    }

    [Fact]
    public void SplitSourceText_GivenParagraphEnds_ShouldSplitAtParagraphEnds()
    {
        // Arrange
        var text = "Hello there.\n\nHow are you?";
        
        // Act
        var result = _sourceSplitter.SplitSourceText(text, 15);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Hello there.", result[0]);
        Assert.Equal("\n\nHow are you?", result[1]);
    }

    [Fact]
    public void SplitSourceText_GivenSentenceEnds_ShouldSplitAtSentenceEnds()
    {
        // Arrange
        var text = "Hello there. How are you?";
        
        // Act
        var result = _sourceSplitter.SplitSourceText(text, 15);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Hello there. ", result[0]);
        Assert.Equal("How are you?", result[1]);
    }

    [Fact]
    public void SplitSourceText_GivenEmptyString_ShouldReturnListWithEmptyString()
    {
        // Arrange
        var text = "";

        // Act
        var result = _sourceSplitter.SplitSourceText(text, 15);

        // Assert
        Assert.Single(result);
        Assert.Equal(text, result[0]);
    }

    [Fact]
    public void SplitSourceText_GivenCharacterLimitIsZero_ShouldReturnWholeTextAsSingleElement()
    {
        // Arrange
        var text = "Hello there. How are you?";

        // Act
        var result = _sourceSplitter.SplitSourceText(text, 0);

        // Assert
        Assert.Single(result);
        Assert.Equal(text, result[0]);
    }

    [Fact]
    public void SplitSourceText_GivenTextLengthLessThanCharacterLimit_ShouldReturnWholeTextAsSingleElement()
    {
        // Arrange
        var text = "Hello";

        // Act
        var result = _sourceSplitter.SplitSourceText(text, 15);

        // Assert
        Assert.Single(result);
        Assert.Equal(text, result[0]);
    }

    [Fact]
    public void SplitSourceText_GivenNoMatchWithSplitterRegex_ShouldSplitAtLastSpace()
    {
        // Arrange
        var text = "Hello there my friend The weather is great!";

        // Act
        var result = _sourceSplitter.SplitSourceText(text, 30);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Hello there my friend ", result[0]);
        Assert.Equal("The weather is great!", result[1]);
    }

    [Fact]
    public void SplitSourceText_GivenMultipleParagraphs_ShouldSplitAtParagraphEnds()
    {
        // Arrange
        var text = "Hello there bud. \n\nHow are you? \n\nI am doing fine.";

        // Act
        var result = _sourceSplitter.SplitSourceText(text, 20);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("Hello there bud. ", result[0]);
        Assert.Equal("\n\nHow are you? ", result[1]);
        Assert.Equal("\n\nI am doing fine.", result[2]);
    }
}