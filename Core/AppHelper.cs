using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.System.Profile;

namespace Get.the.solution.UWP.XAML
{
    public static class AppHelper
    {
        public static String GetAppVersion()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;
            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }
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

        public static ulong GetWindowsBuildNumber(ulong version)
        {
            return (version & 0x00000000FFFF0000L) >> 16;
        }

        private static ulong GetWindowsRevisionNumber(ulong version)
        {
            return (version & 0x000000000000FFFFL);
        }

        public static ulong GetWindowsDeviceVersion()
        {
            string deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong version = ulong.Parse(deviceFamilyVersion);
            return version;
        }

    }
}
