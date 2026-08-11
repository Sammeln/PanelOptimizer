using AGR_PanelOptimizer.Core.Models;

namespace AGR_PanelOptimizer.Core.Tests;

public class PanelTests
{
    [Fact]
    public void Panel_Can_Have_6000x1200_Dimensions()
    {
        var panel = new Panel
        {
            Length = 6000,
            Height = 1200
        };

        Assert.Equal(6000, panel.Length);
        Assert.Equal(1200, panel.Height);
    }
}