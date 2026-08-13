using AGR_PanelOptimizer.Core.Models;
using AGR_PanelOptimizer.Core.Services;

namespace AGR_PanelOptimizer.Core.Tests;

public class OrderOffcutEvaluatorTests
{
    [Fact]
    public void PanelOffcut_880mm_Is_Usable_When_OrderContains880mmValve()
    {
        var offcut = new PanelOffcut
        {
            Length = 1200,
            Height = 880,
            SourcePanelPosition = 5120
        };

        var orders = new[]
        {
            new ValveOrder { Height = 1280, Width = 2000, Quantity = 1 },
            new ValveOrder { Height = 880, Width = 2000, Quantity = 1 }
        };

        var evaluator = new OffcutEvaluator();

        var result = evaluator.IsUsable(offcut, orders);

        Assert.True(result);
    }

    [Fact]
    public void PanelOffcut_880mm_Is_Waste_When_OrderHasNo880mmValve()
    {
        var offcut = new PanelOffcut
        {
            Length = 1200,
            Height = 880,
            SourcePanelPosition = 5120
        };

        var orders = new[]
        {
            new ValveOrder { Height = 1200, Width = 2000, Quantity = 1 },
            new ValveOrder { Height = 1220, Width = 2000, Quantity = 1 },
            new ValveOrder { Height = 1280, Width = 2000, Quantity = 1 }
        };

        var evaluator = new OffcutEvaluator();

        var result = evaluator.IsUsable(offcut, orders);

        Assert.False(result);
    }

    [Fact]
    public void PanelOffcut_Is_Not_Usable_When_Only_Different_Height_Is_Ordered()
    {
        var offcut = new PanelOffcut
        {
            Length = 1200,
            Height = 880,
            SourcePanelPosition = 5120
        };

        var orders = new[]
        {
            new ValveOrder { Height = 900, Width = 2000, Quantity = 1 }
        };

        var evaluator = new OffcutEvaluator();

        var result = evaluator.IsUsable(offcut, orders);

        Assert.False(result);
    }
}
