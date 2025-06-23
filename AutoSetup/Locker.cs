using System;
using System.Diagnostics;
using System.Drawing;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;

namespace AutoSetup
{
    internal class Locker : WindowBase
    {
        public const uint COUNT_TRIES = 3;
        private const string CLASS_NAME = "AutoSetup-Locker";
        private const string VALID_PASSWORD = "Pi141592";

        private uint leftTries;
        private bool success;

        private readonly nint hTextBox;
        private readonly nint hButton;

        public Locker() : base(CLASS_NAME, "Блокировка установщика", new Size(250, 100))
        {
            leftTries = COUNT_TRIES;
            hTextBox = AddTextBox(new RECT(6, 33, 142, 55));
            hButton = AddButton("Проверить", new RECT(148, 33, 228, 55));
        }

        protected override LRESULT WndProc(HWND hWnd, uint msg, WPARAM wParam, LPARAM lParam)
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
            }
            return base.WndProc(hWnd, msg, wParam, lParam);
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
    }
}