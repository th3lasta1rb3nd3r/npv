using Microsoft.AspNetCore.Mvc;
using Moq;
using Npv.Api.Controllers;
using Npv.Api.Requests;
using Npv.Api.Results;
using Npv.Api.Services;


namespace Npv.Api.Tests.Controllers;

public class NpvControllerTests
{
    private readonly Mock<INpvCalculatorService> _npvCalculatorServiceMock;
    private readonly NpvController _controller;

    public NpvControllerTests()
    {
        _npvCalculatorServiceMock = new Mock<INpvCalculatorService>();
        _controller = new NpvController(_npvCalculatorServiceMock.Object);
    }

    [Fact]
    public async Task CalculateNPV_ValidRequest_ReturnsOkWithResults()
    {
        // Arrange
        var request = new NpvRequest
        {
            CashFlows = [1000, 2000, 3000],
            LowerBoundRate = 1,
            UpperBoundRate = 5,
            Increment = 1
        };

        var expectedResults = new List<NpvResult>
            {
                new () { DiscountRate = 1, NpvValue = 5000 },
                new () { DiscountRate = 2, NpvValue = 4800 }
            };

        _npvCalculatorServiceMock
            .Setup(s => s.CalculateNpv(request.CashFlows, request.LowerBoundRate, request.UpperBoundRate, request.Increment, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResults);

        // Act
        var result = await _controller.CalculateNPV(request, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var npvResults = Assert.IsAssignableFrom<IEnumerable<NpvResult>>(okResult.Value);
        Assert.Equal(expectedResults.Count, npvResults.Count());
    }

    [Fact]
    public async Task CalculateNPV_NullCashFlows_ReturnsBadRequest()
    {
        var request = new NpvRequest
        {
            CashFlows = [],
            LowerBoundRate = 1,
            UpperBoundRate = 5,
            Increment = 1
        };

        var result = await _controller.CalculateNPV(request, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Cash flows are required.", badRequest.Value);
    }

    [Fact]
    public async Task CalculateNPV_EmptyCashFlows_ReturnsBadRequest()
    {
        var request = new NpvRequest
        {
            CashFlows = [],
            LowerBoundRate = 1,
            UpperBoundRate = 5,
            Increment = 1
        };

        var result = await _controller.CalculateNPV(request, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Cash flows are required.", badRequest.Value);
    }

    [Fact]
    public async Task CalculateNPV_LowerBoundGreaterThanUpperBound_ReturnsBadRequest()
    {
        var request = new NpvRequest
        {
            CashFlows = [1000, 2000],
            LowerBoundRate = 10,
            UpperBoundRate = 5,
            Increment = 1
        };

        var result = await _controller.CalculateNPV(request, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Lower bound cannot exceed upper bound.", badRequest.Value);
    }

    [Theory]
    [InlineData(-1, 5)]
    public async Task CalculateNPV_InvalidBounds_ReturnsBadRequest(double lower, double upper)
    {
        var request = new NpvRequest
        {
            CashFlows = [1000, 2000],
            LowerBoundRate = lower,
            UpperBoundRate = upper,
            Increment = 1
        };

        var result = await _controller.CalculateNPV(request, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid lower bound rate.", badRequest.Value);
    }
}
