using Terminal.Gui;

namespace AutoSetup
{
    public static class Program
    {
        private static void Main()
        {
            Application.Init();

            try
            {
                Locker.Lock(new MyView());
            }
            finally
            {
                Application.Shutdown();
            }
        }

        public static void ExitToplevel()
        {
            if (Application.Top.IsMdiChild) 
                Application.Top.RequestStop();
            else if (Application.MdiTop != null)
                Application.MdiTop.RequestStop();
            else Application.RequestStop();
        }
    }
}