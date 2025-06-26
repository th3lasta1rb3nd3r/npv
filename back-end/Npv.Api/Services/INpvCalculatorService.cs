using Npv.Api.Results;

namespace Npv.Api.Services;


/// <summary>
/// INpvCalculatorService
/// </summary>
public interface INpvCalculatorService
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
    public Task<IEnumerable<NpvResult>> CalculateNpv(IEnumerable<double> cashFlows, double lowerRate, double upperRate, double increment, CancellationToken cancellationToken);
}