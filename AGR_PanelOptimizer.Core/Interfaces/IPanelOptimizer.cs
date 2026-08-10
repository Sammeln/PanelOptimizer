using AGR_PanelOptimizer.Core.Models;

namespace AGR_PanelOptimizer.Core.Interfaces;

public interface IPanelOptimizer
{
    OptimizationResult Calculate(
        PanelSettings settings,
        IReadOnlyCollection<ValveOrder> orders);
}