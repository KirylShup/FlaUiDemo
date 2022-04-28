using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using FlaUIPractice.Data;
using System;

namespace FlaUIPractice.Core
{
    public static class AppFactory
    {
        private static Window instance;

        public static Window Instance()
        {
            if (instance == null)
            {
                var app = Application.Launch(AppData.AppPath);
                app.WaitWhileBusy(TimeSpan.FromSeconds(10));
                instance = app.GetMainWindow(new UIA3Automation()).WaitUntilEnabled(TimeSpan.FromSeconds(10));
                return instance;
            }    

            return instance;
        }

        public static void QuitApp()
        {
            instance.Close();
            instance = null;
        }
    }
}
