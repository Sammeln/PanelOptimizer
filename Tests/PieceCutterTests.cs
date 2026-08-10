using AGR_PanelOptimizer.Core.Enums;
using AGR_PanelOptimizer.Core.Models;
using AGR_PanelOptimizer.Core.Services;

namespace AGR_PanelOptimizer.Core.Tests;

public class PieceCutterTests
{
    [Fact]
    public void Cut_800_From_1200_Creates_800_And_400_Pieces()
    {
        var source = new Piece
        {
            Length = 1200,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Groove
        };

        var cutter = new PieceCutter();

        var (requiredPiece, remainingPiece) =
            cutter.Cut(source, 800);

        Assert.Equal(800, requiredPiece.Length);
        Assert.Equal(400, remainingPiece.Length);

        Assert.Equal(EdgeType.Tongue, requiredPiece.LeftEdge);
        Assert.Equal(EdgeType.Cut, requiredPiece.RightEdge);

        Assert.Equal(EdgeType.Cut, remainingPiece.LeftEdge);
        Assert.Equal(EdgeType.Groove, remainingPiece.RightEdge);
    }

    [Fact]
    public void Cut_1180_From_1200_Creates_20mm_Remaining_Piece()
    {
        var source = new Piece
        {
            Length = 1200,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Groove
        };

        var cutter = new PieceCutter();

        var (requiredPiece, remainingPiece) =
            cutter.Cut(source, 1180);

        Assert.Equal(1180, requiredPiece.Length);
        Assert.Equal(20, remainingPiece.Length);

        Assert.Equal(EdgeType.Tongue, requiredPiece.LeftEdge);
        Assert.Equal(EdgeType.Cut, requiredPiece.RightEdge);

        Assert.Equal(EdgeType.Cut, remainingPiece.LeftEdge);
        Assert.Equal(EdgeType.Groove, remainingPiece.RightEdge);
    }
    public void Cut_Equal_To_Source_Length_Throws()
    {
        var source = new Piece
        {
            Length = 1200,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Groove
        };

        var cutter = new PieceCutter();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            cutter.Cut(source, 1200));
    }
    [Fact]
    public void Cut_Larger_Than_Source_Length_Throws()
    {
        var source = new Piece
        {
            Length = 1200,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Groove
        };

        var cutter = new PieceCutter();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            cutter.Cut(source, 1201));
    }
}