namespace AGR_PanelOptimizer.Core.Models;

public class PanelCut
{
    public int StartPosition { get; init; }

    public int Length { get; init; }

    public bool IsBlank { get; init; }

    public Blank? Blank { get; init; }
}