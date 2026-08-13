using AGR_PanelOptimizer.Core.Models;
using AGR_PanelOptimizer.Core.Services;

namespace AGR_PanelOptimizer.Core.Tests;

public class OrderMaterialPlannerTests
{
    [Fact]
    public void Prepare_OffcutHeightMatchesOrder_ConvertsOffcutToBlank()
    {
        var cutResult = new PanelCutter().Cut(
            new Panel
            {
                Length = 6000,
                Height = 1200
            },
            1280);

        var orders = new[]
        {
            new ValveOrder
            {
                Height = 1280,
                Width = 800,
                Quantity = 1
            },
            new ValveOrder
            {
                Height = 880,
                Width = 800,
                Quantity = 1
            }
        };

        var planner = new OrderMaterialPlanner();

        var result = planner.Prepare(cutResult, orders);

        Assert.Equal(5, result.Blanks.Count);
        Assert.Contains(result.Blanks, blank =>
            blank.Height == 880 &&
            blank.Length == 1200 &&
            blank.SourcePanelPosition == 4);
        Assert.Empty(result.Waste);
    }

    [Fact]
    public void Prepare_OffcutHeightDoesNotMatchOrder_LeavesOffcutAsWaste()
    {
        var cutResult = new PanelCutter().Cut(
            new Panel
            {
                Length = 6000,
                Height = 1200
            },
            1280);

        var orders = new[]
        {
            new ValveOrder
            {
                Height = 1200,
                Width = 800,
                Quantity = 1
            },
            new ValveOrder
            {
                Height = 1220,
                Width = 800,
                Quantity = 1
            },
            new ValveOrder
            {
                Height = 1280,
                Width = 800,
                Quantity = 1
            }
        };

        var planner = new OrderMaterialPlanner();

        var result = planner.Prepare(cutResult, orders);

        Assert.Equal(4, result.Blanks.Count);
        Assert.DoesNotContain(result.Blanks, blank => blank.Height == 880);

        Assert.Single(result.Waste);
        Assert.Equal(1200, result.Waste[0].Length);
        Assert.Equal(880, result.Waste[0].Height);
        Assert.Equal(4, result.Waste[0].SourcePanelPosition);
    }
}
