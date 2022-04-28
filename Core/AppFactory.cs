using FlaUI.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlaUIPractice.Core
{
    public static class AppFactory
    {
        private static Application instance;

        public static Application Instance()
        {
            if (instance == null)
            {
                instance = Application.Launch("notepad.exe");
                // some logic
                return instance;
            }    

            return instance;
        }

        public static void QuitApp()
        {
            instance.Close(killIfCloseFails: true);
            instance = null;
        }
    }
}
