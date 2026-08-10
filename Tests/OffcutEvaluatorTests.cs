using AGR_PanelOptimizer.Core.Models;
using AGR_PanelOptimizer.Core.Services;

namespace AGR_PanelOptimizer.Core.Tests;

public class OffcutEvaluatorTests
{
    [Fact]
    public void Offcut_Exactly_300mm_Is_Usable()
    {
        var piece = new Piece
        {
            Length = 300
        };

        var evaluator = new OffcutEvaluator();

        var result = evaluator.IsUsable(piece, 300);

        Assert.True(result);
    }
    [Fact]
    public void Offcut_Smaller_Than_300mm_Is_Not_Usable()
    {
        var piece = new Piece
        {
            Length = 299
        };

        var evaluator = new OffcutEvaluator();

        var result = evaluator.IsUsable(piece, 300);

        Assert.False(result);
    }
    [Fact]
    public void Offcut_400mm_Is_Usable()
    {
        var piece = new Piece
        {
            Length = 400
        };

        var evaluator = new OffcutEvaluator();

        var result = evaluator.IsUsable(piece, 300);

        Assert.True(result);
    }
    [Fact]
    public void Offcut_20mm_Is_Not_Usable()
    {
        var piece = new Piece
        {
            Length = 20
        };

        var evaluator = new OffcutEvaluator();

        var result = evaluator.IsUsable(piece, 300);

        Assert.False(result);
    }
}