using Terminal.Gui;
using System;

namespace AutoSetup
{
    public partial class Locker
    {
        private static Window mainWindow;

        private int leftTryCount;

        public static void Lock(Window main)
        {
            mainWindow = main;
            Application.Run(new Locker());
        }

        private Locker()
        {
            InitializeComponent();
            ClearKeybindings();
            passCheck.Clicked += Check;
            leftTryCount = Env.MAX_COUNT_TRY;
        }

        private void Check()
        {
            passInput.SetFocus();
            if (passInput.Text == Env.PASSWORD)
            {
                Program.ExitToplevel();
                Application.Run(mainWindow);
            }
            leftTryCount--;
            if (leftTryCount == 0)
            {
                Console.Clear();
                Environment.Exit(1);
            }
            passInput.Text = "";
            tryInfo.Text = $"У вас осталось {leftTryCount} попыт{(leftTryCount > 1 ? "ки" : "ка")}";
        }
    }
}