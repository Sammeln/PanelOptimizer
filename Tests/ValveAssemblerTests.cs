using AGR_PanelOptimizer.Core.Enums;
using AGR_PanelOptimizer.Core.Models;
using AGR_PanelOptimizer.Core.Services;

namespace AGR_PanelOptimizer.Core.Tests;

public class ValveAssemblerTests
{
    [Fact]
    public void Assemble_1280x2380_With_1200_And_1180_Pieces()
    {
        var pieces = new List<Piece>
        {
            new()
            {
                Length = 1200,
                LeftEdge = EdgeType.Tongue,
                RightEdge = EdgeType.Groove
            },
            new()
            {
                Length = 1180,
                LeftEdge = EdgeType.Tongue,
                RightEdge = EdgeType.Cut
            }
        };

        var assembler = new ValveAssembler();

        var result = assembler.Assemble(
            height: 1280,
            width: 2380,
            pieces);

        Assert.Equal(1280, result.Valve.Height);
        Assert.Equal(2380, result.Valve.Width);

        Assert.Equal(2, result.Valve.Pieces.Count);

        Assert.Equal(1200, result.Valve.Pieces[0].Length);
        Assert.Equal(1180, result.Valve.Pieces[1].Length);
    }
    [Fact]
    public void FirstPiece_May_Start_With_Cut_Edge()
    {
        var pieces = new List<Piece>
        {
        new()
        {
            Length = 400,
            LeftEdge = EdgeType.Cut,
            RightEdge = EdgeType.Groove
        },
        new()
        {
            Length = 1200,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Groove
        },
        new()
        {
            Length = 780,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Cut
        }
        };

        var assembler = new ValveAssembler();

        var result = assembler.Assemble(
            height: 1280,
            width: 2380,
            pieces);

        Assert.Equal(3, result.Valve.Pieces.Count);
    }
    [Fact]
    public void FirstPiece_Cannot_Start_With_Groove()
    {
        var pieces = new List<Piece>
    {
        new()
        {
            Length = 1200,
            LeftEdge = EdgeType.Groove,
            RightEdge = EdgeType.Groove
        },
        new()
        {
            Length = 1180,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Cut
        }
    };

        var assembler = new ValveAssembler();

        Assert.Throws<InvalidOperationException>(() =>
            assembler.Assemble(
                height: 1280,
                width: 2380,
                pieces));
    }
    [Fact]
    public void MiddlePiece_Cannot_Have_Cut_Right_Edge()
    {
        var pieces = new List<Piece>
    {
        new()
        {
            Length = 600,
            LeftEdge = EdgeType.Cut,
            RightEdge = EdgeType.Groove
        },
        new()
        {
            Length = 800,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Cut
        },
        new()
        {
            Length = 980,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Groove
        }
    };

        var assembler = new ValveAssembler();

        Assert.Throws<InvalidOperationException>(() =>
            assembler.Assemble(
                height: 1280,
                width: 2380,
                pieces));
    }
    [Fact]
    public void LastPiece_May_End_With_Cut_Edge()
    {
        var pieces = new List<Piece>
    {
        new()
        {
            Length = 1200,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Groove
        },
        new()
        {
            Length = 1180,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Cut
        }
    };

        var assembler = new ValveAssembler();

        var result = assembler.Assemble(
            height: 1280,
            width: 2380,
            pieces);

        Assert.Equal(EdgeType.Cut,
            result.Valve.Pieces[^1].RightEdge);
    }
    [Fact]
    public void Assemble_Throws_When_Pieces_Do_Not_Match_Valve_Width()
    {
        var pieces = new List<Piece>
    {
        new()
        {
            Length = 1200,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Groove
        },
        new()
        {
            Length = 1100,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Cut
        }
    };

        var assembler = new ValveAssembler();

        Assert.Throws<InvalidOperationException>(() =>
            assembler.Assemble(
                height: 1280,
                width: 2380,
                pieces));
    }
}