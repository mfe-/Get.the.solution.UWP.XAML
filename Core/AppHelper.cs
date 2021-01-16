using System;
using Windows.ApplicationModel;
using Windows.System.Profile;

namespace Get.the.solution.UWP.XAML
{
    /// <summary>
    /// Common App helpers like receiving app or os versoin
    /// </summary>
    public static class AppHelper
    {
        /// <summary>
        /// The current app package version with format major.minor.build.revision
        /// </summary>
        /// <returns></returns>
        public static String GetAppVersion()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;
            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }
        /// <summary>
        /// The complete OS version major.minor.build.revision - for example "10.0.18363.1198"
        /// </summary>
        /// <returns></returns>
        public static string GetWindowsVersion()
        {
            ulong version = GetWindowsDeviceVersion();
            ulong major = (version & 0xFFFF000000000000L) >> 48;
            ulong minor = (version & 0x0000FFFF00000000L) >> 32;
            ulong build = GetWindowsBuildNumber(version);
            ulong revision = GetWindowsRevisionNumber(version);
            var osVersion = $"{major}.{minor}.{build}.{revision}";
            return osVersion;
        }
        /// <summary>
        /// OS build - for example 18363 <seealso href="https://en.wikipedia.org/wiki/Template:Windows_10_versions"/>
        /// </summary>
        /// <returns></returns>
        public static ulong GetWindowsBuildNumber()
        {
            return GetWindowsBuildNumber(GetWindowsDeviceVersion());
        }
        public static ulong GetWindowsBuildNumber(ulong version)
        {
            return (version & 0x00000000FFFF0000L) >> 16;
        }

        private static ulong GetWindowsRevisionNumber(ulong version)
        {
            return (version & 0x000000000000FFFFL);
        }
        /// <summary>
        /// 2814750970545326
        /// </summary>
        /// <returns></returns>
        public static ulong GetWindowsDeviceVersion()
        {
            string deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong version = ulong.Parse(deviceFamilyVersion);
            return version;
        }

    }
}
