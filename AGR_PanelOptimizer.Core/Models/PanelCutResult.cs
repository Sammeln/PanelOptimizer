namespace AGR_PanelOptimizer.Core.Models;

public class PanelCutResult
{
    public IReadOnlyList<Blank> Blanks { get; init; } =
        Array.Empty<Blank>();

    public int RemainingLength { get; init; }
}