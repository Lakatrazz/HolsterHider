using System;

using UnityEngine;

using MelonLoader;

using Il2CppSLZ.Marrow;

using Il2CppInterop.Runtime.Attributes;

namespace HolsterHider.MonoBehaviours;

[RegisterTypeInIl2Cpp]
public class HolsterHiderRig : MonoBehaviour
{
    [HideFromIl2Cpp]
    public RigManager RigManager { get; set; } = null;

    [HideFromIl2Cpp]
    public BodyReferences References { get; set; } = null;

    private BodyConfig _config = null;

    [HideFromIl2Cpp]
    public BodyConfig Config
    {
        get
        {
            return _config;
        }
        set
        {
            _config = value;

            ApplyConfig();
        }
    }

    private Il2CppSystem.Action _onAvatarSwappedDelegate = null;

    private void Awake()
    {
        RigManager = GetComponent<RigManager>();

        References = new();
        References.CacheReferences(RigManager);
    }

    private void OnEnable()
    {
        HookRig();
    }

    private void OnDisable()
    {
        UnhookRig();
    }

    private void HookRig()
    {
        _onAvatarSwappedDelegate = (Action)OnAvatarSwapped;

        RigManager.onAvatarSwapped += _onAvatarSwappedDelegate;
    }

    private void UnhookRig()
    {
        RigManager.onAvatarSwapped -= _onAvatarSwappedDelegate;

        _onAvatarSwappedDelegate = null;
    }

    private void OnAvatarSwapped()
    {
        ApplyConfig();
    }

    public void ApplyConfig()
    {
        if (Config == null)
        {
            return;
        }

        References.ApplyConfig(Config);
    }
}
