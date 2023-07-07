using FactCheckingService.Utils;
using Xunit;
using System.Collections.Generic;

namespace FactCheckingServiceTests.Utils;

public class TagMatcherTests
{
    private readonly TagMatcher _tagMatcher = new TagMatcher();

    [Fact]
    public void IsMatch_ReturnsTrue_WhenBaseTagIsPartOfPotentiallyCompatibleTag()
    {
        // Arrange
        var baseTags = new List<string> { "music", "climate" };
        var potentiallyCompatibleTags = new List<string> { "climate change" };

        // Act
        var result = _tagMatcher.IsMatch(baseTags, potentiallyCompatibleTags);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsMatch_ReturnsFalse_WhenBaseTagIsNotPartOfPotentiallyCompatibleTag()
    {
        // Arrange
        var baseTags = new List<string> { "vegan", "animal rights activist" };
        var potentiallyCompatibleTags = new List<string> { "vegetarian", "flexitarian" };

        // Act
        var result = _tagMatcher.IsMatch(baseTags, potentiallyCompatibleTags);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsMatch_ReturnsFalse_WhenBaseTagListIsEmpty()
    {
        // Arrange
        var baseTags = new List<string> { };
        var potentiallyCompatibleTags = new List<string> { "global warming" };

        // Act
        var result = _tagMatcher.IsMatch(baseTags, potentiallyCompatibleTags);

        // Assert
        Assert.False(result);

    }

    [Fact]
    public void IsMatch_ReturnsFalse_WhenPotentiallyCompatibleTagListIsEmpty()
    {
        // Arrange
        var baseTags = new List<string> { "health" };
        var potentiallyCompatibleTags = new List<string> { };

        // Act
        var result = _tagMatcher.IsMatch(baseTags, potentiallyCompatibleTags);

        // Assert
        Assert.False(result);
    }    
 
    [Fact]
    public void IsMatch_ReturnsFalse_WhenBothListsAreEmpty()
    {
        // Arrange
        var baseTags = new List<string> { };
        var potentiallyCompatibleTags = new List<string> { };

        // Act
        var result = _tagMatcher.IsMatch(baseTags, potentiallyCompatibleTags);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsMatch_ReturnsFalse_WhenBaseTagIsEmpty()
    {
        // Arrange
        var baseTag = "";
        var potentiallyCompatibleTag = "global warming";

        // Act
        var result = _tagMatcher.IsMatch(baseTag, potentiallyCompatibleTag);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsMatch_ReturnsFalse_WhenPotentiallyCompatibleTagIsEmpty()
    {
        // Arrange
        var baseTag = "climate";
        var potentiallyCompatibleTag = "";

        // Act
        var result = _tagMatcher.IsMatch(baseTag, potentiallyCompatibleTag);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsMatch_IgnoresCase_WhenMatchingTags()
    {
        // Arrange
        var baseTag = "CLIMATE";
        var potentiallyCompatibleTag = "climate change";

        // Act
        var result = _tagMatcher.IsMatch(baseTag, potentiallyCompatibleTag);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void IsMatch_MatchesPartOfWord()
    {
        // Arrange
        var baseTag = "climate";
        var potentiallyCompatibleTag = "noclimatechange";

        // Act
        var result = _tagMatcher.IsMatch(baseTag, potentiallyCompatibleTag);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void IsMatch_IgnoresTrailingSpaces_WhenMatchingTags()
    {
        // Arrange
        var baseTag = " climate ";
        var potentiallyCompatibleTag = "noclimatechange";

        // Act
        var result = _tagMatcher.IsMatch(baseTag, potentiallyCompatibleTag);

        // Assert
        Assert.True(result);
    }
}