using BoneLib;
using BoneLib.BoneMenu;

using MelonLoader;

using UnityEngine;

using HolsterHider.MonoBehaviours;

using System.Reflection;
using System;

namespace HolsterHider;

public class HolsterHiderMod : MelonMod
{
    public const string Version = "1.2.0";

    public static Assembly HolsterHiderAssembly { get; private set; } = null;

    public static MelonPreferences_Category MelonCategory { get; private set; }
    public static Page MainPage { get; private set; }

    public static readonly BodyPreferences BodyPreferences = new();


    public static readonly BodyConfig LocalBodyConfig = new();
    public static HolsterHiderRig LocalRig { get; private set; } = null;

    public static event Action OnHolstersChanged;

    private static bool _preferencesSetup = false;

    public override void OnLateInitializeMelon()
    {
        HolsterHiderAssembly = MelonAssembly.Assembly;

        MelonCategory = MelonPreferences.CreateCategory("Holster Hider");

        BodyPreferences.LoadPreferences(MelonCategory);
        BodyPreferences.LoadConfig(LocalBodyConfig);

        SetupBoneMenu();

        _preferencesSetup = true;

        Hooking.OnLevelLoaded += OnLevelLoaded;

        CheckFusion();
    }

    private static void CheckFusion()
    {
        if (FindMelon("LabFusion", "Lakatrazz") != null)
        {
            EmbeddedResource.LoadAssemblyFromAssembly(HolsterHiderAssembly, "HolsterHider.resources.HolsterHiderModule.dll")
                .GetType("HolsterHiderModule.ModuleLoader")
                .GetMethod("LoadModule")
                .Invoke(null, null);
        }
    }

    public static void SetupBoneMenu()
    {
        MainPage = Page.Root.CreatePage("Holster Hider", new Color(1f, 0.75f, 0.79f));

        var scalePreference = BodyPreferences.ScalePreference;
        var scaleElement = MainPage.CreateBool("Resize Holsters", Color.cyan, scalePreference.Preference.Value, scalePreference.OnBoneMenuChange);
        BodyPreferences.ScalePreference.Element = scaleElement;

        foreach (var holster in BodyPreferences.HolsterPreferences)
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

    private static void UpdatePreferences()
    {
        BodyPreferences.UpdatePreferences();
    }

    public static void UpdateHolsters()
    {
        if (LocalRig == null)
        {
            return;
        }

        LocalRig.ApplyConfig();

        OnHolstersChanged?.InvokeActionSafe();
    }

    private static void ResetHolsters()
    {
        BodyPreferences.ResetPreferences();
        UpdateHolsters();
    }

    public static void OnLevelLoaded(LevelInfo level)
    {
        var rigManager = Player.RigManager;

        LocalRig = rigManager.gameObject.AddComponent<HolsterHiderRig>();
        LocalRig.Config = LocalBodyConfig;

        UpdateHolsters();
    }
}
