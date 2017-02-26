using KitchenSink.Tests.Ui;
using KitchenSink.Tests.Ui.SectionArray;
using KitchenSink.Tests.Utilities;
using NUnit.Framework;

namespace KitchenSink.Tests.Test.SectionArray
{
    [TestFixture(Config.Browser.Chrome)]
    [TestFixture(Config.Browser.Edge)]
    [TestFixture(Config.Browser.Firefox)]
    class DatagridPageTest : BaseTest
    {
        private DatagridPage _datagridPage;
        private MainPage _mainPage;

        public DatagridPageTest(Config.Browser browser) : base(browser)
        {
        }

        [SetUp]
        public void SetUp()
        {
            _mainPage = new MainPage(Driver).GoToMainPage();
            _datagridPage = _mainPage.GoToDataGridPage();
        }

        [Test]
        public void TablePage_AddNewRow()
        {
            var rowsBefore = _datagridPage.PetsTableRows.Count;
            _datagridPage.AddPet();
            var rowsAfter = _datagridPage.PetsTableRows.Count;
            Assert.Greater(rowsAfter, rowsBefore);
        }
    }
}
