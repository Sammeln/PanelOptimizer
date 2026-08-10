using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanelOptimizer.Core.Models;

public class PanelSettings
{
    public int PanelLength { get; set; } = 6000;

    public int PanelHeight { get; set; } = 1200;

    public int MinimumOffcut { get; set; } = 300;
}
