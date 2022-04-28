using FlaUI.Core.AutomationElements;
using FlaUIPractice.Core;
using FlaUIPractice.Helpers;
using System;

namespace FlaUIPractice.Pages
{
    public class BasePage
    {
        private protected static Window _appWindow;
        private static BasePage basePage;
        private protected BasePage()
        {
            _appWindow = AppWindowHelper.AppWindow;
        }

        public static BasePage Instance() => basePage ?? new BasePage();


        public void ClickPageElements(string Xpath)
        {
            var element = Utils.WaitForElement(() => _appWindow.FindFirstByXPath(Xpath), TimeSpan.FromSeconds(15));
            element.ClickElement();
        }

        public void WaitForElement(string Xpath)
        {
            Utils.WaitForElement(() => _appWindow.FindFirstByXPath(Xpath), TimeSpan.FromSeconds(15));
        }

        public AutomationElement FindElementByXpath (string Xpath)
        {
            Utils.WaitForElement(() => _appWindow.FindFirstByXPath (Xpath), TimeSpan.FromSeconds(15));
            return _appWindow.FindFirstByXPath(Xpath);
        }
    }
}
