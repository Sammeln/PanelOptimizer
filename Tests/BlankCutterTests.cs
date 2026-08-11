using AGR_PanelOptimizer.Core.Enums;
using AGR_PanelOptimizer.Core.Models;
using AGR_PanelOptimizer.Core.Services;

namespace AGR_PanelOptimizer.Core.Tests;

public class BlankCutterTests
{
    [Fact]
    public void Cut_6000mm_Panel_Into_1280mm_Blanks()
    {
        var panel = new Panel
        {
            Length = 6000,
            Height = 1200
        };

        var cutter = new BlankCutter();

        var result = cutter.Cut(panel, 1280);

        Assert.Equal(6000, result.PanelLength);
        Assert.Equal(1200, result.PanelHeight);

        Assert.Equal(5, result.Cuts.Count);

        Assert.All(
            result.Cuts.Take(4),
            cut =>
            {
                Assert.True(cut.IsBlank);
                Assert.NotNull(cut.Blank);
                Assert.Equal(1280, cut.Length);
            });

        var remainder = result.Cuts[4];

        Assert.False(remainder.IsBlank);
        Assert.Null(remainder.Blank);
        Assert.Equal(5120, remainder.StartPosition);
        Assert.Equal(880, remainder.Length);
    }
    [Fact]
    public void Cut_Stores_Correct_Start_Positions()
    {
        var panel = new Panel
        {
            Length = 6000,
            Height = 1200
        };

        var cutter = new BlankCutter();

        var result = cutter.Cut(panel, 1280);

        Assert.Equal(0, result.Cuts[0].StartPosition);
        Assert.Equal(1280, result.Cuts[1].StartPosition);
        Assert.Equal(2560, result.Cuts[2].StartPosition);
        Assert.Equal(3840, result.Cuts[3].StartPosition);
        Assert.Equal(5120, result.Cuts[4].StartPosition);
    }
    [Fact]
    public void Cut_4500mm_Panel_Into_1280mm_Blanks()
    {
        var panel = new Panel
        {
            Length = 4500,
            Height = 1200
        };

        var cutter = new BlankCutter();

        var result = cutter.Cut(panel, 1280);

        Assert.Equal(4, result.Cuts.Count);

        Assert.Equal(0, result.Cuts[0].StartPosition);
        Assert.Equal(1280, result.Cuts[1].StartPosition);
        Assert.Equal(2560, result.Cuts[2].StartPosition);

        Assert.Equal(3840, result.Cuts[3].StartPosition);
        Assert.Equal(660, result.Cuts[3].Length);
        Assert.False(result.Cuts[3].IsBlank);
    }
    [Fact]
    public void Cut_6000mm_Panel_Into_1280mm_Blanks_Leaves_880mm()
    {
        var panel = new Panel
        {
            Length = 6000,
            Height = 1200
        };

        var cutter = new BlankCutter();

        var result = cutter.Cut(panel, 1280);

        Assert.Equal(4, result.Cuts.Count);
        Assert.Equal(880, result.RemainingLength);
    }
    [Fact]
    public void Cut_4500mm_Panel_Into_1280mm_Blanks_Leaves_660mm()
    {
        var panel = new Panel
        {
            Length = 4500,
            Height = 1200
        };

        var cutter = new BlankCutter();

        var result = cutter.Cut(panel, 1280);

        Assert.Equal(3, result.Blanks.Count);
        Assert.Equal(660, result.RemainingLength);
    }
    [Fact]
    public void Can_Prepare_1280x2380_Valve_From_6000mm_Panel()
    {
        var panel = new Panel
        {
            Length = 6000,
            Height = 1200
        };

        var blankCutter = new BlankCutter();

        var panelResult = blankCutter.Cut(
            panel,
            blankLength: 1280);

        Assert.Equal(4, panelResult.Blanks.Count);
        Assert.Equal(880, panelResult.RemainingLength);

        var firstBlank = panelResult.Blanks[0];

        var piece = new Piece
        {
            Length = 1200,
            Height = 1280,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Groove
        };

        var cutter = new PieceCutter();

        var (requiredPiece, remainingPiece) =
            cutter.Cut(piece, 1180);

        Assert.Equal(1180, requiredPiece.Length);
        Assert.Equal(20, remainingPiece.Length);

        var assembler = new ValveAssembler();

        //var valve = assembler.Assemble(
        //    height: 1280,
        //    width: 2380,
        //    new[]
        //    {
        //    firstBlank.ToPiece(), // пока так сделать нельзя
        //    requiredPiece
        //    });
    }
}