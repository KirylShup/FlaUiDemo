using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using FlaUIPractice.Core;
using FlaUIPractice.Pages;
using NUnit.Framework;
using System;

namespace FlaUIPractice.Tests
{
    [TestFixture]
    public class BaseTest
    {
        protected Application app;
        protected ConditionFactory conditionFactory;
        protected MainPage mainPage;

        [SetUp]
        public void Setup()
        {
            app = Application.Launch(@"C:\Users\Kiryl_Shupenich\AppData\Roaming\Spotify\Spotify.exe");
            app.WaitWhileBusy(TimeSpan.FromSeconds(10));
            AppWindowHelper.AppWindow = app.GetMainWindow(new UIA3Automation()).WaitUntilEnabled(TimeSpan.FromSeconds(10));
            conditionFactory = new ConditionFactory(new UIA3PropertyLibrary());
        }

        [TearDown]
        public void TearDown()
        {
            app.Close();
        }
    }
}
