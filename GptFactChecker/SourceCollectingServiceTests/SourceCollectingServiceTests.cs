using Moq;
using Microsoft.Extensions.Options;
using SourceCollectingService.Strategies;
using SourceCollectingService.Models;
using SourceCollectingService.Transcription.Models;
using Shared.Configuration;
using Shared.Models;
using SourceCollectingService.Audio.Models;
using YoutubeExplode.Common;

namespace SourceCollectingService.Tests;

public class SourceCollectorServiceTests
{
    private readonly Mock<ICollectorStrategyFactory> _collectingStrategyFactoryMock;
    private readonly IOptions<SourceCollectingOptions> _optionsMock;
    private readonly SourceCollectorService _service;

    public SourceCollectorServiceTests()
    {
        _collectingStrategyFactoryMock = new Mock<ICollectorStrategyFactory>();
        _optionsMock = Options.Create(new SourceCollectingOptions());
        _service = new SourceCollectorService(_optionsMock, _collectingStrategyFactoryMock.Object);
    }

    [Fact]
    public async void CollectSource_NullRequest_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CollectSource(null));
    }

    [Fact]
    public async void CollectSource_SuccessfulCollect_ReturnsResponseWithSuccess()
    {
        var source = new Source { Id = "1", SourceType = "TestSourceType" };
        var request = new SourceCollectingRequest { Source = source };
        var strategyMock = new Mock<ICollectorStrategy>();
        var expectedResult = new SourceCollectingResult { IsSuccess = true, Messages = new List<string>() };

        strategyMock.Setup(s => s.CollectSource(It.IsAny<Source>())).ReturnsAsync(expectedResult);
        _collectingStrategyFactoryMock.Setup(f => f.CreateStrategy(It.IsAny<string>())).Returns(strategyMock.Object);

        var response = await _service.CollectSource(request);

        Assert.True(response.IsSuccess);
    }

    [Fact]
    public async void CollectSource_NullSourceInRequest_ThrowsArgumentNullException()
    {
        var request = new SourceCollectingRequest { Source = null };

        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CollectSource(request));
    }

    [Fact]
    public async void CollectSource_FailedCollect_ReturnsResponseWithFailure()
    {
        var source = new Source { Id = "1", SourceType = "TestSourceType" };
        var request = new SourceCollectingRequest { Source = source };
        var strategyMock = new Mock<ICollectorStrategy>();
        var expectedResult = new SourceCollectingResult { IsSuccess = false, Messages = new List<string> { "Failure" } };

        strategyMock.Setup(s => s.CollectSource(It.IsAny<Source>())).ReturnsAsync(expectedResult);
        _collectingStrategyFactoryMock.Setup(f => f.CreateStrategy(It.IsAny<string>())).Returns(strategyMock.Object);

        var response = await _service.CollectSource(request);

        Assert.False(response.IsSuccess);
        Assert.Equal("Failure", response.Messages[0]);
    }

    [Fact]
    public async void CollectSource_CollectThrowsException_ReturnsResponseWithFailureAndExceptionMessage()
    {
        var source = new Source { Id = "1", SourceType = "TestSourceType" };
        var request = new SourceCollectingRequest { Source = source };
        var strategyMock = new Mock<ICollectorStrategy>();

        strategyMock.Setup(s => s.CollectSource(It.IsAny<Source>())).Throws(new Exception("Test exception"));
        _collectingStrategyFactoryMock.Setup(f => f.CreateStrategy(It.IsAny<string>())).Returns(strategyMock.Object);

        var response = await _service.CollectSource(request);

        Assert.False(response.IsSuccess);
        Assert.Contains("Test exception", response.Messages[0]);
    }

    [Fact]
    public async void CollectSource_SuccessfulCollectWithResults_CreatesResponseWithCollectedSource()
    {
        var source = new Source { Id = "1", SourceType = "TestSourceType" };

        var request = new SourceCollectingRequest { Source = source };

        var strategyMock = new Mock<ICollectorStrategy>();

        var audioResult = new AudioCollectingResult
        {
            SourceId = "1",
            Title = "Test title",
            Description = "Test description",
            Author = "Test author",
            Context = "Test context",
            CreatedDate = DateTimeOffset.UtcNow,
            Thumbnails = new List<Thumbnail>() { new Thumbnail("Test URL", new Resolution(10, 10)) },
            Keywords = new List<string> { "Keyword1", "Keyword2" }
        };

        var transcriptionResult = new TranscriptionResult
        {
            SourceId = "2",
            RawTranscription = "Test transcription"
        };

        var transcriptionResult2 = new TranscriptionResult
        {
            SourceId = "3",
            RawTranscription = "Test transcription 2"
        };

        var expectedResult = new SourceCollectingResult
        {
            IsSuccess = true,
            Messages = new List<string>(),
            AudioCollectingResults = new List<AudioCollectingResult> { audioResult },
            TranscriptionResults = new List<TranscriptionResult> { transcriptionResult, transcriptionResult2 }
        };

        strategyMock.Setup(s => s.CollectSource(It.IsAny<Source>())).ReturnsAsync(expectedResult);
        _collectingStrategyFactoryMock.Setup(f => f.CreateStrategy(It.IsAny<string>())).Returns(strategyMock.Object);

        var response = await _service.CollectSource(request);

        Assert.True(response.IsSuccess);
        Assert.NotNull(response.CollectedSource);

        Assert.Equal(audioResult.SourceId, response.CollectedSource.Id);
        Assert.Equal(audioResult.Title, response.CollectedSource.Name);
        Assert.Equal(audioResult.Description, response.CollectedSource.Description);
        Assert.Equal(audioResult.Author, response.CollectedSource.SourcePerson);
        Assert.Equal(audioResult.Context, response.CollectedSource.SourceContext);
        Assert.Equal("Test URL", response.CollectedSource.CoverImageUrl);
        Assert.Equal(transcriptionResult.RawTranscription + " " + transcriptionResult2.RawTranscription, response.CollectedSource.RawText);
    }
}
