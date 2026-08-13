using AGR_PanelOptimizer.Core.Interfaces;
using AGR_PanelOptimizer.Core.Models;

namespace AGR_PanelOptimizer.Core.Services;

public class PanelOptimizer : IPanelOptimizer
{
    private readonly PanelCutter _panelCutter;
    private readonly OrderMaterialPlanner _orderMaterialPlanner;
    private readonly ValvePlanner _valvePlanner;

    public PanelOptimizer(
        PanelCutter panelCutter,
        OrderMaterialPlanner orderMaterialPlanner,
        ValvePlanner valvePlanner)
    {
        ArgumentNullException.ThrowIfNull(panelCutter);
        ArgumentNullException.ThrowIfNull(orderMaterialPlanner);
        ArgumentNullException.ThrowIfNull(valvePlanner);

        _panelCutter = panelCutter;
        _orderMaterialPlanner = orderMaterialPlanner;
        _valvePlanner = valvePlanner;
    }

    public OptimizationResult Calculate(
        PanelSettings settings,
        IReadOnlyCollection<ValveOrder> orders)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(orders);

        if (settings.PanelLength <= 0)
            throw new ArgumentOutOfRangeException(nameof(settings.PanelLength));

        if (settings.PanelHeight <= 0)
            throw new ArgumentOutOfRangeException(nameof(settings.PanelHeight));

        if (settings.MinimumOffcut < 0)
            throw new ArgumentOutOfRangeException(nameof(settings.MinimumOffcut));

        if (orders.Count == 0)
            return new OptimizationResult();

        var materialPool = new MaterialPool();
        var waste = new List<PanelOffcut>();
        var valves = new List<ValveAssemblyResult>();
        var requiredPanels = 0;

        foreach (var order in orders)
        {
            ArgumentNullException.ThrowIfNull(order);

            if (order.Height <= 0)
                throw new ArgumentOutOfRangeException(nameof(order.Height));

            if (order.Width <= 0)
                throw new ArgumentOutOfRangeException(nameof(order.Width));

            if (order.Quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(order.Quantity));

            for (var i = 0; i < order.Quantity; i++)
            {
                while (GetAvailableLength(materialPool, order.Height) < order.Width)
                {
                    var panel = new Panel
                    {
                        Length = settings.PanelLength,
                        Height = settings.PanelHeight
                    };

                    var cutResult = _panelCutter.Cut(panel, order.Height);
                    var materialPlan = _orderMaterialPlanner.Prepare(
                        cutResult,
                        orders);

                    foreach (var blank in materialPlan.Blanks)
                        materialPool.Add(blank);

                    waste.AddRange(materialPlan.Waste);
                    requiredPanels++;
                }

                valves.Add(_valvePlanner.CreateValve(
                    order,
                    materialPool,
                    settings.MinimumOffcut));
            }
        }

        return new OptimizationResult
        {
            RequiredPanels = requiredPanels,
            Valves = valves,
            Waste = waste
        };
    }

    private static int GetAvailableLength(
        MaterialPool materialPool,
        int height)
    {
        return materialPool.Blanks
            .Where(blank => blank.Height == height)
            .Sum(blank => blank.Length);
    }
}
