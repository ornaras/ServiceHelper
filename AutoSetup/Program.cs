#pragma warning disable IDE0079

using System.Runtime.InteropServices;
using Windows.Win32.Foundation;

namespace AutoSetup
{
    public static class Program
    {
        internal static HINSTANCE Handle { get; private set; }
        private static void Main()
        {
#pragma warning disable IL3002
            Handle = new HINSTANCE(Marshal.GetHINSTANCE(typeof(Program).Module));
#pragma warning restore IL3002
            new Locker().Show();
        }
    }
}