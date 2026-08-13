using AGR_PanelOptimizer.Core.Models;

namespace AGR_PanelOptimizer.Core.Services;

public class OffcutEvaluator
{
    public bool IsUsable(Piece piece, int minimumLength)
    {
        ArgumentNullException.ThrowIfNull(piece);

        if (minimumLength < 0)
            throw new ArgumentOutOfRangeException(nameof(minimumLength));

        return piece.Length >= minimumLength;
    }

    public bool IsUsable(
        PanelOffcut offcut,
        IReadOnlyCollection<ValveOrder> orders)
    {
        ArgumentNullException.ThrowIfNull(offcut);
        ArgumentNullException.ThrowIfNull(orders);

        return orders.Any(order => order.Height == offcut.Height);
    }
}
