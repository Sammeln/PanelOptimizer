using AGR_PanelOptimizer.Core.Enums;
using AGR_PanelOptimizer.Core.Models;
using AGR_PanelOptimizer.Core.Services;

namespace AGR_PanelOptimizer.Core.Tests;

public class ValveProductionScenarioTests
{
    [Fact]
    public void Produce_1280x1200_And_880x1200_Valves_Using_PanelOffcut()
    {
        // Arrange
        var panel = new Panel
        {
            Length = 6000,
            Height = 1200
        };

        var panelCutter = new PanelCutter();
        var cutResult = panelCutter.Cut(panel, 1280);

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

        var materialPlanner = new OrderMaterialPlanner();
        var materialPlan = materialPlanner.Prepare(cutResult, orders);

        var materialPool = new MaterialPool();

        foreach (var blank in materialPlan.Blanks)
            materialPool.Add(blank);

        var valvePlanner = new ValvePlanner(
            new BlankPieceCutter(),
            new ValveAssembler());

        // Act
        var firstValve = valvePlanner.CreateValve(
            orders[0],
            materialPool,
            minimumOffcut: 300);

        var secondValve = valvePlanner.CreateValve(
            orders[1],
            materialPool,
            minimumOffcut: 300);

        // Assert
        Assert.Equal(1280, firstValve.Valve.Height);
        Assert.Equal(1200, firstValve.Valve.Width);
        Assert.Single(firstValve.Valve.Pieces);

        Assert.Equal(880, secondValve.Valve.Height);
        Assert.Equal(1200, secondValve.Valve.Width);
        Assert.Single(secondValve.Valve.Pieces);

        Assert.Empty(materialPlan.Waste);
        Assert.Equal(3, materialPool.Blanks.Count);
        Assert.All(
            materialPool.Blanks,
            blank => Assert.Equal(1280, blank.Height));
    }

    [Fact]
    public void Prepare_OffcutAsWaste_WhenNoValveInOrderHasMatchingHeight()
    {
        // Arrange
        var panel = new Panel
        {
            Length = 6000,
            Height = 1200
        };

        var panelCutter = new PanelCutter();
        var cutResult = panelCutter.Cut(panel, 1280);

        var orders = new[]
        {
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
            },
            new ValveOrder
            {
                Height = 1280,
                Width = 1200,
                Quantity = 1
            }
        };

        var materialPlanner = new OrderMaterialPlanner();

        // Act
        var materialPlan = materialPlanner.Prepare(cutResult, orders);

        // Assert
        Assert.Equal(4, materialPlan.Blanks.Count);
        Assert.Single(materialPlan.Waste);

        var waste = materialPlan.Waste[0];

        Assert.Equal(1200, waste.Length);
        Assert.Equal(880, waste.Height);
        Assert.Equal(4, waste.SourcePanelPosition);
    }
}
