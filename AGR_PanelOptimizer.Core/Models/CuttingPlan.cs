namespace AGR_PanelOptimizer.Core.Models;

public class CuttingPlan
{
    public IReadOnlyList<PanelCutPlan> Panels { get; init; } =
        Array.Empty<PanelCutPlan>();
}