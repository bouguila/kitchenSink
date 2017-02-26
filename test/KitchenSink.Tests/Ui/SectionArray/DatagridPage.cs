using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace KitchenSink.Tests.Ui.SectionArray
{
    public class DatagridPage : BasePage
    {
        public DatagridPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//button[text() = 'Add a pet']")]
        public IWebElement AddPetButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//table[@class='htCore']//tbody//tr")]
        public IList<IWebElement> PetsTableRows { get; set; }

        public void AddPet()
        {
            ClickOn(AddPetButton);
        }
    }
}
