using BoneLib.BoneMenu;

using MelonLoader;
using System;

namespace HolsterHider;

public class BoolPreference
{
    public MelonPreferences_Entry<bool> preference;
    public BoolElement element;
    public string identifier;

    public Action<bool> OnSetValue;

    public BoolPreference(MelonPreferences_Category category, string identifier)
    {
        preference = category.CreateEntry(identifier, true);
        this.identifier = identifier;
    }

    public void OnBoneMenuChange(bool value)
    {
        SetValue(value);
        HolsterHiderMod.UpdateHolsters();
    }

    public void SetValue(bool value)
    {
        preference.Value = value;
        preference.Category.SaveToFile(false);

        UpdatePreferences();
    }

    public void UpdatePreferences()
    {
        var value = preference.Value;

        if (element != null)
        {
            element.Value = value;
        }

        OnSetValue?.Invoke(value);
    }
}
