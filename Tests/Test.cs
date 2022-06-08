using NUnit.Framework;
using FlaUIPractice.Tests;
using FlaUIPractice.Pages;
using FlaUI.Core.Capturing;
using FlaUIPractice.Core;
using System.Windows.Media.Imaging;
using System;
using FlaUIPractice.Helpers;

namespace FlaUIPractice
{
    [TestFixture]
    public class Test : BaseTest
    {
        private const string resourcePath = @"C:\Users\Kiryl_Shupenich\Documents\FlaUiDemo\Resources\resource1.png";
        private const double allowedPercetage = 1.0;

        [Test]
        public void CreateAndDeleteNewPlaylistInSpotify()
        {
            MainPage.Instance().ClickPageElements(MainPage.Instance().PageElements.CreateNewPalylistButton);
            var label = MainPage.Instance().FindElementByXpath(MainPage.Instance().PageElements.EditPlaylistDetailsButton);
            Assert.IsTrue(label.IsAvailable);

            MainPage.Instance().ClickPageElements(MainPage.Instance().PageElements.MoreOptionsForPlaylistButton);
            MainPage.Instance().ClickPageElements(MainPage.Instance().PageElements.DeletePlaylistOptionInDropdownMenu);
            MainPage.Instance().ClickPageElements(MainPage.Instance().PageElements.DeletePlaylistButtonInConfirmDialog);
        }

        [Test]
        [TestCase(resourcePath)]
        public void ScreenshotTest(string path)
        {
            MainPage.Instance().ClickPageElements(MainPage.Instance().PageElements.CreateNewPalylistButton);
            var label = MainPage.Instance().FindElementByXpath(MainPage.Instance().PageElements.EditPlaylistDetailsButton);
            Assert.IsTrue(label.IsAvailable);

            using (var capture = Capture.Element(AppWindowHelper.AppWindow))
            {
                var actualBitmap = Utils.ConvertToWritableBitmap(capture);

                WriteableBitmap expectedBitmap = null;
                try
                {
                    expectedBitmap = Utils.LoadFromPng(path);
                }
                catch (Exception caught)
                {
                    throw new Exception("Unable to load image from resource " + path, caught);
                }
                Assert.True(Utils.CompareScreenshots(path, actualBitmap, expectedBitmap, allowedPercetage));
            }
        }
    }
}