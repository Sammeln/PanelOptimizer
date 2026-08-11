namespace AGR_PanelOptimizer.Core.Models;

public class PanelCutResult
{
    public IReadOnlyList<PanelCut> Cuts { get; init; } =
        Array.Empty<PanelCut>();

    public int PanelLength { get; init; }

    public int PanelHeight { get; init; }
}