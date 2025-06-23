using System.Drawing;
using Windows.Win32.UI.WindowsAndMessaging;

namespace AutoSetup
{
    public static class Utils
    {
        public static Size GetScreenSize() => new
        (
            PInvoke.GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CXFULLSCREEN),
            PInvoke.GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CYFULLSCREEN)
        );
    }
}
