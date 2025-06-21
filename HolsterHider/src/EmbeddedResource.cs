using System.Linq;

using System.Reflection;

using System.IO;

namespace HolsterHider;

public static class EmbeddedResource
{
    public static byte[] LoadBytesFromAssembly(Assembly assembly, string name)
    {
        string[] manifestResources = assembly.GetManifestResourceNames();

        if (!manifestResources.Contains(name))
        {
            return null;
        }

        using Stream str = assembly.GetManifestResourceStream(name);
        using MemoryStream memoryStream = new();

        str.CopyTo(memoryStream);

        return memoryStream.ToArray();
    }

    public static Assembly LoadAssemblyFromAssembly(Assembly assembly, string name)
    {
        var rawAssembly = LoadBytesFromAssembly(assembly, name);

        if (rawAssembly == null)
        {
            return null;
        }

        return Assembly.Load(rawAssembly);
    }
}