using AGR_PanelOptimizer.Core.Enums;
using AGR_PanelOptimizer.Core.Models;

namespace AGR_PanelOptimizer.Core.Services;

public class PanelCutter
{
    public PanelCutter()
    {
        
    }
    public PanelCutResult Cut(Panel panel, int blankHeight)
    {
        if (blankHeight <= 0)
            throw new ArgumentOutOfRangeException(nameof(blankHeight));

        if (blankHeight > panel.Length)
            throw new ArgumentException("Высота заготовки не может быть больше длины панели.", nameof(blankHeight));

        var blankCount = panel.Length / blankHeight;
        var remainingLength = panel.Length % blankHeight;

        var blanks = Enumerable.Range(0, blankCount)
            .Select(position => new Blank
            {
                Length = panel.Height,
                Height = blankHeight,
                LeftEdge = EdgeType.Tongue,
                RightEdge = EdgeType.Groove,
                SourcePanelPosition = position
            })
            .ToArray();

        var offcuts = remainingLength == 0
            ? Array.Empty<PanelOffcut>()
            : new[]
            {
                new PanelOffcut
                {
                    Length = panel.Height,
                    Height = remainingLength,
                    SourcePanelPosition = blankCount
                }
            };

        return new PanelCutResult
        {
            Blanks = blanks,
            Offcuts = offcuts
        };
    }
}
