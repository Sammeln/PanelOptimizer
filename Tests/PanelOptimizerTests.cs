using AGR_PanelOptimizer.Core.Models;
using AGR_PanelOptimizer.Core.Services;

namespace Tests;

public class PanelOptimizerTests
{
    [Fact]
    public void Calculate_UsesMatchingOffcutFromSamePanel()
    {
        var settings = new PanelSettings
        {
            PanelLength = 6000,
            PanelHeight = 1200,
            MinimumOffcut = 300
        };

        var orders = new[]
        {
            new ValveOrder
            {
                Height = 1280,
                Width = 1200,
                Quantity = 1
            },
            new ValveOrder
            {
                Height = 880,
                Width = 1200,
                Quantity = 1
            }
        };

        var optimizer = new PanelOptimizer(
            new PanelCutter(),
            new OrderMaterialPlanner(),
            new ValvePlanner(
                new BlankPieceCutter(),
                new ValveAssembler()));

        var result = optimizer.Calculate(settings, orders);

        Assert.Equal(1, result.RequiredPanels);
        Assert.Equal(2, result.Valves.Count);
        Assert.Empty(result.Waste);
        Assert.Contains(result.Valves, x => x.Valve.Height == 1280);
        Assert.Contains(result.Valves, x => x.Valve.Height == 880);
    }

    [Fact]
    public void Calculate_ReportsUnusedOffcutAsWaste()
    {
        var settings = new PanelSettings
        {
            PanelLength = 6000,
            PanelHeight = 1200,
            MinimumOffcut = 300
        };

        var orders = new[]
        {
            new ValveOrder
            {
                Height = 1280,
                Width = 1200,
                Quantity = 1
            },
            new ValveOrder
            {
                Height = 1200,
                Width = 1200,
                Quantity = 1
            },
            new ValveOrder
            {
                Height = 1220,
                Width = 1200,
                Quantity = 1
            }
        };

        var optimizer = new PanelOptimizer(
            new PanelCutter(),
            new OrderMaterialPlanner(),
            new ValvePlanner(
                new BlankPieceCutter(),
                new ValveAssembler()));

        var result = optimizer.Calculate(settings, orders);

        Assert.Equal(3, result.Valves.Count);
        Assert.Contains(result.Waste, x => x.Height == 880 && x.Length == 1200);
    }

    [Fact]
    public void Calculate_BuildsCuttingPlanForEachRequiredPanel()
    {
        var settings = new PanelSettings
        {
            PanelLength = 6000,
            PanelHeight = 1200,
            MinimumOffcut = 300
        };

        var orders = new[]
        {
            new ValveOrder
            {
                Height = 1280,
                Width = 1200,
                Quantity = 1
            },
            new ValveOrder
            {
                Height = 880,
                Width = 1200,
                Quantity = 1
            }
        };

        var optimizer = new PanelOptimizer(
            new PanelCutter(),
            new OrderMaterialPlanner(),
            new ValvePlanner(
                new BlankPieceCutter(),
                new ValveAssembler()));

        var result = optimizer.Calculate(settings, orders);

        Assert.Equal(1, result.RequiredPanels);
        Assert.Single(result.CuttingPlan.Panels);

        var panelPlan = result.CuttingPlan.Panels[0];

        Assert.Equal(0, panelPlan.PanelIndex);
        Assert.Equal(6000, panelPlan.PanelLength);
        Assert.Equal(1200, panelPlan.PanelHeight);
        Assert.Equal(4, panelPlan.Blanks.Count);
        Assert.All(panelPlan.Blanks, blank =>
        {
            Assert.Equal(1280, blank.Height);
            Assert.Equal(1200, blank.Length);
        });
        Assert.Equal(880, panelPlan.RemainingLength);
    }
}
