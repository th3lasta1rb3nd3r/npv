using Npv.Api.Results;

namespace Npv.Api.Services;

/// <summary>
/// NpvCalculatorService
/// </summary>
public class NpvCalculatorService : INpvCalculatorService
{
    /// <summary>
    /// CalculateNpv
    /// </summary>
    /// <param name="cashFlows">double[]</param>
    /// <param name="lowerRate">double</param>
    /// <param name="upperRate">double</param>
    /// <param name="increment">double</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>NpvResult[]</returns>
    public async Task<IEnumerable<NpvResult>> CalculateNpv(
        IEnumerable<double> cashFlows,
        double lowerRate,
        double upperRate,
        double increment,
        CancellationToken cancellationToken)
    {
        if (increment <= 0)
        {
            return [];
        }

        var results = new List<NpvResult>();

        for (double rate = lowerRate; rate <= upperRate; rate += increment)
        {
            cancellationToken.ThrowIfCancellationRequested();

            double npv = 0;
            int period = 1;
            foreach (var cashFlow in cashFlows)
            {
                npv += cashFlow / Math.Pow(1 + (rate / 100), period);
                period++;
            }

            results.Add(new()
            {
                DiscountRate = Math.Round(rate, 2),
                NpvValue = Math.Round(npv, 2)
            });
        }

        //to handle async
        await Task.CompletedTask;

        return results;
    }
}