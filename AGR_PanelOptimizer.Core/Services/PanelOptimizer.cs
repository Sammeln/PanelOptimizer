using AGR_PanelOptimizer.Core.Interfaces;
using AGR_PanelOptimizer.Core.Models;

namespace AGR_PanelOptimizer.Core.Services;

public class PanelOptimizer : IPanelOptimizer
{
    public OptimizationResult Calculate(
        PanelSettings settings,
        IReadOnlyCollection<ValveOrder> orders)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(orders);

        return new OptimizationResult();
    }
}