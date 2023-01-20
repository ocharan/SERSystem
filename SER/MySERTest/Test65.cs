using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MySER;

public class Test65
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
    public void test65()
    {
        
        driver.Navigate().GoToUrl("https://localhost:7173/");
        driver.Manage().Window.Size = new System.Drawing.Size(1846, 1053);
        driver.FindElement(By.Id("inputUsuario")).SendKeys("060606");
        driver.FindElement(By.Id("inputContraseña")).SendKeys("1234");
        driver.FindElement(By.Id("btnIniciarSesion")).Click();
        driver.FindElement(By.Id("btnExpedientes")).Click();

        driver.FindElement(By.Id("btnDocumento&S19029349")).Click();
        
        driver.FindElement(By.Id("nombreList")).Click();
        {
            var dropdown = driver.FindElement(By.Id("nombreList"));
            dropdown.FindElement(By.XPath("//option[. = 'Anteproyecto']")).Click();
        }
        driver.FindElement(By.Id("documento_Notas")).SendKeys("Este es el documento del anteproyecto");
        IWebElement element = driver.FindElement(By.XPath("//input[@type='file']"));
        element.SendKeys("/home/josuecg/AUTOMATING_RESTAPI.pdf");
        driver.FindElement(By.Id("btnGuardarDocumentoExperiencia")).Click();
        

    }
}