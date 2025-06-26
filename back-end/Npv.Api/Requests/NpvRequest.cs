
namespace Npv.Api.Requests;


/// <summary>
/// NpvRequest
/// </summary>
public class NpvRequest
{
    /// <summary>
    /// CashFlows
    /// </summary>
    public IEnumerable<double> CashFlows { get; set; } = [];

    /// <summary>
    /// LowerBoundRate
    /// </summary>
    public double LowerBoundRate { get; set; }

    /// <summary>
    /// UpperBoundRate
    /// </summary>
    public double UpperBoundRate { get; set; }

    /// <summary>
    /// Increment
    /// </summary>
    public double Increment { get; set; }
}