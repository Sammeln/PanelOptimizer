using AGR_PanelOptimizer.Core.Enums;
using AGR_PanelOptimizer.Core.Models;
using AGR_PanelOptimizer.Core.Services;

namespace AGR_PanelOptimizer.Core.Tests;

public class PanelCutterTests
{
    [Fact]
    public void Cut_6000x1200_Panel_Into_1280_Blanks()
    {
        var panel = new Panel
        {
            Length = 6000,
            Height = 1200
        };

        PanelCutter cutter = new();

        var result = cutter.Cut(panel, 1280);

        Assert.Equal(4, result.Blanks.Count);

        foreach (var blank in result.Blanks)
        {
            Assert.Equal(1200, blank.Length);
            Assert.Equal(1280, blank.Height);
            Assert.Equal(EdgeType.Tongue, blank.LeftEdge);
            Assert.Equal(EdgeType.Groove, blank.RightEdge);
        }

        Assert.Single(result.Offcuts);

        var offcut = result.Offcuts[0];

        Assert.Equal(1200, offcut.Length);
        Assert.Equal(880, offcut.Height);
        Assert.Equal(4, offcut.SourcePanelPosition);
    }

    [Fact]
    public void Cut_6000x1200_Panel_Without_Remainder()
    {
        var panel = new Panel
        {
            Length = 6000,
            Height = 1200
        };

        PanelCutter cutter = new();

        var result = cutter.Cut(panel, 1200);

        Assert.Equal(5, result.Blanks.Count);
        Assert.Empty(result.Offcuts);
    }
}
