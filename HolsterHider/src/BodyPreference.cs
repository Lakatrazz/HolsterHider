using MelonLoader;

namespace HolsterHider;

public class BodyPreference
{
    public HolsterPreference[] _holsterPreferences = null;

    public HolsterPreference[] HolsterPreferences => _holsterPreferences;

    public BoolPreference ScalePreference { get; set; }

    public HolsterPreference HeadPreference => _holsterPreferences[0];
    public HolsterPreference RightShoulderPreference => _holsterPreferences[1];
    public HolsterPreference LeftShoulderPreference => _holsterPreferences[2];
    public HolsterPreference RightUnderarmPreference => _holsterPreferences[3];
    public HolsterPreference LeftUnderarmPreference => _holsterPreferences[4];
    public HolsterPreference BackPreference => _holsterPreferences[5];
    public HolsterPreference AmmoPouchPreference => _holsterPreferences[6];
    public HolsterPreference BodyLogPreference => _holsterPreferences[7];

    public void LoadPreferences(MelonPreferences_Category category)
    {
        ScalePreference = new BoolPreference(category, "Resize Holsters");

        _holsterPreferences = new HolsterPreference[] {
            new(category, "Head"),
            new(category, "Right Shoulder"),
            new(category, "Left Shoulder"),
            new(category, "Right Underarm"),
            new(category, "Left Underarm"),
            new(category, "Back"),
            new(category, "Ammo Pouch"),
            new(category, "Body Log"),
        };
    }

    public void LoadConfig(BodyConfig config)
    {
        ScalePreference.OnSetValue = (value) =>
        {
            config.ScaleHolsters = value;
        };
        ScalePreference.UpdatePreferences();

        for (var i = 0; i < HolsterPreferences.Length; i++)
        {
            HolsterPreferences[i].config = config.HolsterConfigs[i];
            HolsterPreferences[i].UpdatePreferences();
        }
    }

    public void UpdatePreferences()
    {
        ScalePreference.UpdatePreferences();

        foreach (var preference in HolsterPreferences)
        {
            preference.UpdatePreferences();
        }
    }

    public void ResetPreferences()
    {
        ScalePreference.SetValue(true);

        foreach (var preference in HolsterPreferences)
        {
            preference.SetVisibility(HolsterVisibility.DEFAULT);
        }

        HolsterHiderMod.UpdateHolsters();
    }
}
