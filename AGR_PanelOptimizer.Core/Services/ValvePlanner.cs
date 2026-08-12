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
        MaterialPool materialPool,
        int minimumOffcut)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentNullException.ThrowIfNull(materialPool);

        if (order.Height <= 0)
            throw new ArgumentOutOfRangeException(nameof(order.Height));

        if (order.Width <= 0)
            throw new ArgumentOutOfRangeException(nameof(order.Width));

        if (minimumOffcut < 0)
            throw new ArgumentOutOfRangeException(nameof(minimumOffcut));

        if (materialPool.Blanks.Count == 0)
            throw new ArgumentException(
                "At least one blank is required.",
                nameof(materialPool));

        var pieces = new List<Piece>();
        var remainingWidth = order.Width;

        foreach (var blank in materialPool.Blanks.ToArray())
        {
            if (remainingWidth == 0)
                break;

            if (blank.Height != order.Height)
                continue;

            var requiredLength = Math.Min(
                blank.Length,
                remainingWidth);

            var (requiredPiece, remainingPiece) =
                _blankPieceCutter.Cut(
                    blank,
                    requiredLength);

            materialPool.Remove(blank);
            pieces.Add(requiredPiece);

            if (remainingPiece is not null &&
                remainingPiece.Length >= minimumOffcut)
            {
                // Пригодный остаток возвращаем в пул как новую заготовку.
                materialPool.Add(new Blank
                {
                    Length = remainingPiece.Length,
                    Height = remainingPiece.Height,
                    LeftEdge = remainingPiece.LeftEdge,
                    RightEdge = remainingPiece.RightEdge,
                    SourcePanelPosition = blank.SourcePanelPosition
                });
            }

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

    public ValveAssemblyResult CreateValve(
        ValveOrder order,
        IReadOnlyList<Blank> blanks,
        int minimumOffcut)
    {
        ArgumentNullException.ThrowIfNull(blanks);

        var materialPool = new MaterialPool();

        foreach (var blank in blanks)
            materialPool.Add(blank);

        return CreateValve(order, materialPool, minimumOffcut);
    }
}
