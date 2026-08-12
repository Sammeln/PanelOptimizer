using AGR_PanelOptimizer.Core.Models;
using AGR_PanelOptimizer.Core.Services;

namespace AGR_PanelOptimizer.Core.Tests;

public class MaterialPoolTests
{
    [Fact]
    public void Add_ShouldAddBlankToPool()
    {
        var pool = new MaterialPool();

        var blank = new Blank
        {
            Height = 1280,
            Length = 1200
        };

        pool.Add(blank);

        Assert.Single(pool.Blanks);
        Assert.Same(blank, pool.Blanks[0]);
    }

    [Fact]
    public void Add_ShouldPreserveInsertionOrder()
    {
        var pool = new MaterialPool();

        var first = new Blank
        {
            Height = 1280,
            Length = 1200
        };

        var second = new Blank
        {
            Height = 1280,
            Length = 600
        };

        pool.Add(first);
        pool.Add(second);

        Assert.Equal(2, pool.Blanks.Count);
        Assert.Same(first, pool.Blanks[0]);
        Assert.Same(second, pool.Blanks[1]);
    }

    [Fact]
    public void Remove_ShouldRemoveBlankFromPool()
    {
        var pool = new MaterialPool();

        var blank = new Blank
        {
            Height = 1280,
            Length = 600
        };

        pool.Add(blank);

        var removed = pool.Remove(blank);

        Assert.True(removed);
        Assert.Empty(pool.Blanks);
    }

    [Fact]
    public void Remove_ShouldReturnFalse_WhenBlankIsNotInPool()
    {
        var pool = new MaterialPool();

        var blank = new Blank
        {
            Height = 1280,
            Length = 600
        };

        var removed = pool.Remove(blank);

        Assert.False(removed);
        Assert.Empty(pool.Blanks);
    }
}