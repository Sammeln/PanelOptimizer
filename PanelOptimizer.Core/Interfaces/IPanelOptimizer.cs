using PanelOptimizer.Core.Models;

namespace PanelOptimizer.Core.Interfaces;

public interface IPanelOptimizer
{
    OptimizationResult Calculate(
        PanelSettings settings,
        IReadOnlyCollection<ValveOrder> orders);
}