namespace HolsterHider;

public class BodyConfig
{
    private readonly HolsterConfig[] _holsterConfigs = null;

    public HolsterConfig[] HolsterConfigs => _holsterConfigs;

    public bool ScaleHolsters { get; set; } = true;

    public HolsterConfig HeadGroup => HolsterConfigs[0];
    public HolsterConfig RightShoulderGroup => HolsterConfigs[1];
    public HolsterConfig LeftShoulderGroup => HolsterConfigs[2];
    public HolsterConfig RightUnderarmGroup => HolsterConfigs[3];
    public HolsterConfig LeftUnderarmGroup => HolsterConfigs[4];
    public HolsterConfig BackGroup => HolsterConfigs[5];
    public HolsterConfig AmmoPouchGroup => HolsterConfigs[6];
    public HolsterConfig BodyLogGroup => HolsterConfigs[7];

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
