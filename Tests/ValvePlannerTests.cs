using AGR_PanelOptimizer.Core.Enums;
using AGR_PanelOptimizer.Core.Models;
using AGR_PanelOptimizer.Core.Services;
using Xunit;

namespace AGR_PanelOptimizer.Core.Tests;

public class ValvePlannerTests
{
    [Fact]
    public void CreateValve_ShouldCreateValveFromTwoBlanks()
    {
        var order = new ValveOrder
        {
            Height = 1280,
            Width = 2380,
            Quantity = 1
        };

        var blanks = new[]
        {
            new Blank
            {
                Height = 1280,
                Length = 1200,
                LeftEdge = EdgeType.Tongue,
                RightEdge = EdgeType.Groove
            },
            new Blank
            {
                Height = 1280,
                Length = 1200,
                LeftEdge = EdgeType.Tongue,
                RightEdge = EdgeType.Groove
            }
        };

        var planner = new ValvePlanner(
            new BlankPieceCutter(),
            new ValveAssembler());

        var result = planner.CreateValve(
            order,
            blanks,
            minimumOffcut: 300);

        Assert.NotNull(result.Valve);

        Assert.Equal(1280, result.Valve.Height);
        Assert.Equal(2380, result.Valve.Width);

        Assert.Equal(2, result.Valve.Pieces.Count);

        Assert.Equal(1200, result.Valve.Pieces[0].Length);
        Assert.Equal(1180, result.Valve.Pieces[1].Length);

        Assert.Equal(EdgeType.Tongue, result.Valve.Pieces[0].LeftEdge);
        Assert.Equal(EdgeType.Groove, result.Valve.Pieces[0].RightEdge);

        Assert.Equal(EdgeType.Tongue, result.Valve.Pieces[1].LeftEdge);
        Assert.Equal(EdgeType.Cut, result.Valve.Pieces[1].RightEdge);
    }

    [Fact]
    public void CreateValve_ShouldReuseOffcutWithCorrectBlankOrientation()
    {
        var order = new ValveOrder
        {
            Height = 1280,
            Width = 3000,
            Quantity = 2
        };

        var materialPool = new MaterialPool();

        materialPool.Add(new Blank
        {
            Height = 1280,
            Length = 1200,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Groove
        });

        materialPool.Add(new Blank
        {
            Height = 1280,
            Length = 1200,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Groove
        });

        materialPool.Add(new Blank
        {
            Height = 1280,
            Length = 1200,
            LeftEdge = EdgeType.Tongue,
            RightEdge = EdgeType.Groove
        });

        var planner = new ValvePlanner(
            new BlankPieceCutter(),
            new ValveAssembler());

        var firstValve = planner.CreateValve(
            order,
            materialPool,
            minimumOffcut: 300);

        Assert.Equal(3000, firstValve.Valve.Width);
        Assert.Equal(3, firstValve.Valve.Pieces.Count);
        Assert.Equal(1200, firstValve.Valve.Pieces[0].Length);
        Assert.Equal(1200, firstValve.Valve.Pieces[1].Length);
        Assert.Equal(600, firstValve.Valve.Pieces[2].Length);

        Assert.Single(materialPool.Blanks);
        Assert.Equal(600, materialPool.Blanks[0].Length);
        Assert.Equal(1280, materialPool.Blanks[0].Height);
        Assert.Equal(EdgeType.Cut, materialPool.Blanks[0].LeftEdge);
        Assert.Equal(EdgeType.Groove, materialPool.Blanks[0].RightEdge);

        var secondValve = planner.CreateValve(
            order,
            materialPool,
            minimumOffcut: 300);

        Assert.Equal(3000, secondValve.Valve.Width);
        Assert.Equal(3, secondValve.Valve.Pieces.Count);
        Assert.Equal(600, secondValve.Valve.Pieces[0].Length);
        Assert.Equal(1200, secondValve.Valve.Pieces[1].Length);
        Assert.Equal(1200, secondValve.Valve.Pieces[2].Length);

        Assert.Empty(materialPool.Blanks);
    }

    [Fact]
    public void CreateValve_ShouldThrow_WhenThereIsNotEnoughMaterial()
    {
        var order = new ValveOrder
        {
            Height = 1280,
            Width = 2380,
            Quantity = 1
        };

        var blanks = new[]
        {
            new Blank
            {
                Height = 1280,
                Length = 1200,
                LeftEdge = EdgeType.Tongue,
                RightEdge = EdgeType.Groove
            }
        };

        var planner = new ValvePlanner(
            new BlankPieceCutter(),
            new ValveAssembler());

        Assert.Throws<InvalidOperationException>(() =>
            planner.CreateValve(
                order,
                blanks,
                minimumOffcut: 300));
    }
}
