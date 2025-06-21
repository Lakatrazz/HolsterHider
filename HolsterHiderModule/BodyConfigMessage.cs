using HolsterHider;

using LabFusion.Network;
using LabFusion.Network.Serialization;
using LabFusion.Player;
using LabFusion.SDK.Modules;

namespace HolsterHiderModule;

public class HolsterConfigData : INetSerializable
{
    public HolsterVisibility Visibility;

    public void Serialize(INetSerializer serializer)
    {
        serializer.SerializeValue(ref Visibility, Precision.OneByte);
    }

    public HolsterConfigData() { }

    public HolsterConfigData(HolsterConfig config)
    {
        Visibility = config.Visibility;
    }

    public HolsterConfig CreateConfig()
    {
        return new HolsterConfig() 
        {
            Visibility = Visibility 
        };
    }
}

public class BodyConfigData : INetSerializable
{
    public bool ScaleHolsters;

    public HolsterConfigData HeadGroup;
    public HolsterConfigData RightShoulderGroup;
    public HolsterConfigData LeftShoulderGroup;
    public HolsterConfigData RightUnderarmGroup;
    public HolsterConfigData LeftUnderarmGroup;
    public HolsterConfigData BackGroup;
    public HolsterConfigData AmmoPouchGroup;
    public HolsterConfigData BodyLogGroup;

    public void Serialize(INetSerializer serializer)
    {
        serializer.SerializeValue(ref ScaleHolsters);

        serializer.SerializeValue(ref HeadGroup);
        serializer.SerializeValue(ref RightShoulderGroup);
        serializer.SerializeValue(ref LeftShoulderGroup);
        serializer.SerializeValue(ref RightUnderarmGroup);
        serializer.SerializeValue(ref LeftUnderarmGroup);
        serializer.SerializeValue(ref BackGroup);
        serializer.SerializeValue(ref AmmoPouchGroup);
        serializer.SerializeValue(ref BodyLogGroup);
    }

    public BodyConfigData() { }

    public BodyConfigData(BodyConfig config)
    {
        ScaleHolsters = config.ScaleHolsters;

        HeadGroup = new(config.HeadGroup);
        RightShoulderGroup = new(config.RightShoulderGroup);
        LeftShoulderGroup = new(config.LeftShoulderGroup);
        RightUnderarmGroup = new(config.RightUnderarmGroup);
        LeftUnderarmGroup = new(config.LeftUnderarmGroup);
        BackGroup = new(config.BackGroup);
        AmmoPouchGroup = new(config.AmmoPouchGroup);
        BodyLogGroup = new(config.BodyLogGroup);
    }

    public BodyConfig CreateConfig()
    {
        var config = new BodyConfig()
        {
            ScaleHolsters = ScaleHolsters
        };

        config.HolsterConfigs[0] = HeadGroup.CreateConfig();
        config.HolsterConfigs[1] = RightShoulderGroup.CreateConfig();
        config.HolsterConfigs[2] = LeftShoulderGroup.CreateConfig();
        config.HolsterConfigs[3] = RightUnderarmGroup.CreateConfig();
        config.HolsterConfigs[4] = LeftUnderarmGroup.CreateConfig();
        config.HolsterConfigs[5] = BackGroup.CreateConfig();
        config.HolsterConfigs[6] = AmmoPouchGroup.CreateConfig();
        config.HolsterConfigs[7] = BodyLogGroup.CreateConfig();

        return config;
    }
}

public class BodyConfigMessage : ModuleMessageHandler
{
    protected override void OnHandleMessage(ReceivedMessage received)
    {
        var data = received.ReadData<BodyConfigData>();

        if (received.Sender == PlayerIDManager.LocalSmallID)
        {
            return;
        }

        PlayerConfigManager.SetConfig(received.Sender.Value, data.CreateConfig());
    }
}
