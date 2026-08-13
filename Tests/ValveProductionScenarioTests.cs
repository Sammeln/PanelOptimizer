using AGR_PanelOptimizer.Core.Enums;
using AGR_PanelOptimizer.Core.Models;
using AGR_PanelOptimizer.Core.Services;

namespace AGR_PanelOptimizer.Core.Tests;

public class ValveProductionScenarioTests
{
    [Fact]
    public void Produce_1280x2380_Valve_From_6000mm_Panel()
    {
        // Arrange

        var panel = new Panel
        {
            Length = 6000,
            Height = 1200
        };

        var blankCutter = new PanelCutter();

        var blankResult = blankCutter.Cut(
            panel,
            1280);

        // Act

        var blankPieceCutter = new BlankPieceCutter();

        var (piece1, remaining1) =
            blankPieceCutter.Cut(
                blankResult.Blanks[0],
                1200);

        var (piece2, remaining2) =
            blankPieceCutter.Cut(
                blankResult.Blanks[1],
                1180);

        // Assert

        Assert.Equal(4, blankResult.Blanks.Count);

        Assert.Equal(1200, piece1.Length);
        Assert.Equal(1280, piece1.Height);

        Assert.Equal(1180, piece2.Length);
        Assert.Equal(1280, piece2.Height);

        Assert.Null(remaining1);

        Assert.NotNull(remaining2);
        Assert.Equal(20, remaining2!.Length);

        Assert.Equal(EdgeType.Tongue, piece1.LeftEdge);
        Assert.Equal(EdgeType.Groove, piece1.RightEdge);

        Assert.Equal(EdgeType.Tongue, piece2.LeftEdge);
        Assert.Equal(EdgeType.Cut, piece2.RightEdge);

        Assert.Equal(EdgeType.Cut, remaining2.LeftEdge);
        Assert.Equal(EdgeType.Groove, remaining2.RightEdge);

        var assembler = new ValveAssembler();

        var result = assembler.Assemble(
            height: 1280,
            width: 2380,
            new[]
            {
                piece1,
                piece2
            });

        Assert.Equal(1280, result.Valve.Height);
        Assert.Equal(2380, result.Valve.Width);
        Assert.Equal(2, result.Valve.Pieces.Count);
        var offcutEvaluator = new OffcutEvaluator();

        Assert.False(
            !offcutEvaluator.IsUsable(remaining2!, remaining2.Length));
    }
}