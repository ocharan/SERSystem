using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MySER;

public class Test12
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
    public void test12()
    {
        driver.Navigate().GoToUrl("https://localhost:7173/");
        driver.Manage().Window.Size = new System.Drawing.Size(1846, 1053);
        driver.FindElement(By.Id("inputUsuario")).SendKeys("josuecapi");
        driver.FindElement(By.Id("inputContraseña")).SendKeys("1234");
        driver.FindElement(By.Id("btnIniciarSesion")).Click();
        driver.FindElement(By.Id("btnDocentes")).Click();
        driver.FindElement(By.Id("12345")).Click();
        driver.FindElement(By.Id("inputNombre")).Clear();
        driver.FindElement(By.Id("inputApellidoPaterno")).Clear();
        driver.FindElement(By.Id("inputApellidoMaterno")).Clear();
        driver.FindElement(By.Id("input_numeroPersonal")).Clear();
        
        driver.FindElement(By.Id("inputNombre")).SendKeys("Juan Pablo");
        driver.FindElement(By.Id("inputApellidoPaterno")).SendKeys("Gonzalez");
        driver.FindElement(By.Id("inputApellidoMaterno")).SendKeys("Del Valle");
        driver.FindElement(By.Id("input_numeroPersonal")).SendKeys("12345");
        driver.FindElement(By.Id("btnGuardarDocente")).Click();
    }
}