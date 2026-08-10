namespace AGR_PanelOptimizer.Core.Models;

public class Valve
{
    public int Height { get; init; }

    public int Width { get; init; }

    public IReadOnlyList<Piece> Pieces { get; init; } =
        Array.Empty<Piece>();

    public IReadOnlyList<Piece> Waste { get; init; } =
        Array.Empty<Piece>();
}