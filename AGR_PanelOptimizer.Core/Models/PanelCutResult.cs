namespace AGR_PanelOptimizer.Core.Models;

public class PanelCutResult
{
    public IReadOnlyList<Blank> Blanks { get; init; } = Array.Empty<Blank>();

    public IReadOnlyList<PanelOffcut> Offcuts { get; init; } = Array.Empty<PanelOffcut>();
}
