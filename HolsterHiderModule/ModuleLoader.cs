using LabFusion.SDK.Modules;

namespace HolsterHiderModule;

public static class ModuleLoader
{
    public static void LoadModule()
    {
        ModuleManager.RegisterModule<HolsterHiderModule>();
    }
}