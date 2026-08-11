using AGR_PanelOptimizer.Core.Models;

namespace AGR_PanelOptimizer.Core.Services;

public class BlankCutter
{
    public IReadOnlyList<Blank> Cut(
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
                Length = blankLength,
                Height = panel.Height,
                SourcePanelPosition = i * blankLength
            });
        }

        return blanks;
    }
}