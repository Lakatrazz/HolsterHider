using BoneLib.BoneMenu;

using MelonLoader;
using System;

namespace HolsterHider;

public class BoolPreference
{
    public MelonPreferences_Entry<bool> Preference;
    public BoolElement Element;
    public string Identifier;

    public Action<bool> OnSetValue;

    public BoolPreference(MelonPreferences_Category category, string identifier)
    {
        Preference = category.CreateEntry(identifier, true);
        this.Identifier = identifier;
    }

    public void OnBoneMenuChange(bool value)
    {
        SetValue(value);
        HolsterHiderMod.UpdateHolsters();
    }

    public void SetValue(bool value)
    {
        Preference.Value = value;
        Preference.Category.SaveToFile(false);

        UpdatePreferences();
    }

    public void UpdatePreferences()
    {
        var value = Preference.Value;

        if (Element != null)
        {
            Element.Value = value;
        }

        OnSetValue?.Invoke(value);
    }
}
