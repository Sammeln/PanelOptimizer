namespace AGR_PanelOptimizer.Core.Models;

public class OrderMaterialPlan
{
    public IReadOnlyList<Blank> Blanks { get; init; } = [];
    public IReadOnlyList<PanelOffcut> Waste { get; init; } = [];
}
