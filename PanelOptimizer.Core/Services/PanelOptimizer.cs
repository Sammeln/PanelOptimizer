using PanelOptimizer.Core.Interfaces;
using PanelOptimizer.Core.Models;

namespace PanelOptimizer.Core.Services;

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