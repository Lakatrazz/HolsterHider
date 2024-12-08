using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolsterHider;

public class BodyConfig
{
    private readonly HolsterConfig[] _holsterConfigs = null;

    public HolsterConfig[] HolsterConfigs => _holsterConfigs;

    public bool ScaleHolsters { get; set; } = true;

    public HolsterConfig HeadGroup => _holsterConfigs[0];
    public HolsterConfig RightShoulderGroup => _holsterConfigs[1];
    public HolsterConfig LeftShoulderGroup => _holsterConfigs[2];
    public HolsterConfig RightUnderarmGroup => _holsterConfigs[3];
    public HolsterConfig LeftUnderarmGroup => _holsterConfigs[4];
    public HolsterConfig BackGroup => _holsterConfigs[5];
    public HolsterConfig AmmoPouchGroup => _holsterConfigs[6];
    public HolsterConfig BodyLogGroup => _holsterConfigs[7];

    public BodyConfig()
    {
        _holsterConfigs = new HolsterConfig[]
        {
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
        };
    }
}
