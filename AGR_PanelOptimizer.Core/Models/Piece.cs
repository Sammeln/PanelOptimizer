using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AGR_PanelOptimizer.Core.Enums;

namespace AGR_PanelOptimizer.Core.Models;

public class Piece
{
    public int Length { get; init; }
    public int Height { get; init; }

    public EdgeType LeftEdge { get; init; }

    public EdgeType RightEdge { get; init; }
}
