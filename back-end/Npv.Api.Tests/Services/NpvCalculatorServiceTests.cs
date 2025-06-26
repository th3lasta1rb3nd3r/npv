using Npv.Api.Services;

namespace Npv.Api.Tests.Services;

public class NpvCalculatorServiceTests
{
    private readonly NpvCalculatorService _service = new();

    [Fact]
    public async Task CalculateNpv_ShouldReturnCorrectRates_ForGivenScenario()
    {
        // Arrange
        var cashFlows = new double[] { 1000, 2000, 3000, 4000 };
        double lowerRate = 1.00;
        double upperRate = 15.00;
        double increment = 0.25;

        // Act
        var result = await _service.CalculateNpv(cashFlows, lowerRate, upperRate, increment, CancellationToken.None);
        var npvs = result.ToList();

        // Assert
        int expectedCount = (int)((upperRate - lowerRate) / increment) + 1;
        Assert.Equal(expectedCount, npvs.Count);

        // Check that discount rates are correct
        double expectedRate = lowerRate;
        foreach (var npvResult in result)
        {
            Assert.Equal(Math.Round(expectedRate, 2), npvResult.DiscountRate);
            expectedRate += increment;
        }
    }

    [Fact]
    public async Task CalculateNpv_ValidInput_ReturnsExpectedResults()
    {
        var cashFlows = new double[] { 1000, 2000, 3000 };
        double lowerRate = 5;
        double upperRate = 7;
        double increment = 1;

        var result = await _service.CalculateNpv(cashFlows, lowerRate, upperRate, increment, CancellationToken.None);
        var npvs = result.ToList();

        Assert.Equal(3, npvs.Count);
        Assert.Equal(5, npvs[0].DiscountRate);
        Assert.Equal(6, npvs[1].DiscountRate);
        Assert.Equal(7, npvs[2].DiscountRate);
    }

    [Fact]
    public async Task CalculateNpv_LowerBoundEqualsUpperBound_ReturnsSingleResult()
    {
        var cashFlows = new double[] { 1000, 2000, 3000 };
        double rate = 5;

        var result = await _service.CalculateNpv(cashFlows, rate, rate, 1, CancellationToken.None);
        var npvs = result.ToList();

        Assert.Single(npvs);
        Assert.Equal(rate, npvs[0].DiscountRate);
    }

    [Fact]
    public async Task CalculateNpv_IncrementLargerThanRange_ReturnsSingleResult()
    {
        var cashFlows = new double[] { 1000, 2000, 3000 };
        double lowerRate = 5;
        double upperRate = 7;
        double increment = 10;

        var result = await _service.CalculateNpv(cashFlows, lowerRate, upperRate, increment, CancellationToken.None);
        var npvs = result.ToList();

        Assert.Single(npvs);
        Assert.Equal(lowerRate, npvs[0].DiscountRate);
    }

    [Fact]
    public async Task CalculateNpv_NegativeCashFlows_CalculatesNPV()
    {
        var cashFlows = new double[] { -1000, 2000, -3000 };
        double lowerRate = 5;
        double upperRate = 5;
        double increment = 1;

        var result = await _service.CalculateNpv(cashFlows, lowerRate, upperRate, increment, CancellationToken.None);
        var npvs = result.ToList();

        Assert.Single(npvs);
        Assert.Equal(5, npvs[0].DiscountRate);
    }


    [Fact]
    public async Task CalculateNpv_EmptyCashFlows_ReturnsZeroNPV()
    {
        var cashFlows = Array.Empty<double>();
        double lowerRate = 5;
        double upperRate = 7;
        double increment = 1;

        var result = await _service.CalculateNpv(cashFlows, lowerRate, upperRate, increment, CancellationToken.None);
        var npvs = result.ToList();

        Assert.Equal(3, npvs.Count);
        Assert.All(npvs, r => Assert.Equal(0, r.NpvValue));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task CalculateNpv_InvalidIncrement_ReturnEmpty(double increment)
    {
        var cashFlows = new double[] { 1000, 2000, 3000 };

        var result = await _service.CalculateNpv(cashFlows, 0, 0, increment, CancellationToken.None);
        Assert.Empty(result);
    }

    [Fact]
    public async Task CalculateNpv_RespectsCancellationToken_ThrowsOperationCanceledException()
    {
        var cashFlows = new double[] { 1000, 2000, 3000 };
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await foreach (var _ in ToAsyncEnumerable(_service.CalculateNpv(cashFlows, 5, 7, 1, cts.Token)))
            {
                // Force enumeration to trigger cancellation
            }
        });
    }

    /// <summary>
    /// ToAsyncEnumerable
    /// </summary>
    private static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(Task<IEnumerable<T>> task)
    {
        var list = await task;
        foreach (var item in list)
        {
            yield return item;
        }
    }
}
