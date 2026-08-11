using AGR_PanelOptimizer.Core.Models;

namespace AGR_PanelOptimizer.Core.Services;

public class ValveAssembler
{
    public ValveAssemblyResult Assemble(
        int height,
        int width,
        IReadOnlyList<Piece> pieces)
    {
        ArgumentNullException.ThrowIfNull(pieces);

        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height));

        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width));

        if (pieces.Count == 0)
            throw new ArgumentException(
                "At least one piece is required.",
                nameof(pieces));

        var totalLength = pieces.Sum(x => x.Length);

        if (totalLength != width)
        {
            throw new InvalidOperationException(
                $"Pieces total length ({totalLength}) " +
                $"does not match valve width ({width}).");
        }

        ValidatePieces(height,pieces);

        return new ValveAssemblyResult
        {
            Valve = new Valve
            {
                Height = height,
                Width = width,
                Pieces = pieces
            }
        };
    }

    private static void ValidatePieces(
        int valveHeight,
        IReadOnlyList<Piece> pieces)
    {
        var first = pieces[0];

        foreach (var piece in pieces)
        {
            if (piece.Height != valveHeight)
            {
                throw new InvalidOperationException(
                    $"Piece height ({piece.Height}) " +
                    $"does not match valve height ({valveHeight}).");
            }
        }

        if (first.LeftEdge != Enums.EdgeType.Tongue &&
            first.LeftEdge != Enums.EdgeType.Cut)
        {
            throw new InvalidOperationException(
                "The first piece must have Tongue or Cut on the left edge.");
        }

        if (first.RightEdge != Enums.EdgeType.Groove)
        {
            throw new InvalidOperationException(
                "The first piece must have Groove on the right edge.");
        }

        for (var i = 1; i < pieces.Count - 1; i++)
        {
            var piece = pieces[i];

            if (piece.LeftEdge != Enums.EdgeType.Tongue ||
                piece.RightEdge != Enums.EdgeType.Groove)
            {
                throw new InvalidOperationException(
                    "Middle pieces must have Tongue on the left " +
                    "and Groove on the right.");
            }
        }

        if (pieces.Count > 1)
        {
            var last = pieces[^1];

            if (last.LeftEdge != Enums.EdgeType.Tongue)
            {
                throw new InvalidOperationException(
                    "The last piece must have Tongue on the left edge.");
            }

            if (last.RightEdge != Enums.EdgeType.Groove &&
                last.RightEdge != Enums.EdgeType.Cut)
            {
                throw new InvalidOperationException(
                    "The last piece must have Groove or Cut on the right edge.");
            }
        }
    }
}