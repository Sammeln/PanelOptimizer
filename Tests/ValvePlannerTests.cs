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
