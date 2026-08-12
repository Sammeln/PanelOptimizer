using AGR_PanelOptimizer.Core.Models;

namespace AGR_PanelOptimizer.Core.Services;

public class MaterialPool
{
    private readonly List<Blank> _blanks = [];

    public IReadOnlyList<Blank> Blanks => _blanks;

    public void Add(Blank blank)
    {
        ArgumentNullException.ThrowIfNull(blank);

        _blanks.Add(blank);
    }

    public bool Remove(Blank blank)
    {
        ArgumentNullException.ThrowIfNull(blank);

        return _blanks.Remove(blank);
    }
}