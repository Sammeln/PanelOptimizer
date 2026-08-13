namespace AGR_PanelOptimizer.Core.Models;

public class OptimizationResult
{
    public int RequiredPanels { get; init; }

    public IReadOnlyList<ValveAssemblyResult> Valves { get; init; } = [];

    public CuttingPlan CuttingPlan { get; init; } = new();

    public IReadOnlyList<PanelOffcut> Waste { get; init; } = [];
}
