using AGR_PanelOptimizer.Core.Enums;
using AGR_PanelOptimizer.Core.Models;

namespace AGR_PanelOptimizer.Core.Services;

public class BlankPieceCutter
{
    public (Piece RequiredPiece, Piece? RemainingPiece) Cut(
        Blank blank,
        int requiredLength)
    {
        ArgumentNullException.ThrowIfNull(blank);

        if (requiredLength <= 0)
            throw new ArgumentOutOfRangeException(nameof(requiredLength));

        if (requiredLength > blank.Length)
            throw new ArgumentOutOfRangeException(nameof(requiredLength));

        if (requiredLength == blank.Length)
        {
            return (
                new Piece
                {
                    Length = blank.Length,
                    Height = blank.Height,
                    LeftEdge = blank.LeftEdge,
                    RightEdge = blank.RightEdge
                },
                null);
        }

        var requiredPiece = new Piece
        {
            Length = requiredLength,
            Height = blank.Height,
            LeftEdge = blank.LeftEdge,
            RightEdge = EdgeType.Cut
        };

        var remainingPiece = new Piece
        {
            Length = blank.Length - requiredLength,
            Height = blank.Height,
            LeftEdge = EdgeType.Cut,
            RightEdge = blank.RightEdge
        };

        return (requiredPiece, remainingPiece);
    }
}