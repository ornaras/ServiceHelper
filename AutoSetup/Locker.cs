using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;

namespace AutoSetup
{
    public class Locker
    {
        public const uint COUNT_TRIES = 3;
        private const string CLASS_NAME = "AutoSetup-Locker";
        private const string VALID_PASSWORD = "Pi141592";

        private uint leftTries;
        private bool success;

        private readonly HWND handle;
        private readonly nint hTextBox;
        private readonly nint hButton;

        public Locker()
        {
            leftTries = COUNT_TRIES;
            var ptrClassName = Marshal.StringToHGlobalUni(CLASS_NAME);
            var ptrTitle = Marshal.StringToHGlobalUni("Блокировка установщика");
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
                    var x = (screen.Width - 250) / 2;
                    var y = (screen.Height - 100) / 2;
                    handle = PInvoke.CreateWindowEx(
                        0, new PCWSTR((char*)ptrClassName), new PCWSTR((char*)ptrTitle),
                        WINDOW_STYLE.WS_OVERLAPPED | (WINDOW_STYLE)0x0111, x, y, 250, 100, new HWND(nint.Zero),
                        HMENU.Null, Program.Handle);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptrClassName);
                Marshal.FreeHGlobal(ptrTitle);
            }
            hTextBox = AddTextBox(new RECT(6, 33, 142, 55));
            hButton = AddButton("Проверить", new RECT(148, 33, 228, 55));
        }

        private LRESULT WndProc(HWND hWnd, uint msg, WPARAM wParam, LPARAM lParam)
        {
            switch (msg)
            {
                case 0x000F:
                    var hdc = PInvoke.BeginPaint(handle, out var paint);
                    var rect = new RECT(new Point(0, 6), new Size(250, 20));
                    DrawText(hdc, $"Осталось {leftTries} попыток", ref rect,
                        DRAW_TEXT_FORMAT.DT_CENTER | DRAW_TEXT_FORMAT.DT_VCENTER);
                    PInvoke.EndPaint(handle, paint);
                    return new LRESULT(0);
                case 0x0111:
                    if (lParam == hButton) Check();
                    return new LRESULT(0);
                case 0x0002:
                    if (success) 
                        Debug.WriteLine("Правильный пароль"); // TODO: Заглушка
                    return new LRESULT(0);
                default:
                    return PInvoke.DefWindowProc(hWnd, msg, wParam, lParam);
            }
        }

        private void Check()
        {
            var password = new Span<char>(new char[VALID_PASSWORD.Length + 1], 0, VALID_PASSWORD.Length + 1);
            var hWnd = new HWND(hTextBox);
            PInvoke.GetWindowText(hWnd, password);
            success = password.ToString()[..^1] == VALID_PASSWORD;
            if (!success)
            {
                PInvoke.SetWindowText(hWnd, "");
                leftTries--;
                unsafe {
                    PInvoke.RedrawWindow(handle, null, HRGN.Null, REDRAW_WINDOW_FLAGS.RDW_ERASENOW | REDRAW_WINDOW_FLAGS.RDW_INVALIDATE);
                }
            } 
            if (leftTries == 0 || success) PInvoke.DestroyWindow(handle);
        }

        public void Show()
        {
            PInvoke.ShowWindow(handle, SHOW_WINDOW_CMD.SW_NORMAL);
            while (PInvoke.GetMessage(out var msg, handle, 0, 0) > 0)
            {
                PInvoke.TranslateMessage(msg);
                PInvoke.DispatchMessage(msg);
            }
        }

        private static void DrawText(HDC hdc, string text, ref RECT rect, DRAW_TEXT_FORMAT format)
        {
            var ptr = Marshal.StringToHGlobalUni(text);
            unsafe
            {
                PInvoke.DrawText(hdc, new PCWSTR((char*)ptr), -1, ref rect, format);
            }
            Marshal.FreeHGlobal(ptr);
        }

        private nint AddButton(string text, RECT rect)
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

        private nint AddTextBox(RECT rect)
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