using AGR_PanelOptimizer.Core.Enums;
using AGR_PanelOptimizer.Core.Models;

namespace AGR_PanelOptimizer.Core.Services;

public class PieceCutter
{
    public (Piece RequiredPiece, Piece RemainingPiece) Cut(
        Piece source,
        int requiredLength)
    {
        if (requiredLength <= 0)
            throw new ArgumentOutOfRangeException(nameof(requiredLength));

        if (requiredLength >= source.Length)
            throw new ArgumentOutOfRangeException(nameof(requiredLength));

        var requiredPiece = new Piece
        {
            Length = requiredLength,
            LeftEdge = source.LeftEdge,
            RightEdge = EdgeType.Cut
        };

        var remainingPiece = new Piece
        {
            Length = source.Length - requiredLength,
            LeftEdge = EdgeType.Cut,
            RightEdge = source.RightEdge
        };

        return (requiredPiece, remainingPiece);
    }
}