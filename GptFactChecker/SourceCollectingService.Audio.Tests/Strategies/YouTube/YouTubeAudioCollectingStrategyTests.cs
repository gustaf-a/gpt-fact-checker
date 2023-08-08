using SourceCollectingService.Audio.Strategies.YouTube;
using Shared.Models;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Moq;

public class YouTubeAudioCollectingStrategyTests
{
    private readonly YouTubeAudioCollectingStrategy _strategy;
    private readonly SourceCollectingOptions _options;

    public YouTubeAudioCollectingStrategyTests()
    {
        _options = new SourceCollectingOptions
        {
        };

        var optionsMock = new Mock<IOptions<SourceCollectingOptions>>();
        optionsMock.Setup(o => o.Value).Returns(_options);

        _strategy = new YouTubeAudioCollectingStrategy(optionsMock.Object);
    }

    [Theory]
    [InlineData("https://www.youtube.com/watch?v=gD-3-Yeotdw")]
    [InlineData("https://www.youtube.com/watch?v=khnr4-ehwKA")]
    [InlineData("www.youtube.com/watch?v=uQK2u5tqC9o")]
    [InlineData("https://www.youtube.com/watch?v=_3ngiSxVCBs&list=PLxYlm3T8cftQmB-WesUyWKcqPv1kD3d5Q&index=2")]
    [InlineData("youtube.com/watch?v=_3ngiSxVCBs&list=PLxYlm3T8cftQmB-WesUyWKcqPv1kD3d5Q&index=2")]
    public void TestValidYoutubeUrls(string url)
    {
        Source source = new Source
        {
            SourceUrl = url
        };

        Assert.True(_strategy.IsCompatible(source));
    }

    [Theory]
    [InlineData("www.example.com")]
    [InlineData("youtube.com/watch?invalid-param=123")]
    [InlineData("https://www.youtube.com/watch?v=")]
    [InlineData(null)]
    [InlineData("")]
    public void TestInvalidYoutubeUrls(string url)
    {
        Source source = new Source
        {
            SourceUrl = url
        };

        Assert.False(_strategy.IsCompatible(source));
    }

    [Fact]
    public void TestInvalidSource()
    {
        Source source = null;

        Assert.False(_strategy.IsCompatible(source));
    }
}
