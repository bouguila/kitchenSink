using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace KitchenSink.Tests.Ui.SectionCustom
{
    public class FileUploadPage : BasePage
    {
        public FileUploadPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.Id, Using = "fileElement")]
        public IWebElement FileInput { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".alert-warning")]
        public IWebElement InfoLabel { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".kitchensink-test-uploaded-files-list")]
        public IList<IWebElement> UploadedFilesList { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".kitchensink-test-delete-button")]
        public IList<IWebElement> DeleteButtons { get; set; }

        public void UploadAFile(string filePath)
        {
            FileInput.SendKeys(filePath); //BUG in EDGE: https://developer.microsoft.com/en-us/microsoft-edge/platform/issues/7194303/
        }

        public int GetUploadedFilesCount()
        {
            return UploadedFilesList.Count;
        }

        public void DeleteAllFiles()
        {
            foreach (var deleteButton in DeleteButtons)
            {
                ClickOn(deleteButton);
            }
        }
    }
}
