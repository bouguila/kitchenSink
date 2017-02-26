using KitchenSink.Tests.Ui;
using KitchenSink.Tests.Ui.SectionBoolean;
using KitchenSink.Tests.Utilities;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

namespace KitchenSink.Tests.Test.SectionBoolean
{
    [TestFixture(Config.Browser.Chrome)]
    [TestFixture(Config.Browser.Edge)]
    [TestFixture(Config.Browser.Firefox)]
    class CheckboxPageTest : BaseTest
    {
        private CheckboxPage _checkboxPage;
        private MainPage _mainPage;

        public CheckboxPageTest(Config.Browser browser) : base(browser)
        {
        }

        [SetUp]
        public void SetUp()
        {
            _mainPage = new MainPage(Driver).GoToMainPage();
            _checkboxPage = _mainPage.GoToCheckboxPage();
        }

        [Test]
        public void CheckboxPage_CheckboxUncheckedAndCheckedAgain()
        {
            WaitUntil(x => _checkboxPage.Checkbox.Displayed);
            WaitUntil(x => _checkboxPage.InfoLabel.Displayed);
            Assert.IsTrue(WaitForText(_checkboxPage.InfoLabel, "You can drive", 5));
            _checkboxPage.ToggleCheckbox();
            Assert.IsTrue(WaitForText(_checkboxPage.InfoLabel, "You can't drive", 5));
            _checkboxPage.ToggleCheckbox();
            Assert.IsTrue(WaitForText(_checkboxPage.InfoLabel, "You can drive", 5));
        }
    }
}
