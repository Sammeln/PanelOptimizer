using AGR_PanelOptimizer.Core.Enums;
using AGR_PanelOptimizer.Core.Models;

namespace AGR_PanelOptimizer.Core.Services;

public class BlankCutter
{
    public PanelCutResult Cut(
        Panel panel,
        int blankHeight)
    {
        ArgumentNullException.ThrowIfNull(panel);

        if (blankHeight <= 0)
            throw new ArgumentOutOfRangeException(nameof(blankHeight));

        var blankCount = panel.Length / blankHeight;

        var cuts = new List<PanelCut>();

        for (var i = 0; i < blankCount; i++)
        {
            var blank = new Blank
            {
                Height = blankHeight,
                Length = panel.Height,
                LeftEdge = EdgeType.Tongue,
                RightEdge = EdgeType.Groove,
                SourcePanelPosition = i * blankHeight
            };

            cuts.Add(new PanelCut
            {
                StartPosition = i * blankHeight,
                Length = blankHeight,
                IsBlank = true,
                Blank = blank
            });
        }

        var usedLength = blankCount * blankHeight;
        var remainingLength = panel.Length - usedLength;

        if (remainingLength > 0)
        {
            cuts.Add(new PanelCut
            {
                StartPosition = usedLength,
                Length = remainingLength,
                IsBlank = false
            });
        }

        return new PanelCutResult
        {
            PanelLength = panel.Length,
            PanelHeight = panel.Height,
            Cuts = cuts
        };
    }
}