namespace AGR_PanelOptimizer.Core.Models;

public class PanelCutPlan
{
    public int PanelIndex { get; init; }

    public int PanelLength { get; init; }

    public int PanelHeight { get; init; }

    public IReadOnlyList<Blank> Blanks { get; init; } =
        Array.Empty<Blank>();

    public int RemainingLength { get; init; }
}