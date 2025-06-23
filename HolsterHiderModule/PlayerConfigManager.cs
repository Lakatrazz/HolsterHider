using HolsterHider;
using HolsterHider.MonoBehaviours;

using Il2CppSLZ.Marrow;

using LabFusion.Entities;
using LabFusion.Player;
using LabFusion.Utilities;

using UnityEngine;

namespace HolsterHiderModule;

public static class PlayerConfigManager
{
    public static readonly Dictionary<byte, BodyConfig> PlayerIDToConfig = new();

    public static readonly Dictionary<byte, HolsterHiderRig> PlayerIDToRig = new();

    public static void OnInitialize()
    {
        MultiplayerHooking.OnPlayerLeft += OnPlayerLeft;
        MultiplayerHooking.OnDisconnected += OnDisconnected;

        NetworkPlayer.OnNetworkRigCreated += OnNetworkRigCreated;
    }

    private static void OnDisconnected()
    {
        PlayerIDToConfig.Clear();
        PlayerIDToRig.Clear();
    }

    private static void OnPlayerLeft(PlayerID playerID)
    {
        RemoveConfig(playerID.SmallID);
    }

    private static void OnNetworkRigCreated(NetworkPlayer player, RigManager rigManager)
    {
        if (player.NetworkEntity.IsOwner)
        {
            return;
        }

        if (!PlayerIDToConfig.TryGetValue(player.PlayerID.SmallID, out var config))
        {
            return;
        }

        var rig = GetOrCreateRig(player.PlayerID.SmallID);

        if (rig != null)
        {
            rig.Config = config;
        }
    }

    public static void SetConfig(byte smallID, BodyConfig config) 
    {
        PlayerIDToConfig[smallID] = config;

        var rig = GetOrCreateRig(smallID);

        if (rig != null)
        {
            rig.Config = config;
        }
    }

    public static void RemoveConfig(byte smallID)
    {
        PlayerIDToConfig.Remove(smallID);

        var existingRig = GetRig(smallID);

        if (existingRig != null)
        {
            GameObject.Destroy(existingRig);
        }
    }

    public static HolsterHiderRig GetRig(byte smallID)
    {
        PlayerIDToRig.TryGetValue(smallID, out var rig);

        return rig;
    }

    public static HolsterHiderRig GetOrCreateRig(byte smallID)
    {
        var existingRig = GetRig(smallID);

        if (existingRig != null)
        {
            return existingRig;
        }

        if (!NetworkPlayerManager.TryGetPlayer(smallID, out var player))
        {
            return null;
        }

        if (!player.HasRig)
        {
            return null;
        }

        if (player.NetworkEntity.IsOwner)
        {
            return null;
        }

        var newRig = player.RigRefs.RigManager.gameObject.AddComponent<HolsterHiderRig>();

        return newRig;
    }
}
