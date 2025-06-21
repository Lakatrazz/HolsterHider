using BoneLib.BoneMenu;

using MelonLoader;

using System;

namespace HolsterHider;

public class HolsterPreference
{
    public MelonPreferences_Entry<HolsterVisibility> preference;
    public EnumElement element;
    public string identifier;

    public HolsterConfig config;

    public HolsterPreference(MelonPreferences_Category category, string identifier)
    {
        preference = category.CreateEntry(identifier, HolsterVisibility.DEFAULT);
        this.identifier = identifier;
    }

    public void OnBoneMenuChange(Enum value)
    {
        var visibility = (HolsterVisibility)value;

        SetVisibility(visibility);
        HolsterHiderMod.UpdateHolsters();
    }

    public void SetVisibility(HolsterVisibility visibility)
    {
        preference.Value = visibility;
        preference.Category.SaveToFile(false);

        UpdatePreferences();
    }

    public void UpdatePreferences()
    {
        var visibility = preference.Value;

        if (element != null)
        {
            element.Value = visibility;
        }

        if (config != null)
        {
            config.Visibility = visibility;
        }
    }
}
