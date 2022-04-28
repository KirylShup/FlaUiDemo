using NUnit.Framework;
using FlaUIPractice.Tests;
using FlaUIPractice.Pages;

namespace FlaUIPractice
{
    [TestFixture]
    public class Test : BaseTest
    {

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
    }
}