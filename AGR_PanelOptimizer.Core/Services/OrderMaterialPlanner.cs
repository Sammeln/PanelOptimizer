using AGR_PanelOptimizer.Core.Models;

namespace AGR_PanelOptimizer.Core.Services;

public class OrderMaterialPlanner
{
    private readonly OffcutEvaluator _offcutEvaluator;

    public OrderMaterialPlanner(OffcutEvaluator? offcutEvaluator = null)
    {
        _offcutEvaluator = offcutEvaluator ?? new OffcutEvaluator();
    }

    public OrderMaterialPlan Prepare(
        PanelCutResult cutResult,
        IReadOnlyCollection<ValveOrder> orders)
    {
        ArgumentNullException.ThrowIfNull(cutResult);
        ArgumentNullException.ThrowIfNull(orders);

        var blanks = new List<Blank>(cutResult.Blanks);
        var waste = new List<PanelOffcut>();

        foreach (var offcut in cutResult.Offcuts)
        {
            if (_offcutEvaluator.IsUsable(offcut, orders))
            {
                blanks.Add(new Blank
                {
                    Height = offcut.Height,
                    Length = offcut.Length,
                    LeftEdge = Enums.EdgeType.Tongue,
                    RightEdge = Enums.EdgeType.Groove,
                    SourcePanelPosition = offcut.SourcePanelPosition
                });
            }
            else
            {
                waste.Add(offcut);
            }
        }

        return new OrderMaterialPlan
        {
            Blanks = blanks,
            Waste = waste
        };
    }
}
