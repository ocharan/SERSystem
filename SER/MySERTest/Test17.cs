using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MySER;

public class Test17
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
    public void test17()
    {
        driver.Navigate().GoToUrl("https://localhost:7173/");
        driver.Manage().Window.Size = new System.Drawing.Size(1846, 1053);
        driver.FindElement(By.Id("inputUsuario")).SendKeys("josuecapi");
        driver.FindElement(By.Id("inputContraseña")).SendKeys("1234");
        driver.FindElement(By.Id("btnIniciarSesion")).Click();
        driver.FindElement(By.Id("btnTiposDocumento")).Click();
        driver.FindElement(By.Id("btnRegistrarTipo")).Click();
        driver.FindElement(By.Id("nombreList")).Click();
        {
            var dropdown = driver.FindElement(By.Id("nombreList"));
            dropdown.FindElement(By.XPath("//option[. = 'Proyecto guíado']")).Click();
        }
        
        driver.FindElement(By.Id("inputNombreTipo")).SendKeys("Reporte de la RSL");
        driver.FindElement(By.Id("btnGuardarTipoDocumento")).Click();
    }
}