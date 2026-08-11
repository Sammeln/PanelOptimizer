using AGR_PanelOptimizer.Core.Enums;
using AGR_PanelOptimizer.Core.Models;
using AGR_PanelOptimizer.Core.Services;

namespace AGR_PanelOptimizer.Core.Tests;

public class BlankPieceCutterTests
{
    [Fact]
    public void Cut_Whole_Blank_Creates_Piece_Without_Remaining_Piece()
    {
        var blank = new Blank
        {
            Length = 1200,
            Height = 1280,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Groove
        };

        var cutter = new BlankPieceCutter();

        var (piece, remaining) =
            cutter.Cut(blank, 1200);

        Assert.Equal(1200, piece.Length);
        Assert.Equal(1280, piece.Height);

        Assert.Equal(EdgeType.Tongue, piece.LeftEdge);
        Assert.Equal(EdgeType.Groove, piece.RightEdge);

        Assert.Null(remaining);
    }
    [Fact]
    public void Cut_1180_From_1200_Creates_20mm_Remaining_Piece()
    {
        var blank = new Blank
        {
            Length = 1200,
            Height = 1280,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Groove
        };

        var cutter = new BlankPieceCutter();

        var (piece, remaining) =
            cutter.Cut(blank, 1180);

        Assert.Equal(1180, piece.Length);
        Assert.Equal(1280, piece.Height);

        Assert.Equal(EdgeType.Tongue, piece.LeftEdge);
        Assert.Equal(EdgeType.Cut, piece.RightEdge);

        Assert.NotNull(remaining);

        Assert.Equal(20, remaining.Length);
        Assert.Equal(1280, remaining.Height);

        Assert.Equal(EdgeType.Cut, remaining.LeftEdge);
        Assert.Equal(EdgeType.Groove, remaining.RightEdge);
    }
    [Fact]
    public void Cut_Whole_Offcut_Preserves_Cut_And_Groove_Edges()
    {
        var blank = new Blank
        {
            Length = 400,
            Height = 1280,
            LeftEdge = EdgeType.Cut,
            RightEdge = EdgeType.Groove
        };

        var cutter = new BlankPieceCutter();

        var (piece, remaining) =
            cutter.Cut(blank, 400);

        Assert.Equal(400, piece.Length);

        Assert.Equal(EdgeType.Cut, piece.LeftEdge);
        Assert.Equal(EdgeType.Groove, piece.RightEdge);

        Assert.Null(remaining);
    }
    [Fact]
    public void Cut_Preserves_Blank_Height()
    {
        var blank = new Blank
        {
            Length = 1200,
            Height = 1350,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Groove
        };

        var cutter = new BlankPieceCutter();

        var (piece, remaining) =
            cutter.Cut(blank, 800);

        Assert.Equal(1350, piece.Height);
        Assert.Equal(1350, remaining!.Height);
    }
}