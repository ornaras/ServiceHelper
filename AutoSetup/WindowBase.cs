using System.Runtime.InteropServices;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using System.Drawing;

namespace AutoSetup
{
    internal abstract class WindowBase
    {
        protected readonly HWND handle;

        public WindowBase(string className, string title, Size size)
        {
            var ptrClassName = Marshal.StringToHGlobalUni(className);
            var ptrTitle = Marshal.StringToHGlobalUni(title);
            try
            {
                unsafe
                {
                    var wndclass = new WNDCLASSW
                    {
                        lpszClassName = new PCWSTR((char*)ptrClassName),
                        lpfnWndProc = WndProc,
                        hInstance = Program.Handle,
                    };
                    PInvoke.RegisterClass(wndclass);
                    var screen = GetScreenSize();
                    var x = (screen.Width - size.Width) / 2;
                    var y = (screen.Height - size.Height) / 2;
                    handle = PInvoke.CreateWindowEx(
                        0, new PCWSTR((char*)ptrClassName), new PCWSTR((char*)ptrTitle),
                        WINDOW_STYLE.WS_OVERLAPPED | (WINDOW_STYLE)0x0111, x, y, size.Width, 
                        size.Height, new HWND(nint.Zero), HMENU.Null, Program.Handle);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptrClassName);
                Marshal.FreeHGlobal(ptrTitle);
            }
        }

        protected virtual LRESULT WndProc(HWND hWnd, uint msg, WPARAM wParam, LPARAM lParam) =>
            PInvoke.DefWindowProc(hWnd, msg, wParam, lParam);

        public void Show()
        {
            PInvoke.ShowWindow(handle, SHOW_WINDOW_CMD.SW_NORMAL);
            while (PInvoke.GetMessage(out var msg, handle, 0, 0) > 0)
            {
                PInvoke.TranslateMessage(msg);
                PInvoke.DispatchMessage(msg);
            }
        }

        protected static void DrawText(HDC hdc, string text, ref RECT rect, DRAW_TEXT_FORMAT format)
        {
            var ptr = Marshal.StringToHGlobalUni(text);
            unsafe
            {
                PInvoke.DrawText(hdc, new PCWSTR((char*)ptr), -1, ref rect, format);
            }
            Marshal.FreeHGlobal(ptr);
        }

        protected nint AddButton(string text, RECT rect)
        {
            var ptrClassName = Marshal.StringToHGlobalUni("BUTTON");
            var ptrText = Marshal.StringToHGlobalUni(text);
            try
            {
                unsafe
                {
                    return PInvoke.CreateWindowEx(0, new PCWSTR((char*)ptrClassName), new PCWSTR((char*)ptrText),
                        WINDOW_STYLE.WS_TABSTOP | WINDOW_STYLE.WS_VISIBLE | WINDOW_STYLE.WS_CHILD | (WINDOW_STYLE)0x00000001,
                        rect.X, rect.Y, rect.Width, rect.Height, handle, HMENU.Null, Program.Handle);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptrClassName);
                Marshal.FreeHGlobal(ptrText);
            }
        }

        protected nint AddTextBox(RECT rect)
        {
            var ptrClassName = Marshal.StringToHGlobalUni("EDIT");
            var ptrText = Marshal.StringToHGlobalUni("");
            try
            {
                unsafe
                {
                    return PInvoke.CreateWindowEx(0, new PCWSTR((char*)ptrClassName), new PCWSTR((char*)ptrText),
                        WINDOW_STYLE.WS_TABSTOP | WINDOW_STYLE.WS_VISIBLE | WINDOW_STYLE.WS_CHILD | (WINDOW_STYLE)(128 | 8388608),
                        rect.X, rect.Y, rect.Width, rect.Height, handle, HMENU.Null, Program.Handle);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptrClassName);
                Marshal.FreeHGlobal(ptrText);
            }
        }
    }
}
