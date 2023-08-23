namespace Librarian.Core
{
    public static class CommonUtils
    {
        public static Stream LoadStream(Type type, string resourceName)
        {
            var assembly = type.Assembly;
            var resourceNames = assembly.GetManifestResourceNames();
#pragma warning disable CS8603 // Possible null reference return.
            return resourceNames.Contains(resourceName)
                ? assembly.GetManifestResourceStream(resourceName)
                : throw new ArgumentException($"Resource name {resourceName} not found in the assembly {assembly.FullName}", nameof(resourceName));
#pragma warning restore CS8603 // Possible null reference return.
        }

        public static string GetName<T>()
            where T : IStaticInterface
        {
            return T.Name;
        }
    }
}