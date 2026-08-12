using AGR_PanelOptimizer.Core.Models;

namespace AGR_PanelOptimizer.Core.Services;

public class ValvePlanner
{
    private readonly BlankPieceCutter _blankPieceCutter;
    private readonly ValveAssembler _valveAssembler;

    public ValvePlanner(
        BlankPieceCutter blankPieceCutter,
        ValveAssembler valveAssembler)
    {
        ArgumentNullException.ThrowIfNull(blankPieceCutter);
        ArgumentNullException.ThrowIfNull(valveAssembler);

        _blankPieceCutter = blankPieceCutter;
        _valveAssembler = valveAssembler;
    }

    public ValveAssemblyResult CreateValve(
        ValveOrder order,
        IReadOnlyList<Blank> blanks,
        int minimumOffcut)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentNullException.ThrowIfNull(blanks);

        if (order.Height <= 0)
            throw new ArgumentOutOfRangeException(nameof(order.Height));

        if (order.Width <= 0)
            throw new ArgumentOutOfRangeException(nameof(order.Width));

        if (minimumOffcut < 0)
            throw new ArgumentOutOfRangeException(nameof(minimumOffcut));

        if (blanks.Count == 0)
            throw new ArgumentException(
                "At least one blank is required.",
                nameof(blanks));

        var pieces = new List<Piece>();
        var remainingWidth = order.Width;

        foreach (var blank in blanks)
        {
            if (remainingWidth == 0)
                break;

            if (blank.Height != order.Height)
            {
                throw new InvalidOperationException(
                    $"Blank height ({blank.Height}) " +
                    $"does not match valve height ({order.Height}).");
            }

            var requiredLength = Math.Min(
                blank.Length,
                remainingWidth);

            var (requiredPiece, remainingPiece) =
                _blankPieceCutter.Cut(
                    blank,
                    requiredLength);

            pieces.Add(requiredPiece);

            // Если после реза остался пригодный остаток,
            // пока просто не теряем информацию о нём.
            // На следующем этапе он будет возвращаться в MaterialPool.
            _ = remainingPiece;

            remainingWidth -= requiredPiece.Length;
        }

        if (remainingWidth > 0)
        {
            throw new InvalidOperationException(
                $"Not enough material to create valve " +
                $"{order.Height}x{order.Width}. " +
                $"Missing length: {remainingWidth} mm.");
        }

        return _valveAssembler.Assemble(
            order.Height,
            order.Width,
            pieces);
    }
}