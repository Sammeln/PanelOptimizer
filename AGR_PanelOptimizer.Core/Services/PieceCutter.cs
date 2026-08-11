using AGR_PanelOptimizer.Core.Enums;
using AGR_PanelOptimizer.Core.Models;

namespace AGR_PanelOptimizer.Core.Services;

public class PieceCutter
{
    public (Piece RequiredPiece, Piece RemainingPiece) Cut(
        Piece source,
        int requiredLength)
    {
        ArgumentNullException.ThrowIfNull(source);

        if (requiredLength <= 0)
            throw new ArgumentOutOfRangeException(nameof(requiredLength));

        if (requiredLength >= source.Length)
            throw new ArgumentOutOfRangeException(nameof(requiredLength));

        var requiredPiece = new Piece
        {
            Length = requiredLength,
            Height = source.Height,
            LeftEdge = source.LeftEdge,
            RightEdge = EdgeType.Cut
        };

        var remainingPiece = new Piece
        {
            Length = source.Length - requiredLength,
            Height = source.Height,
            LeftEdge = EdgeType.Cut,
            RightEdge = source.RightEdge
        };

        return (requiredPiece, remainingPiece);
    }
}