using AGR_PanelOptimizer.Core.Models;

namespace AGR_PanelOptimizer.Core.Tests;

public class CuttingPlanTests
{
    [Fact]
    public void PanelCutPlan_Stores_Panel_Information()
    {
        var blank = new Blank
        {
            Height = 1280,
            Length = 1200,
            SourcePanelPosition = 0
        };

        var plan = new PanelCutPlan
        {
            PanelIndex = 1,
            PanelLength = 6000,
            PanelHeight = 1200,
            Blanks = new[] { blank },
            RemainingLength = 4720
        };

        Assert.Equal(1, plan.PanelIndex);
        Assert.Equal(6000, plan.PanelLength);
        Assert.Equal(1200, plan.PanelHeight);

        Assert.Single(plan.Blanks);
        Assert.Equal(1280, plan.Blanks[0].Height);
        Assert.Equal(1200, plan.Blanks[0].Length);

        Assert.Equal(4720, plan.RemainingLength);
    }
}