using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;

namespace MySER;

public class Test63
{
    private IWebDriver driver;
    public IDictionary<string, object> vars {get; private set;}
    private IJavaScriptExecutor js;
    [SetUp]
    public void SetUp() {
        FirefoxOptions firefoxOptions = new FirefoxOptions();
        firefoxOptions.AcceptInsecureCertificates = true;
        driver = new FirefoxDriver(firefoxOptions);
        js = (IJavaScriptExecutor)driver;
        vars = new Dictionary<string, object>();
    }
    [TearDown]
    protected void TearDown() {
        driver.Quit();
    }

    [Test]
    public void test63()
    {
        
        driver.Navigate().GoToUrl("https://localhost:7173/");
        driver.Manage().Window.Size = new System.Drawing.Size(1846, 1053);
        driver.FindElement(By.Id("inputUsuario")).SendKeys("josuecg");
        driver.FindElement(By.Id("inputContraseña")).SendKeys("1234");
        driver.FindElement(By.Id("btnIniciarSesion")).Click();
        driver.FindElement(By.Id("btnExperienciaRecepcional")).Click();
        driver.FindElement(By.Id("btnTrabajos")).Click();
        
        driver.FindElement(By.Id("btnAsinarEstudiantes&2017")).Click();
        
        var element = driver.FindElement(By.Id("btn_Guardar"));
        element.Click();
        driver.FindElement(By.Id("btnAsignarS18017548")).Click();
        driver.FindElement(By.Id("btnAsignarS18019640")).Click();
        driver.FindElement(By.Id("btnAsignarS19029349")).Click();
        

        js.ExecuteScript("window.scrollBy(0,document.body.scrollHeight)");
        element.Click();

    }
}