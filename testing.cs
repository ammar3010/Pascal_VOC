using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

[TestClass]
public class WebsiteTests
{
    private static IWebDriver driver;
    private static WebDriverWait wait;

    [ClassInitialize]
    public static void Setup(TestContext context)
    {
        driver = new ChromeDriver();
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    [ClassCleanup]
    public static void Cleanup()
    {
        driver.Quit();
    }

    [TestMethod]
    public void TestWebsiteIsAccessible()
    {
        driver.Navigate().GoToUrl("https://eshoponweb-devops-canary.azurewebsites.net/");
        Assert.IsTrue(driver.Title.Contains("eShopOnWeb"));
    }

    [TestMethod]
    public void TestWebsiteHasCorrectBranding()
    {
        driver.Navigate().GoToUrl("https://eshoponweb-devops-canary.azurewebsites.net/");
        IWebElement logo = driver.FindElement(By.CssSelector("a.navbar-brand > img"));
        Assert.IsTrue(logo.GetAttribute("src").Contains("logo.png"));
    }

    [TestMethod]
    public void TestNavigationMenu()
    {
        driver.Navigate().GoToUrl("https://eshoponweb-devops-canary.azurewebsites.net/");
        IWebElement menu = driver.FindElement(By.CssSelector("ul.navbar-nav"));
        Assert.IsNotNull(menu);
        ReadOnlyCollection<IWebElement> menuItems = menu.FindElements(By.CssSelector("li.nav-item"));
        foreach (IWebElement menuItem in menuItems)
        {
            menuItem.Click();
            Assert.IsTrue(driver.Url.Contains(menuItem.GetAttribute("data-page")));
        }
    }

    [TestMethod]
    public void TestSearchFunctionality()
    {
        driver.Navigate().GoToUrl("https://eshoponweb-devops-canary.azurewebsites.net/");
        IWebElement searchBox = driver.FindElement(By.Id("search-box"));
        searchBox.SendKeys("camera");
        searchBox.SendKeys(Keys.Enter);
        IWebElement searchResult = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".card-title")));
        Assert.IsTrue(searchResult.Text.Contains("Camera"));
    }

    [TestMethod]
    public void TestProductDisplay()
    {
        driver.Navigate().GoToUrl("https://eshoponweb-devops-canary.azurewebsites.net/");
        IWebElement productSection = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".card-deck")));
        ReadOnlyCollection<IWebElement> products = productSection.FindElements(By.CssSelector(".card"));
        Assert.IsTrue(products.Count > 0);
    }

    [TestMethod]
    public void TestProductDetailsPage()
    {
        driver.Navigate().GoToUrl("https://eshoponweb-devops-canary.azurewebsites.net/");
        IWebElement productSection = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".card-deck")));
        IWebElement product = productSection.FindElement(By.CssSelector(".card"));
        string productName = product.FindElement(By.CssSelector(".card-title")).Text;
        product.Click();
        IWebElement productDetails = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".product-detail")));
        Assert.IsTrue(productDetails.Text.Contains(productName));
    }

    [TestMethod]
    public void TestShoppingCart()
    {
        driver.Navigate().GoToUrl("https://eshoponweb-devops-canary.azurewebsites.net/");
        IWebElement productSection = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".card-deck")));
        IWebElement product = productSection.FindElement(By.CssSelector(".card"));
        string productName = product.FindElement(By.CssSelector(".card-title")).Text;
        product.Click();
        IWebElement addToCartButton = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".btn-add-to-cart")));
        addToCartButton.Click();
        IWebElement cartLink = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".cart a")));
        cartLink.Click();
        IWebElement cartItemsSection = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".cart-items")));
        ReadOnlyCollection<IWebElement> cartItems = cartItemsSection.FindElements(By.CssSelector(".cart-item"));
        Assert.IsTrue(cartItems.Count == 1);
        Assert.IsTrue(cartItems[0].Text.Contains(productName));
    }
}

