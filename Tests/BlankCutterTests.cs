using AGR_PanelOptimizer.Core.Models;
using AGR_PanelOptimizer.Core.Services;

namespace AGR_PanelOptimizer.Core.Tests;

public class BlankCutterTests
{
    [Fact]
    public void Cut_6000mm_Panel_Into_1280mm_Blanks_Returns_4_Blanks()
    {
        var panel = new Panel
        {
            Length = 6000,
            Height = 1200
        };

        var cutter = new BlankCutter();

        var blanks = cutter.Cut(
            panel,
            blankLength: 1280);

        Assert.Equal(4, blanks.Count);

        Assert.All(
            blanks,
            blank =>
            {
                Assert.Equal(1280, blank.Length);
                Assert.Equal(1200, blank.Height);
            });
    }
    [Fact]
    public void Blanks_Have_Correct_Source_Positions()
    {
        var panel = new Panel
        {
            Length = 6000,
            Height = 1200
        };

        var cutter = new BlankCutter();

        var blanks = cutter.Cut(panel, 1280);

        Assert.Equal(0, blanks[0].SourcePanelPosition);
        Assert.Equal(1280, blanks[1].SourcePanelPosition);
        Assert.Equal(2560, blanks[2].SourcePanelPosition);
        Assert.Equal(3840, blanks[3].SourcePanelPosition);
    }
    [Fact]
    public void Cut_4500mm_Panel_Into_1280mm_Blanks_Returns_3_Blanks()
    {
        var panel = new Panel
        {
            Length = 4500,
            Height = 1200
        };

        var cutter = new BlankCutter();

        var blanks = cutter.Cut(panel, 1280);

        Assert.Equal(3, blanks.Count);
    }
}