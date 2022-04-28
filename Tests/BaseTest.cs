using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using FlaUIPractice.Core;
using NUnit.Framework;

namespace FlaUIPractice.Tests
{
    [TestFixture]
    public class BaseTest
    {
        protected ConditionFactory conditionFactory;

        [SetUp]
        public void Setup()
        {
            AppWindowHelper.AppWindow = AppFactory.Instance();
            conditionFactory = new ConditionFactory(new UIA3PropertyLibrary());
        }

        [TearDown]
        public void TearDown()
        {
            AppFactory.QuitApp();
        }
    }
}
