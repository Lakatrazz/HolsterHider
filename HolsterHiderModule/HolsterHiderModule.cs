using HolsterHider;

using LabFusion.Network;
using LabFusion.Player;
using LabFusion.SDK.Modules;
using LabFusion.Utilities;

using System.Reflection;

using Module = LabFusion.SDK.Modules.Module;

namespace HolsterHiderModule;

public class HolsterHiderModule : Module
{
    public override string Name => "Holster Hider";

    public override string Author => "Lakatrazz";

    public override Version Version => new(HolsterHiderMod.Version);

    public override ConsoleColor Color => ConsoleColor.Green;

    public Assembly ModuleAssembly { get; private set; } = null;

    protected override void OnModuleRegistered()
    {
        ModuleAssembly = Assembly.GetExecutingAssembly();

        ModuleMessageManager.LoadHandlers(ModuleAssembly);

        PlayerConfigManager.OnInitialize();

        MultiplayerHooking.OnJoinedServer += OnJoinedServer;
        MultiplayerHooking.OnPlayerJoined += OnPlayerJoined;
        HolsterHiderMod.OnHolstersChanged += OnHolstersChanged;
    }

    private void OnJoinedServer()
    {
        OnHolstersChanged();
    }

    private void OnPlayerJoined(PlayerID playerID)
    {
        var data = new BodyConfigData(HolsterHiderMod.LocalBodyConfig);

        MessageRelay.RelayModule<BodyConfigMessage, BodyConfigData>(data, new MessageRoute(playerID.SmallID, NetworkChannel.Reliable));
    }

    private void OnHolstersChanged()
    {
        var data = new BodyConfigData(HolsterHiderMod.LocalBodyConfig);

        MessageRelay.RelayModule<BodyConfigMessage, BodyConfigData>(data, CommonMessageRoutes.ReliableToOtherClients);
    }

    protected override void OnModuleUnregistered()
    {
    }
}