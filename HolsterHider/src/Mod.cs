using BoneLib;
using BoneLib.BoneMenu;

using MelonLoader;

using UnityEngine;

using Il2CppSLZ.VRMK;

namespace HolsterHider;

public class HolsterHiderMod : MelonMod
{
    public const string Version = "1.1.0";

    public MelonPreferences_Category MelonCategory { get; private set; }
    public Page MainPage { get; private set; }


    private static readonly BodyPreference _bodyPreferences = new();
    private static readonly BodyReference _bodyReferences = new();
    private static readonly BodyConfig _bodyConfig = new();

    private bool _preferencesSetup = false;

    public override void OnLateInitializeMelon()
    {
        MelonCategory = MelonPreferences.CreateCategory("Holster Hider");

        _bodyPreferences.LoadPreferences(MelonCategory);
        _bodyPreferences.LoadConfig(_bodyConfig);

        SetupBoneMenu();

        _preferencesSetup = true;

        Hooking.OnLevelLoaded += OnLevelLoaded;
        Hooking.OnSwitchAvatarPostfix += OnSwitchAvatar;
    }

    private void OnSwitchAvatar(Avatar avatar)
    {
        if (avatar != Player.RigManager.avatar)
        {
            return;
        }

        UpdateHolsters();
    }

    public void SetupBoneMenu()
    {
        MainPage = Page.Root.CreatePage("Holster Hider", new Color(1f, 0.75f, 0.79f));

        var scalePreference = _bodyPreferences.ScalePreference;
        var scaleElement = MainPage.CreateBool("Resize Holsters", Color.cyan, scalePreference.preference.Value, scalePreference.OnBoneMenuChange);
        _bodyPreferences.ScalePreference.element = scaleElement;

        foreach (var holster in _bodyPreferences.HolsterPreferences)
        {
            holster.element = MainPage.CreateEnum(holster.identifier, Color.green, holster.preference.Value, holster.OnBoneMenuChange);
        }

        MainPage.CreateFunction("Reset Settings", Color.green, ResetHolsters);
    }

    public override void OnPreferencesLoaded()
    {
        if (!_preferencesSetup)
        {
            return;
        }

        UpdatePreferences();
        UpdateHolsters();
    }

    private void UpdatePreferences()
    {
        _bodyPreferences.UpdatePreferences();
    }

    public static void UpdateHolsters()
    {
        _bodyReferences.ApplyConfig(_bodyConfig);
    }

    private void ResetHolsters()
    {
        _bodyPreferences.ResetPreferences();
        UpdateHolsters();
    }

    public static void OnLevelLoaded(LevelInfo level)
    {
        var rigManager = Player.RigManager;

        _bodyReferences.CacheReferences(rigManager);

        UpdateHolsters();
    }
}
