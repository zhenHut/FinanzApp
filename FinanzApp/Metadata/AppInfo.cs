
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FinanzApp.Metadata
{
    public static class AppInfo
    {
        public const string Product = "FinanzApp";
        public const string Company = "Andres Möring";
        public const int StartYear = 2025;

        public static string CurrentVersion =>
            Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
            ?? FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion
            ?? "1.0.0";

        // "1.3.0" (schneidet "-beta.1" und "+sha" ab)
        public static string SemVer
        {
            get
            {
                var info = Assembly.GetExecutingAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

                if (!string.IsNullOrWhiteSpace(info))
                {
                    var m = Regex.Match(info, @"^\d+\.\d+\.\d+"); // SemVer Kern
                    if (m.Success) return m.Value;
                }

                var fv = FileVersionInfo.GetVersionInfo(
                    Assembly.GetExecutingAssembly().Location).FileVersion ?? "1.0.0.0";
                // FileVersion ist vierteilig -> auf 3 Teile kürzen
                var parts = fv.Split('.');
                return $"{parts[0]}.{parts[1]}.{parts[2]}";
            }
        }

        public static Version SemVerVersion =>  Version.Parse(SemVer);
        public static string ProductWithVersion => $"{Product} v{SemVer}";

        public static string YearRange =>
            StartYear == DateTime.Now.Year ? $"{StartYear}" : $"{StartYear}–{DateTime.Now.Year}";
        public static string Copyright => $"© {YearRange} {Company}";
    }
}

