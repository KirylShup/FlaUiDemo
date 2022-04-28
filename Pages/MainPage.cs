using FlaUI.Core.AutomationElements;
using FlaUIPractice.Core;

namespace FlaUIPractice.Pages
{
    public class MainPage : BasePage
    {
        private protected static Window _appWindow;
        private static MainPage _mainPage;
        private protected MainPage()
        {
            _appWindow = AppWindowHelper.AppWindow;
        }

        public static new MainPage Instance() => _mainPage ?? new MainPage();

        public Elements PageElements => new Elements();
        public sealed class Elements
        {
            public string CreateNewPalylistButton => "//Button[@Name = 'Create Playlist']";
            public string EditPlaylistDetailsButton => "//Button[contains(@Name, 'My Playlist')]";
            public string MoreOptionsForPlaylistButton => "//MenuItem[contains(@Name, 'More options for My Playlist #')]";
            public string DeletePlaylistOptionInDropdownMenu => "//MenuItem[contains(@Name, 'Delete')]";
            public string DeletePlaylistButtonInConfirmDialog => "//Custom/Button[contains(@Name, 'DELETE')]";
        }
    }
}
