using AGR_PanelOptimizer.Core.Models;

namespace AGR_PanelOptimizer.Core.Services;

public class BlankCutter
{
    public PanelCutResult Cut(
        Panel panel,
        int blankLength)
    {
        ArgumentNullException.ThrowIfNull(panel);

        if (blankLength <= 0)
            throw new ArgumentOutOfRangeException(nameof(blankLength));

        var count = panel.Length / blankLength;

        var blanks = new List<Blank>(count);

        for (var i = 0; i < count; i++)
        {
            blanks.Add(new Blank
            {
                Height = blankLength,
                Length= panel.Height,
                LeftEdge = Enums.EdgeType.Tongue,
                RightEdge = Enums.EdgeType.Groove,
                SourcePanelPosition = i * blankLength
            });
        }

        var remainingLength =
            panel.Length - count * blankLength;

        return new PanelCutResult
        {
            Blanks = blanks,
            RemainingLength = remainingLength
        };
    }
}